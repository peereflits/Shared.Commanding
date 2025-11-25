using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding;

public abstract class LoggedCommandService : ICommand
{
    private readonly string commandName;

    protected LoggedCommandService(ILogger<LoggedCommandService> logger)
    {
        Logger      = logger;
        commandName = GetType().Name;
    }

    protected ILogger<LoggedCommandService> Logger { get; }

    public virtual ValueTask<bool> CanExecute() => new(result: true);

    public async Task Execute()
    {
        LogHandler.ExecutingCommand(logger: Logger, commandName: commandName);

        if(!await CanExecute())
        {
            LogHandler.CannotExecuteCommand(logger: Logger, commandName: commandName);
            throw new CommandException(command: this);
        }

        try
        {
            await OnExecute();
            LogHandler.ExecutedCommand(logger: Logger, commandName: commandName);
        }
        catch(Exception)
        {
            LogHandler.FailedToExecuteCommand(logger: Logger, commandName: commandName);
            throw;
        }
    }

    protected abstract Task OnExecute();
}

public abstract class LoggedCommandService<TParameters> : ICommand<TParameters> where TParameters : IRequest
{
    private readonly string commandName;

    protected LoggedCommandService(ILogger<LoggedCommandService<TParameters>> logger)
    {
        Logger      = logger;
        commandName = GetType().Name;
    }

    protected ILogger<LoggedCommandService<TParameters>> Logger { get; }

    public virtual ValueTask<bool> CanExecute(TParameters parameters) => new(result: true);

    public async Task Execute(TParameters parameters)
    {
        LogHandler.ExecutingCommand(logger: Logger, commandName: commandName, parameters: parameters);

        if(!await CanExecute(parameters: parameters))
        {
            LogHandler.CannotExecuteCommand(
                                            logger: Logger
                                          , commandName: commandName
                                          , parameters: parameters
                                           );

            OnCommandException(parameters: parameters);
        }

        try
        {
            await OnExecute(parameters: parameters);
            LogHandler.ExecutedCommand(logger: Logger, commandName: commandName, parameters: parameters);
        }
        catch(Exception)
        {
            LogHandler.FailedToExecuteCommand(
                                              logger: Logger
                                            , commandName: commandName
                                            , parameters: parameters
                                             );

            throw;
        }
    }

    protected abstract Task OnExecute(TParameters parameters);

    protected virtual void OnCommandException(TParameters parameters)
    {
        throw new CommandException<TParameters>(command: this);
    }
}
