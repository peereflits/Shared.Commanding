using System;

namespace Peereflits.Shared.Commanding;

public class CommandException : ApplicationException
{
    public CommandException(IAction command) : this($"Failed to execute command {command.CommandName}.") { }
    public CommandException(string message) : base(message) { }
    public CommandException(string message, Exception innerException) : base(message, innerException) { }
}