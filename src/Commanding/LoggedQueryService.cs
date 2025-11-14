using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding;

public abstract class LoggedQueryService<TResult> : IQuery<TResult>
{
    private readonly string queryName;

    protected LoggedQueryService(ILogger<LoggedQueryService<TResult>> logger)
    {
        Logger = logger;
        queryName = GetType().Name;
    }

    protected ILogger<LoggedQueryService<TResult>> Logger { get; }

    public virtual ValueTask<bool> CanExecute() => new(true);

    public async Task<TResult> Execute()
    {
        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation("Executing a {CommandName}", queryName);
        }

        if(!await CanExecute())
        {
            Logger.LogWarning("Cannot execute a {CommandName}", queryName);
            OnCommandException();
        }

        try
        {
            TResult result = await OnExecute();

            if(Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation("Executed a {CommandName}", queryName);
            }

            return result;
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Failed to execute a {CommandName}", queryName);
            throw;
        }
    }

    protected abstract Task<TResult> OnExecute();

    protected virtual void OnCommandException()
    {
        throw new QueryException<TResult>(this);
    }
}

public abstract class LoggedQueryService<TParameters, TResult> 
        : IQuery<TParameters, TResult> where TParameters : IRequest
{
    private readonly string queryName;

    protected LoggedQueryService(ILogger<LoggedQueryService<TParameters, TResult>> logger)
    {
        Logger = logger;
        queryName = GetType().Name;
    }

    protected ILogger<LoggedQueryService<TParameters, TResult>> Logger { get; }

    public virtual ValueTask<bool> CanExecute(TParameters parameters) => new(true);

    public async Task<TResult> Execute(TParameters parameters)
    {
        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation("Executing a {CommandName} with {@Parameters}", queryName, parameters);
        }

        if(!await CanExecute(parameters))
        {
            Logger.LogWarning("Cannot execute a {CommandName} with {@Parameters}", queryName, parameters);
            OnCommandException(parameters);
        }

        try
        {
            TResult result = await OnExecute(parameters);

            if(Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation("Executed a {CommandName} with {@Parameters}", queryName, parameters);
            }

            return result;
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Failed to execute a {CommandName} with {@Parameters}", queryName, parameters);
            throw;
        }
    }

    protected abstract Task<TResult> OnExecute(TParameters parameters);

    protected virtual void OnCommandException(TParameters parameters)
    {
        throw new QueryException<TParameters, TResult>(this);
    }
}