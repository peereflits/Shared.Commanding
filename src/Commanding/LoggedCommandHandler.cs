using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding;

public abstract class LoggedCommandHandler : ICommand
{
    private readonly string commandName;

    protected LoggedCommandHandler(ILogger<LoggedCommandHandler> logger)
    {
        Logger      = logger;
        commandName = GetType().Name;
    }

    protected ILogger<LoggedCommandHandler> Logger { get; }

    public virtual ValueTask<bool> CanExecute() => new(result: true);

    public async Task Execute()
    {
        LogHandler.HandlingCommand(logger: Logger, commandName: commandName);

        if(!await CanExecute())
        {
            LogHandler.CannotHandleCommand(logger: Logger, commandName: commandName);
            throw new CommandException(command: this);
        }

        try
        {
            await OnExecute();
            LogHandler.HandledCommand(logger: Logger, commandName: commandName);
        }
        catch(Exception)
        {
            LogHandler.FailedToHandleCommand(logger: Logger, commandName: commandName);
            throw;
        }
    }

    protected abstract Task OnExecute();
}

public abstract class LoggedCommandHandler<TRequest> : ICommand<TRequest> where TRequest : IRequest
{
    private readonly string commandName;

    protected LoggedCommandHandler(ILogger<LoggedCommandHandler<TRequest>> logger)
    {
        Logger      = logger;
        commandName = GetType().Name;
    }

    protected ILogger<LoggedCommandHandler<TRequest>> Logger { get; }

    public virtual ValueTask<bool> CanExecute(TRequest request) => new(result: true);

    public async Task Execute(TRequest request)
    {
        LogHandler.HandlingCommand(logger: Logger, commandName: commandName, request: request);

        if(!await CanExecute(request: request))
        {
            LogHandler.CannotHandleCommand(logger: Logger, commandName: commandName, request: request);
            OnCommandException(request: request);
        }

        try
        {
            await OnExecute(request: request);
            LogHandler.HandledCommand(logger: Logger, commandName: commandName, request: request);
        }
        catch(Exception)
        {
            LogHandler.FailedToHandleCommand(logger: Logger, commandName: commandName, request: request);
            throw;
        }
    }

    protected abstract Task OnExecute(TRequest request);

    protected virtual void OnCommandException(TRequest request)
    {
        throw new CommandException<TRequest>(command: this);
    }
}
