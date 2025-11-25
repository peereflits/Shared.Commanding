using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Peereflits.Shared.Commanding.Tests.Helpers;
using Xunit;

namespace Peereflits.Shared.Commanding.Tests;

public sealed class TypedLoggedCommandServiceTest
{
    private readonly MockedLogger<TypedTestCommandService> logger;

    private readonly TypedTestCommandService subject;
    private readonly ITestService testService;

    public TypedLoggedCommandServiceTest()
    {
        testService = Substitute.For<ITestService>();
        testService
               .CanExecute()
               .Returns(returnThis: true);

        logger = Substitute.For<MockedLogger<TypedTestCommandService>>();
        logger
               .IsEnabled(logLevel: Arg.Any<LogLevel>())
               .Returns(returnThis: true, true, true);

        subject = new TypedTestCommandService(testService: testService, logger: logger);
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

        await Assert.ThrowsAsync<CommandException<TestRequest>>(testCode: () => subject.Execute(parameters: invalidRequest));
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

        Exception? result = await Record.ExceptionAsync(testCode: () => subject.Execute(parameters: validRequest));

        Assert.Null(@object: result);
    }

    [Fact]
    public async Task WhenExecute_ItShouldLog()
    {
        testService
               .Execute()
               .Returns(returnThis: Task.CompletedTask);

        await subject.Execute(parameters: new TestRequest { Id = 1 });

        logger
               .Received()
               .Log(logLevel: LogLevel.Information, message: "Executing a TypedTestCommandService with TestRequest { Id = 1 }");

        logger
               .Received()
               .Log(logLevel: LogLevel.Information, message: "Executed a TypedTestCommandService with TestRequest { Id = 1 }");
    }

    [Fact]
    public async Task WhenCannotExecute_ItShouldLog()
    {
        testService
               .CanExecute()
               .Returns(returnThis: false);

        await Assert.ThrowsAsync<CommandException<TestRequest>>(testCode: () => subject.Execute(parameters: new TestRequest { Id = 1 }));

        logger
               .Received()
               .Log(logLevel: LogLevel.Warning, message: "Cannot execute a TypedTestCommandService with TestRequest { Id = 1 }");
    }

    [Fact]
    public async Task WhenExecuteFails_ItShouldLogError()
    {
        testService
               .Execute()
               .Throws(ex: new AggregateException());

        await Assert.ThrowsAsync<AggregateException>(testCode: () => subject.Execute(parameters: new TestRequest { Id = 1 }));

        logger
               .Received()
               .Log(
                    logLevel: LogLevel.Error
                  , message: "Failed to execute a TypedTestCommandService with TestRequest { Id = 1 }"
                   );
    }
}
