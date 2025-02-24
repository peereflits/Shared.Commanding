using System.Threading.Tasks;

namespace Peereflits.Shared.Commanding;

public interface IQuery<TResponse> 
{
    Task<bool> CanExecute();
    Task<TResponse> Execute();
}

public abstract class Query<TResponse> : IQuery<TResponse>
{
    public virtual Task<bool> CanExecute() => Task.FromResult(true);

    public async Task<TResponse> Execute()
    {
        if(!await CanExecute())
        {
            OnCommandException();
        }

        return await OnExecute();
    }

    protected abstract Task<TResponse> OnExecute();

    protected virtual void OnCommandException()
    {
        throw new QueryException<TResponse>(this);
    }
}

public interface IQuery<in TRequest, TResponse> where TRequest : IRequest
{
    Task<bool> CanExecute(TRequest request);
    Task<TResponse> Execute(TRequest parameters);
}

public abstract class Query<TRequest, TResponse> : IQuery<TRequest, TResponse> where TRequest : IRequest
{
    public abstract string CommandName { get; }

    public virtual Task<bool> CanExecute(TRequest parameters) => Task.FromResult(true);

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
        throw new QueryException<TRequest, TResponse>(this);
    }
}