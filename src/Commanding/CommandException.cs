using System;

namespace Peereflits.Shared.Commanding;

public class CommandingException : ApplicationException
{
    public CommandingException(string message) : base(message) { }
    public CommandingException(string message, Exception innerException) : base(message, innerException) { }

}

public class CommandException(ICommand command) 
        : CommandingException($"Failed to execute command {command.GetType().Name}.");
public class CommandException<TRequest>(ICommand<TRequest> command) 
        : CommandingException($"Failed to execute command {command.GetType().Name}.") where TRequest : IRequest;
public class QueryException<TResponse>(IQuery<TResponse> query) 
        : CommandingException($"Failed to execute query {query.GetType().Name}.");
public class QueryException<TRequest, TResponse>(IQuery<TRequest, TResponse> query) 
        : CommandingException($"Failed to execute query {query.GetType().Name}.")
        where TRequest : IRequest;