using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding;

public abstract class LoggedQueryHandler<TResponse> : IQuery<TResponse>
{
    private readonly string queryName;

    protected LoggedQueryHandler(ILogger<LoggedQueryHandler<TResponse>> logger)
    {
        Logger = logger;
        queryName = GetType().Name;
    }

    protected ILogger<LoggedQueryHandler<TResponse>> Logger { get; }

    public virtual ValueTask<bool> CanExecute() => new(true);

    public async Task<TResponse> Execute()
    {
        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation("Handling a {CommandName}", queryName);
        }

        if(!await CanExecute())
        {
            Logger.LogWarning("Cannot handle a {CommandName}", queryName);
            OnCommandException();
        }

        try
        {
            TResponse result = await OnExecute();

            if(Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation("Handled a {CommandName}", queryName);
            }

            return result;
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Failed to handle a {CommandName}", queryName);
            throw;
        }
    }

    protected abstract Task<TResponse> OnExecute();

    protected virtual void OnCommandException()
    {
        throw new QueryException<TResponse>(this);
    }
}

public abstract class LoggedQueryHandler<TRequest, TResponse> : IQuery<TRequest, TResponse> where TRequest : IRequest
{
    private readonly string queryName;

    protected LoggedQueryHandler(ILogger<LoggedQueryHandler<TRequest, TResponse>> logger)
    {
        Logger = logger;
        queryName = GetType().Name;
    }

    protected ILogger<LoggedQueryHandler<TRequest, TResponse>> Logger { get; }

    public virtual ValueTask<bool> CanExecute(TRequest request) => new(true);

    public async Task<TResponse> Execute(TRequest request)
    {
        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation("{CommandName}: handling a {RequestType} with {@Request}", queryName, typeof(TRequest).Name, request);
        }

        if(!await CanExecute(request))
        {
            Logger.LogWarning("{CommandName}: cannot handle a {RequestType} with {@Request}", queryName, typeof(TRequest).Name, request);
            OnCommandException(request);
        }

        try
        {
            TResponse result = await OnExecute(request);

            if(Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation("{CommandName}: handled a {RequestType} with {@Request}", queryName, typeof(TRequest).Name, request);
            }

            return result;
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "{CommandName}: failed to handle a {RequestType} with {@Request}", queryName, typeof(TRequest).Name, request);
            throw;
        }
    }

    protected abstract Task<TResponse> OnExecute(TRequest request);

    protected virtual void OnCommandException(TRequest request)
    {
        throw new QueryException<TRequest,TResponse>(this);
    }
}