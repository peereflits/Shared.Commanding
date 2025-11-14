using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding;

public abstract class LoggedCommandService : ICommand
{
    private readonly string commandName ;

    protected LoggedCommandService(ILogger<LoggedCommandService> logger)
    {
        Logger = logger;
        commandName = GetType().Name;
    }

    protected ILogger<LoggedCommandService> Logger { get; }

    public virtual ValueTask<bool> CanExecute() => new(true);

    public async Task Execute()
    {
        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation("Executing a {CommandName}", commandName);
        }

        if(!await CanExecute())
        {
            Logger.LogWarning("Cannot execute a {CommandName}", commandName);
            throw new CommandException(this);
        }

        try
        {
            await OnExecute();

            if(Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation("Executed a {CommandName}", commandName);
            }
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Failed to execute a {CommandName}", commandName);
            throw;
        }
    }

    protected abstract Task OnExecute();
}

public abstract class LoggedCommandService<TParameters> : ICommand<TParameters> where TParameters : IRequest
{
    private readonly string commandName ;

    protected LoggedCommandService(ILogger<LoggedCommandService<TParameters>> logger)
    {
        Logger = logger;
        commandName = GetType().Name;
    }

    protected ILogger<LoggedCommandService<TParameters>> Logger { get; }

    public virtual ValueTask<bool> CanExecute(TParameters parameters) => new(true);

    public async Task Execute(TParameters parameters)
    {
        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation("Executing a {CommandName} with {@Parameters}", commandName, parameters);
        }

        if(!await CanExecute(parameters))
        {
            Logger.LogWarning("Cannot execute a {CommandName} with {@Parameters}", commandName, parameters);
            OnCommandException(parameters);
        }

        try
        {
            await OnExecute(parameters);

            if(Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation("Executed a {CommandName} with {@Parameters}", commandName, parameters);
            }
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Failed to execute a {CommandName} with {@Parameters}", commandName, parameters);
            throw;
        }
    }

    protected abstract Task OnExecute(TParameters parameters);

    protected virtual void OnCommandException(TParameters parameters)
    {
        throw new CommandException<TParameters>(this);
    }
}