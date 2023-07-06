using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding;

public abstract class LoggedQueryHandler<TResponse> : IQuery<TResponse>
{
    protected LoggedQueryHandler(ILogger<LoggedQueryHandler<TResponse>> logger) => Logger = logger;

    protected ILogger<LoggedQueryHandler<TResponse>> Logger { get; }

    public abstract string CommandName { get; }

    public virtual Task<bool> CanExecute() => Task.FromResult(true);

    public async Task<TResponse> Execute()
    {
        Logger.LogInformation("Handling a {CommandName}", CommandName);

        if(!await CanExecute())
        {
            Logger.LogWarning("Cannot handle a {CommandName}", CommandName);
            OnCommandException();
        }

        try
        {
            TResponse result = await OnExecute();

            Logger.LogInformation("Handled a {CommandName}", CommandName);
            return result;
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Failed to handle a {CommandName}", CommandName);
            throw;
        }
    }

    protected abstract Task<TResponse> OnExecute();

    protected virtual void OnCommandException()
    {
        throw new CommandException(this);
    }
}

public abstract class LoggedQueryHandler<TRequest, TResponse> : IQuery<TRequest, TResponse> where TRequest : IRequest
{
    protected LoggedQueryHandler(ILogger<LoggedQueryHandler<TRequest, TResponse>> logger) => Logger = logger;

    protected ILogger<LoggedQueryHandler<TRequest, TResponse>> Logger { get; }

    public abstract string CommandName { get; }

    public abstract Task<bool> CanExecute(TRequest request);

    public async Task<TResponse> Execute(TRequest request)
    {
        Logger.LogInformation("{CommandName}: handling a {RequestType} with {@Request}", CommandName, typeof(TRequest).Name, request);

        if(!await CanExecute(request))
        {
            Logger.LogWarning("{CommandName}: cannot handle a {RequestType} with {@Request}", CommandName, typeof(TRequest).Name, request);
            OnCommandException(request);
        }

        try
        {
            TResponse result = await OnExecute(request);

            Logger.LogInformation("{CommandName}: handled a {RequestType} with {@Request}", CommandName, typeof(TRequest).Name, request);
            return result;
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "{CommandName}: failed to handle a {RequestType} with {@Request}", CommandName, typeof(TRequest).Name, request);
            throw;
        }
    }

    protected abstract Task<TResponse> OnExecute(TRequest request);

    protected virtual void OnCommandException(TRequest request)
    {
        throw new CommandException(this);
    }
}