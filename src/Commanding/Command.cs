using System.Threading.Tasks;

namespace Peereflits.Shared.Commanding;

public interface ICommand 
{
    ValueTask<bool> CanExecute();
    Task Execute();
}

public abstract class Command : ICommand
{
    public virtual ValueTask<bool> CanExecute() => new(true);

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
    ValueTask<bool> CanExecute(TRequest request);
    Task Execute(TRequest parameters);
}

public abstract class Command<TRequest> : ICommand<TRequest> where TRequest : IRequest
{
    public virtual ValueTask<bool> CanExecute(TRequest parameters) => new(true);

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