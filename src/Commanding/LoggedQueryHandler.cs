using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding;

public abstract class LoggedQueryHandler<TResponse> : IQuery<TResponse>
{
    private readonly string queryName;

    protected LoggedQueryHandler(ILogger<LoggedQueryHandler<TResponse>> logger)
    {
        Logger    = logger;
        queryName = GetType().Name;
    }

    protected ILogger<LoggedQueryHandler<TResponse>> Logger { get; }

    public virtual ValueTask<bool> CanExecute() => new(result: true);

    public async Task<TResponse> Execute()
    {
        LogHandler.HandlingQuery(logger: Logger, queryName: queryName);

        if(!await CanExecute())
        {
            LogHandler.CannotHandleQuery(logger: Logger, queryName: queryName);
            OnCommandException();
        }

        try
        {
            TResponse result = await OnExecute();
            LogHandler.HandledQuery(logger: Logger, queryName: queryName);

            return result;
        }
        catch(Exception)
        {
            LogHandler.FailedToHandleQuery(logger: Logger, queryName: queryName);
            throw;
        }
    }

    protected abstract Task<TResponse> OnExecute();

    protected virtual void OnCommandException()
    {
        throw new QueryException<TResponse>(query: this);
    }
}

public abstract class LoggedQueryHandler<TRequest, TResponse> : IQuery<TRequest, TResponse> where TRequest : IRequest
{
    private readonly string queryName;

    protected LoggedQueryHandler(ILogger<LoggedQueryHandler<TRequest, TResponse>> logger)
    {
        Logger    = logger;
        queryName = GetType().Name;
    }

    protected ILogger<LoggedQueryHandler<TRequest, TResponse>> Logger { get; }

    public virtual ValueTask<bool> CanExecute(TRequest request) => new(result: true);

    public async Task<TResponse> Execute(TRequest request)
    {
        LogHandler.HandlingQuery(logger: Logger, queryName: queryName, request: request);

        if(!await CanExecute(request: request))
        {
            LogHandler.CannotHandleQuery(logger: Logger, queryName: queryName, request: request);
            OnCommandException(request: request);
        }

        try
        {
            TResponse result = await OnExecute(request: request);
            LogHandler.HandledQuery(logger: Logger, queryName: queryName, request: request);

            return result;
        }
        catch(Exception)
        {
            LogHandler.FailedToHandleQuery(logger: Logger, queryName: queryName, request: request);
            throw;
        }
    }

    protected abstract Task<TResponse> OnExecute(TRequest request);

    protected virtual void OnCommandException(TRequest request)
    {
        throw new QueryException<TRequest, TResponse>(query: this);
    }
}
