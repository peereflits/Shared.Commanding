using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Peereflits.Shared.Commanding.Tests.Helpers;
using Xunit;

namespace Peereflits.Shared.Commanding.Tests;

public sealed class TypedLoggedCommandHandlerTest
{
    private readonly MockedLogger<TypedTestCommandHandler> logger;

    private readonly TypedTestCommandHandler subject;
    private readonly ITestService testService;

    public TypedLoggedCommandHandlerTest()
    {
        testService = Substitute.For<ITestService>();
        testService
               .CanExecute()
               .Returns(returnThis: true);

        logger = Substitute.For<MockedLogger<TypedTestCommandHandler>>();
        logger
               .IsEnabled(logLevel: Arg.Any<LogLevel>())
               .Returns(returnThis: true, true, true);

        subject = new TypedTestCommandHandler(testService: testService, logger: logger);
    }

    [Fact]
    public async Task WhenRequestIsInvalid_ItShouldReturnFalse()
    {
        var invalidRequest = new TestRequest { Id = 0 };

        bool result = await subject.CanExecute(request: invalidRequest);

        Assert.False(condition: result);
    }

    [Fact]
    public async Task WhenCanNotExecuteRequest_ItShouldThrow()
    {
        var invalidRequest = new TestRequest { Id = 0 };

        await Assert.ThrowsAsync<CommandException<TestRequest>>(testCode: () => subject.Execute(request: invalidRequest));
    }

    [Fact]
    public async Task WhenRequestIsValid_ItShouldReturnTrue()
    {
        var validRequest = new TestRequest { Id = 1 };

        bool result = await subject.CanExecute(request: validRequest);

        Assert.True(condition: result);
    }

    [Fact]
    public async Task WhenCanExecuteRequest_ItShouldNotThrow()
    {
        var validRequest = new TestRequest { Id = 1 };

        Exception? result = await Record.ExceptionAsync(testCode: () => subject.Execute(request: validRequest));

        Assert.Null(@object: result);
    }

    [Fact]
    public async Task WhenExecute_ItShouldLog()
    {
        testService
               .Execute()
               .Returns(returnThis: Task.CompletedTask);

        await subject.Execute(request: new TestRequest { Id = 1 });

        logger
               .Received()
               .Log(
                    logLevel: LogLevel.Information
                  , message: "Handling a TypedTestCommandHandler with TestRequest { Id = 1 }"
                   );

        logger
               .Received()
               .Log(
                    logLevel: LogLevel.Information
                  , message: "Handled a TypedTestCommandHandler with TestRequest { Id = 1 }"
                   );
    }

    [Fact]
    public async Task WhenCannotExecute_ItShouldLog()
    {
        testService
               .CanExecute()
               .Returns(returnThis: false);

        await Assert.ThrowsAsync<CommandException<TestRequest>>(testCode: () => subject.Execute(request: new TestRequest { Id = 1 }));

        logger
               .Received()
               .Log(
                    logLevel: LogLevel.Warning
                  , message: "Cannot handle a TypedTestCommandHandler with TestRequest { Id = 1 }"
                   );
    }

    [Fact]
    public async Task WhenExecuteFails_ItShouldLogError()
    {
        testService
               .Execute()
               .Throws(ex: new AggregateException());

        await Assert.ThrowsAsync<AggregateException>(testCode: () => subject.Execute(request: new TestRequest { Id = 1 }));

        logger
               .Received()
               .Log(
                    logLevel: LogLevel.Error
                  , message: "Failed to handle a TypedTestCommandHandler with TestRequest { Id = 1 }"
                   );
    }
}
