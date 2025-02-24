using System.Threading.Tasks;

namespace Peereflits.Shared.Commanding;

public interface ICommand 
{
    Task<bool> CanExecute();
    Task Execute();
}

public abstract class Command : ICommand
{
    public virtual Task<bool> CanExecute() => Task.FromResult(true);

    public async Task Execute()
    {
        if(!await CanExecute())
        {
            throw new CommandException(this);
        }

        await OnExecute();
    }

    protected abstract Task OnExecute();
}

public interface ICommand<in TRequest> where TRequest : IRequest
{
    Task<bool> CanExecute(TRequest request);
    Task Execute(TRequest parameters);
}

public abstract class Command<TRequest> : ICommand<TRequest> where TRequest : IRequest
{
    public abstract string CommandName { get; }

    public virtual Task<bool> CanExecute(TRequest parameters) => Task.FromResult(true);

    public async Task Execute(TRequest parameters)
    {
        if (!await CanExecute(parameters))
        {
            OnCommandException(parameters);
        }

        await OnExecute(parameters);
    }

    protected abstract Task OnExecute(TRequest parameters);

    protected virtual void OnCommandException(TRequest parameters)
    {
        throw new CommandException<TRequest>(this);
    }
}