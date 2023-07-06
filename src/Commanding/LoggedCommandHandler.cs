using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding;

public abstract class LoggedCommandHandler : ICommand
{
    protected LoggedCommandHandler(ILogger<LoggedCommandHandler> logger) => Logger = logger;

    protected ILogger<LoggedCommandHandler> Logger { get; }

    public abstract string CommandName { get; }

    public virtual Task<bool> CanExecute() => Task.FromResult(true);

    public async Task Execute()
    {
        Logger.LogInformation("Handling a {CommandName}", CommandName);

        if(!await CanExecute())
        {
            Logger.LogWarning("Cannot handle a {CommandName}", CommandName);
            throw new CommandException(this);
        }

        try
        {
            await OnExecute();

            Logger.LogInformation("Handled a {CommandName}", CommandName);
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Failed to handle a {CommandName}", CommandName);
            throw;
        }
    }

    protected abstract Task OnExecute();
}

public abstract class LoggedCommandHandler<TRequest> : ICommand<TRequest> where TRequest : IRequest
{
    protected LoggedCommandHandler(ILogger<LoggedCommandHandler<TRequest>> logger) => Logger = logger;

    protected ILogger<LoggedCommandHandler<TRequest>> Logger { get; }

    public abstract string CommandName { get; }

    public abstract Task<bool> CanExecute(TRequest request);

    public async Task Execute(TRequest request)
    {
        Logger.LogInformation("{CommandName}: handling a {RequestType} with {@Request}", CommandName, typeof(TRequest).Name, request);

        if(!await CanExecute(request))
        {
            Logger.LogWarning("{CommandName}: cannot handle a {RequestType} with {@Request}", CommandName, typeof(TRequest).Name, request);
            OnCommandException(request);
        }

        try
        {
            await OnExecute(request);

            Logger.LogInformation("{CommandName}: handled a {RequestType} with {@Request}", CommandName, typeof(TRequest).Name, request);
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "{CommandName}: failed to handle a {RequestType} with {@Request}", CommandName, typeof(TRequest).Name, request);
            throw;
        }
    }

    protected abstract Task OnExecute(TRequest request);

    protected virtual void OnCommandException(TRequest request)
    {
        throw new CommandException(this);
    }
}