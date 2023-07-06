using System.Threading.Tasks;

namespace Peereflits.Shared.Commanding;

public interface ICommand : IExecutable
{
    Task Execute();
}

public interface ICommand<in TRequest> : IExecutable<TRequest> where TRequest : IRequest
{
    Task Execute(TRequest parameters);
}

public abstract class Command : ICommand
{
    public abstract string CommandName { get; }

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

public abstract class Command<TRequest> : ICommand<TRequest> where TRequest : IRequest
{
    public abstract string CommandName { get; }

    public abstract Task<bool> CanExecute(TRequest parameters);

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
        throw new CommandException(this);
    }
}