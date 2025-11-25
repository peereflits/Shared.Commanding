using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding;

public abstract class LoggedQueryService<TResult> : IQuery<TResult>
{
    private readonly string queryName;

    protected LoggedQueryService(ILogger<LoggedQueryService<TResult>> logger)
    {
        Logger    = logger;
        queryName = GetType().Name;
    }

    protected ILogger<LoggedQueryService<TResult>> Logger { get; }

    public virtual ValueTask<bool> CanExecute() => new(result: true);

    public async Task<TResult> Execute()
    {
        LogHandler.ExecutingQuery(logger: Logger, queryName: queryName);

        if(!await CanExecute())
        {
            LogHandler.CannotExecuteQuery(logger: Logger, queryName: queryName);
            OnCommandException();
        }

        try
        {
            TResult result = await OnExecute();
            LogHandler.ExecutedQuery(logger: Logger, queryName: queryName);

            return result;
        }
        catch(Exception)
        {
            LogHandler.FailedToExecuteQuery(logger: Logger, queryName: queryName);
            throw;
        }
    }

    protected abstract Task<TResult> OnExecute();

    protected virtual void OnCommandException()
    {
        throw new QueryException<TResult>(query: this);
    }
}

public abstract class LoggedQueryService<TParameters, TResult>
        : IQuery<TParameters, TResult> where TParameters : IRequest
{
    private readonly string queryName;

    protected LoggedQueryService(ILogger<LoggedQueryService<TParameters, TResult>> logger)
    {
        Logger    = logger;
        queryName = GetType().Name;
    }

    protected ILogger<LoggedQueryService<TParameters, TResult>> Logger { get; }

    public virtual ValueTask<bool> CanExecute(TParameters parameters) => new(result: true);

    public async Task<TResult> Execute(TParameters parameters)
    {
        LogHandler.ExecutingQuery(logger: Logger, queryName: queryName, request: parameters);

        if(!await CanExecute(parameters: parameters))
        {
            LogHandler.CannotExecuteQuery(logger: Logger, queryName: queryName, request: parameters);
            OnCommandException(parameters: parameters);
        }

        try
        {
            TResult result = await OnExecute(parameters: parameters);
            LogHandler.ExecutedQuery(logger: Logger, queryName: queryName, request: parameters);

            return result;
        }
        catch(Exception)
        {
            LogHandler.FailedToExecuteQuery(logger: Logger, queryName: queryName, request: parameters);
            throw;
        }
    }

    protected abstract Task<TResult> OnExecute(TParameters parameters);

    protected virtual void OnCommandException(TParameters parameters)
    {
        throw new QueryException<TParameters, TResult>(query: this);
    }
}
