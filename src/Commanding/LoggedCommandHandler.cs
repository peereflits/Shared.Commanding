using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding;

public abstract class LoggedCommandHandler : ICommand
{
    private readonly string commandName;

    protected LoggedCommandHandler(ILogger<LoggedCommandHandler> logger)
    {
        Logger = logger;
        commandName = GetType().Name;
    }

    protected ILogger<LoggedCommandHandler> Logger { get; }

    public virtual Task<bool> CanExecute() => Task.FromResult(true);

    public async Task Execute()
    {
        if(Logger.IsEnabled(LogLevel.Debug) || Logger.IsEnabled(LogLevel.Trace))
        {
            Logger.LogInformation("Handling a {CommandName}", commandName);
        }

        if(!await CanExecute())
        {
            Logger.LogWarning("Cannot handle a {CommandName}", commandName);
            throw new CommandException(this);
        }

        try
        {
            await OnExecute();

            if(Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation("Handled a {CommandName}", commandName);
            }
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Failed to handle a {CommandName}", commandName);
            throw;
        }
    }

    protected abstract Task OnExecute();
}

public abstract class LoggedCommandHandler<TRequest> : ICommand<TRequest> where TRequest : IRequest
{
    private readonly string commandName;

    protected LoggedCommandHandler(ILogger<LoggedCommandHandler<TRequest>> logger)
    {
        Logger = logger;
        commandName = GetType().Name;
    }

    protected ILogger<LoggedCommandHandler<TRequest>> Logger { get; }

    public virtual Task<bool> CanExecute(TRequest request) => Task.FromResult(true);

    public async Task Execute(TRequest request)
    {
        if(Logger.IsEnabled(LogLevel.Debug) || Logger.IsEnabled(LogLevel.Trace))
        {
            Logger.LogInformation("{CommandName}: handling a {RequestType} with {@Request}", commandName, typeof(TRequest).Name, request);
        }

        if(!await CanExecute(request))
        {
            Logger.LogWarning("{CommandName}: cannot handle a {RequestType} with {@Request}", commandName, typeof(TRequest).Name, request);
            OnCommandException(request);
        }

        try
        {
            await OnExecute(request);

            if(Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation("{CommandName}: handled a {RequestType} with {@Request}", commandName, typeof(TRequest).Name, request);
            }
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "{CommandName}: failed to handle a {RequestType} with {@Request}", commandName, typeof(TRequest).Name, request);
            throw;
        }
    }

    protected abstract Task OnExecute(TRequest request);

    protected virtual void OnCommandException(TRequest request)
    {
        throw new CommandException<TRequest>(this);
    }
}