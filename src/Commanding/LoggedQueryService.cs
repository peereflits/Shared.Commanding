using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding;

public abstract class LoggedQueryService<TResult> : IQuery<TResult>
{
    protected LoggedQueryService(ILogger<LoggedQueryService<TResult>> logger) => Logger = logger;

    protected ILogger<LoggedQueryService<TResult>> Logger { get; }

    public abstract string CommandName { get; }

    public virtual Task<bool> CanExecute() => Task.FromResult(true);

    public async Task<TResult> Execute()
    {
        Logger.LogInformation("Executing a {CommandName}", CommandName);

        if(!await CanExecute())
        {
            Logger.LogWarning("Cannot execute a {CommandName}", CommandName);
            OnCommandException();
        }

        try
        {
            TResult result = await OnExecute();

            Logger.LogInformation("Executed a {CommandName}", CommandName);
            return result;
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Failed to execute a {CommandName}", CommandName);
            throw;
        }
    }

    protected abstract Task<TResult> OnExecute();

    protected virtual void OnCommandException()
    {
        throw new CommandException(this);
    }
}

public abstract class LoggedQueryService<TParameters, TResult> : IQuery<TParameters, TResult> where TParameters : IRequest
{
    protected LoggedQueryService(ILogger<LoggedQueryService<TParameters, TResult>> logger) => Logger = logger;

    protected ILogger<LoggedQueryService<TParameters, TResult>> Logger { get; }

    public abstract string CommandName { get; }

    public abstract Task<bool> CanExecute(TParameters parameters);

    public async Task<TResult> Execute(TParameters parameters)
    {
        Logger.LogInformation("Executing a {CommandName} with {@Parameters}", CommandName, parameters);

        if(!await CanExecute(parameters))
        {
            Logger.LogWarning("Cannot execute a {CommandName} with {@Parameters}", CommandName, parameters);
            OnCommandException(parameters);
        }

        try
        {
            TResult result = await OnExecute(parameters);

            Logger.LogInformation("Executed a {CommandName} with {@Parameters}", CommandName, parameters);
            return result;
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Failed to execute a {CommandName} with {@Parameters}", CommandName, parameters);
            throw;
        }
    }

    protected abstract Task<TResult> OnExecute(TParameters parameters);

    protected virtual void OnCommandException(TParameters parameters)
    {
        throw new CommandException(this);
    }
}