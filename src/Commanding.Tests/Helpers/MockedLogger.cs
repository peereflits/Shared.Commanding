using System;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

public abstract class MockedLogger<T> : ILogger<T>
{
    void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
            Log(logLevel, formatter(state, exception));

    public abstract void Log(LogLevel logLevel, string message);

    public virtual bool IsEnabled(LogLevel logLevel) => true;

    IDisposable ILogger.BeginScope<TState>(TState state) => BeginScope(state);

    public abstract IDisposable BeginScope<TState>(TState state);
}
