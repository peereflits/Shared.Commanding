using System.Threading.Tasks;

namespace Peereflits.Shared.Commanding;


public interface IQuery<TResponse> : IExecutable
{
    Task<TResponse> Execute();
}

public interface IQuery<in TRequest, TResponse> : IExecutable<TRequest> where TRequest : IRequest
{
    Task<TResponse> Execute(TRequest parameters);
}

public abstract class Query<TResponse> : IQuery<TResponse>
{
    public abstract string CommandName { get; }

    public virtual Task<bool> CanExecute() => Task.FromResult(true);

    public abstract Task<TResponse> Execute();
}

public abstract class Query<TRequest, TResponse> : IQuery<TRequest, TResponse> where TRequest : IRequest
{
    public abstract string CommandName { get; }

    public abstract Task<bool> CanExecute(TRequest parameters);

    public async Task<TResponse> Execute(TRequest parameters)
    {
        if(!await CanExecute(parameters))
        {
            OnCommandException(parameters);
        }

        return await OnExecute(parameters);
    }

    protected abstract Task<TResponse> OnExecute(TRequest parameters);

    protected virtual void OnCommandException(TRequest parameters)
    {
        throw new CommandException(this);
    }
}