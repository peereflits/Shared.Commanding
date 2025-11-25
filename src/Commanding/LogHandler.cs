using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding;

#pragma warning disable CS8795 // Partial member must have an implementation part because it has accessibility modifiers.

public static partial class LogHandler
{
    // ----------------------------------------------------
    // LoggedCommandHandler
    // ----------------------------------------------------
    private const int CmdHandler01Id = -1052453872; // "Handling a {CommandName}".GetHashCode()
    private const int CmdHandler02Id = 26502146;    // "Handled a {CommandName}".GetHashCode()
    private const int CmdHandler03Id = -852123818;  // "Cannot handle a {CommandName}".GetHashCode()
    private const int CmdHandler04Id = -1611729963; // "Failed to handle a {CommandName}".GetHashCode()

    [LoggerMessage(EventId = CmdHandler01Id, Level = LogLevel.Information, Message = "Handling a {CommandName}")]
    public static partial void HandlingCommand(ILogger logger, string commandName);

    [LoggerMessage(EventId = CmdHandler02Id, Level = LogLevel.Information, Message = "Handled a {CommandName}")]
    public static partial void HandledCommand(ILogger logger, string commandName);

    [LoggerMessage(EventId = CmdHandler03Id, Level = LogLevel.Warning, Message = "Cannot handle a {CommandName}")]
    public static partial void CannotHandleCommand(ILogger logger, string commandName);

    [LoggerMessage(EventId = CmdHandler04Id, Level = LogLevel.Error, Message = "Failed to handle a {CommandName}")]
    public static partial void FailedToHandleCommand(ILogger logger, string commandName);


    // ----------------------------------------------------
    // LoggedCommandHandler<TRequest>
    // ----------------------------------------------------
    private const int CmdHandler11Id = -694605769; // "Handling a {CommandName} with {@Request}".GetHashCode()
    private const int CmdHandler12Id = 1527975633; // "Handled a {CommandName} with {@Request}".GetHashCode()
    private const int CmdHandler13Id = 1204198089; // "Cannot handle a {CommandName} with {@Request}".GetHashCode()
    private const int CmdHandler14Id = 1841231565; // "Failed to handle a {CommandName} with {@Request}".GetHashCode()

    [LoggerMessage(EventId = CmdHandler11Id, Level = LogLevel.Information, Message = "Handling a {CommandName} with {@Request}")]
    public static partial void HandlingCommand(ILogger logger, string commandName, object request);

    [LoggerMessage(EventId = CmdHandler12Id, Level = LogLevel.Information, Message = "Handled a {CommandName} with {@Request}")]
    public static partial void HandledCommand(ILogger logger, string commandName, object request);

    [LoggerMessage(EventId = CmdHandler13Id, Level = LogLevel.Warning, Message = "Cannot handle a {CommandName} with {@Request}")]
    public static partial void CannotHandleCommand(ILogger logger, string commandName, object request);

    [LoggerMessage(EventId = CmdHandler14Id, Level = LogLevel.Error, Message = "Failed to handle a {CommandName} with {@Request}")]
    public static partial void FailedToHandleCommand(ILogger logger, string commandName, object request);


    // ----------------------------------------------------
    // LoggedCommandService
    // ----------------------------------------------------
    private const int CmdService01Id = 1489615776;  // "Executing a {CommandName}".GetHashCode()
    private const int CmdService02Id = -67864941;   // "Executed a {CommandName}".GetHashCode()
    private const int CmdService03Id = -1666849133; // "Cannot execute a {CommandName}".GetHashCode()
    private const int CmdService04Id = 1865557750;  // "Failed to execute a {CommandName}".GetHashCode()

    [LoggerMessage(EventId = CmdService01Id, Level = LogLevel.Information, Message = "Executing a {CommandName}")]
    public static partial void ExecutingCommand(ILogger logger, string commandName);

