using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding;

public abstract class LoggedCommandService : ICommand
{
    protected LoggedCommandService(ILogger<LoggedCommandService> logger) => Logger = logger;

    protected ILogger<LoggedCommandService> Logger { get; }

    public abstract string CommandName { get; }

    public virtual Task<bool> CanExecute() => Task.FromResult(true);

    public async Task Execute()
    {
        Logger.LogInformation("Executing a {CommandName}", CommandName);

        if(!await CanExecute())
        {
            Logger.LogWarning("Cannot execute a {CommandName}", CommandName);
            throw new CommandException(this);
        }

        try
        {
            await OnExecute();

            Logger.LogInformation("Executed a {CommandName}", CommandName);
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Failed to execute a {CommandName}", CommandName);
            throw;
        }
    }

    protected abstract Task OnExecute();
}

public abstract class LoggedCommandService<TParameters> : ICommand<TParameters> where TParameters : IRequest
{
    protected LoggedCommandService(ILogger<LoggedCommandService<TParameters>> logger) => Logger = logger;

    protected ILogger<LoggedCommandService<TParameters>> Logger { get; }

    public abstract string CommandName { get; }

    public abstract Task<bool> CanExecute(TParameters parameters);

    public async Task Execute(TParameters parameters)
    {
        Logger.LogInformation("Executing a {CommandName} with {@Parameters}", CommandName, parameters);

        if(!await CanExecute(parameters))
        {
            Logger.LogWarning("Cannot execute a {CommandName} with {@Parameters}", CommandName, parameters);
            OnCommandException(parameters);
        }

        try
        {
            await OnExecute(parameters);

            Logger.LogInformation("Executed a {CommandName} with {@Parameters}", CommandName, parameters);
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Failed to execute a {CommandName} with {@Parameters}", CommandName, parameters);
            throw;
        }
    }

    protected abstract Task OnExecute(TParameters parameters);

    protected virtual void OnCommandException(TParameters parameters)
    {
        throw new CommandException(this);
    }
}