    [LoggerMessage(EventId = CmdService02Id, Level = LogLevel.Information, Message = "Executed a {CommandName}")]
    public static partial void ExecutedCommand(ILogger logger, string commandName);

    [LoggerMessage(EventId = CmdService03Id, Level = LogLevel.Warning, Message = "Cannot execute a {CommandName}")]
    public static partial void CannotExecuteCommand(ILogger logger, string commandName);

    [LoggerMessage(EventId = CmdService04Id, Level = LogLevel.Error, Message = "Failed to execute a {CommandName}")]
    public static partial void FailedToExecuteCommand(ILogger logger, string commandName);


    // ----------------------------------------------------
    // LoggedCommandService<TParameters>
    // ----------------------------------------------------
    private const int CmdService11Id = -1647182086; // "Executing a {CommandName} with {@Parameters}".GetHashCode()
    private const int CmdService12Id = 878645943;   // "Executed a {CommandName} with {@Parameters}".GetHashCode()
    private const int CmdService13Id = 1176137617;  // "Cannot execute a {CommandName} with {@Parameters}".GetHashCode()
    private const int CmdService14Id = -721960863;  // "Failed to execute a {CommandName} with {@Parameters}".GetHashCode()

    [LoggerMessage(EventId = CmdService11Id, Level = LogLevel.Information, Message = "Executing a {CommandName} with {@Parameters}")]
    public static partial void ExecutingCommand(ILogger logger, string commandName, object parameters);

    [LoggerMessage(EventId = CmdService12Id, Level = LogLevel.Information, Message = "Executed a {CommandName} with {@Parameters}")]
    public static partial void ExecutedCommand(ILogger logger, string commandName, object parameters);

    [LoggerMessage(EventId = CmdService13Id, Level = LogLevel.Warning, Message = "Cannot execute a {CommandName} with {@Parameters}")]
    public static partial void CannotExecuteCommand(ILogger logger, string commandName, object parameters);

    [LoggerMessage(EventId = CmdService14Id, Level = LogLevel.Error, Message = "Failed to execute a {CommandName} with {@Parameters}")]
    public static partial void FailedToExecuteCommand(ILogger logger, string commandName, object parameters);


    // ----------------------------------------------------
    // LoggedQueryHandler<TResponse>
    // ----------------------------------------------------
    private const int QryHandler01Id = -1312408377; // "Handling a {QueryName}".GetHashCode()
    private const int QryHandler02Id = -1769740389; // "Handled a {QueryName}".GetHashCode()
    private const int QryHandler03Id = 1508363972;  // "Cannot handle a {QueryName}".GetHashCode()
    private const int QryHandler04Id = -2031604466; // "Failed to handle a {QueryName}".GetHashCode()

    [LoggerMessage(EventId = QryHandler01Id, Level = LogLevel.Information, Message = "Handling a {QueryName}")]
    public static partial void HandlingQuery(ILogger logger, string queryName);

    [LoggerMessage(EventId = QryHandler02Id, Level = LogLevel.Information, Message = "Handled a {QueryName}")]
    public static partial void HandledQuery(ILogger logger, string queryName);

    [LoggerMessage(EventId = QryHandler03Id, Level = LogLevel.Warning, Message = "Cannot handle a {QueryName}")]
    public static partial void CannotHandleQuery(ILogger logger, string queryName);

    [LoggerMessage(EventId = QryHandler04Id, Level = LogLevel.Error, Message = "Failed to handle a {QueryName}")]
    public static partial void FailedToHandleQuery(ILogger logger, string queryName);


    // ----------------------------------------------------
    // LoggedQueryHandler<TRequest, TResponse>
    // ----------------------------------------------------
    private const int QryHandler11Id = -1483675383; // "Handling a {QueryName} with {@Request}".GetHashCode()
    private const int QryHandler12Id = 2081299211;  // "Handled a {QueryName} with {@Request}".GetHashCode()
    private const int QryHandler13Id = 953864082;   // "Cannot handle a {QueryName} with {@Request}".GetHashCode()
    private const int QryHandler14Id = 676216179;   // "Failed to handle a {QueryName} with {@Request}".GetHashCode()

    [LoggerMessage(EventId = QryHandler11Id, Level = LogLevel.Information, Message = "Handling a {QueryName} with {@Request}")]
    public static partial void HandlingQuery(ILogger logger, string queryName, object request);

    [LoggerMessage(EventId = QryHandler12Id, Level = LogLevel.Information, Message = "Handled a {QueryName} with {@Request}")]
    public static partial void HandledQuery(ILogger logger, string queryName, object request);

    [LoggerMessage(EventId = QryHandler13Id, Level = LogLevel.Warning, Message = "Cannot handle a {QueryName} with {@Request}")]
    public static partial void CannotHandleQuery(ILogger logger, string queryName, object request);

    [LoggerMessage(EventId = QryHandler14Id, Level = LogLevel.Error, Message = "Failed to handle a {QueryName} with {@Request}")]
    public static partial void FailedToHandleQuery(ILogger logger, string queryName, object request);


    // ----------------------------------------------------
    // LoggedQueryService<TResult>
    // ----------------------------------------------------
    private const int QryService01Id = -336876455;  // "Executing a {QueryName}".GetHashCode()
    private const int QryService02Id = -2076214865; // "Executed a {QueryName}".GetHashCode()
    private const int QryService03Id = -177478635;  // "Cannot execute a {QueryName}".GetHashCode()
    private const int QryService04Id = 2030904752;  // "Failed to execute a {QueryName}".GetHashCode()

    [LoggerMessage(EventId = QryService01Id, Level = LogLevel.Information, Message = "Executing a {QueryName}")]
    public static partial void ExecutingQuery(ILogger logger, string queryName);

    [LoggerMessage(EventId = QryService02Id, Level = LogLevel.Information, Message = "Executed a {QueryName}")]
    public static partial void ExecutedQuery(ILogger logger, string queryName);

    [LoggerMessage(EventId = QryService03Id, Level = LogLevel.Warning, Message = "Cannot execute a {QueryName}")]
    public static partial void CannotExecuteQuery(ILogger logger, string queryName);

    [LoggerMessage(EventId = QryService04Id, Level = LogLevel.Error, Message = "Failed to execute a {QueryName}")]
    public static partial void FailedToExecuteQuery(ILogger logger, string queryName);


    // ----------------------------------------------------
    // LoggedQueryService<TRequest, TResponse>
    // ----------------------------------------------------
    private const int QryService11Id = 504745592;   // "Executing a {QueryName} with {@Request}".GetHashCode()
    private const int QryService12Id = 845758664;   // "Executed a {QueryName} with {@Request}".GetHashCode()
    private const int QryService13Id = 979968330;   // "Cannot execute a {QueryName} with {@Request}".GetHashCode()
    private const int QryService14Id = -1858691295; // "Failed to execute a {QueryName} with {@Request}".GetHashCode()

    [LoggerMessage(EventId = QryService11Id, Level = LogLevel.Information, Message = "Executing a {QueryName} with {@Request}")]
    public static partial void ExecutingQuery(ILogger logger, string queryName, object request);

    [LoggerMessage(EventId = QryService12Id, Level = LogLevel.Information, Message = "Executed a {QueryName} with {@Request}")]
    public static partial void ExecutedQuery(ILogger logger, string queryName, object request);

    [LoggerMessage(EventId = QryService13Id, Level = LogLevel.Warning, Message = "Cannot execute a {QueryName} with {@Request}")]
    public static partial void CannotExecuteQuery(ILogger logger, string queryName, object request);

    [LoggerMessage(EventId = QryService14Id, Level = LogLevel.Error, Message = "Failed to execute a {QueryName} with {@Request}")]
    public static partial void FailedToExecuteQuery(ILogger logger, string queryName, object request);
}
