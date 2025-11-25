using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Peereflits.Shared.Commanding.Tests.Helpers;
using Xunit;

namespace Peereflits.Shared.Commanding.Tests;

public sealed class LoggedCommandHandlerTest
{
    private readonly MockedLogger<TestCommandHandler> logger;

    private readonly TestCommandHandler subject;
    private readonly ITestService testService;

    public LoggedCommandHandlerTest()
    {
        testService = Substitute.For<ITestService>();
        testService
               .CanExecute()
               .Returns(returnThis: true);

        logger = Substitute.For<MockedLogger<TestCommandHandler>>();
        logger
               .IsEnabled(logLevel: Arg.Any<LogLevel>())
               .Returns(returnThis: true);

        subject = new TestCommandHandler(testService: testService, logger: logger);
    }

    [Fact]
    public async Task WhenCanNotExecuteRequest_ItShouldThrow()
    {
        testService.CanExecute().Returns(returnThis: false);

        await Assert.ThrowsAsync<CommandException>(testCode: () => subject.Execute());
    }

    [Fact]
    public async Task WhenCanExecuteRequest_ItShouldNotThrow()
    {
        Exception? result = await Record.ExceptionAsync(testCode: () => subject.Execute());

        Assert.Null(@object: result);
    }

    [Fact]
    public async Task WhenExecute_ItShouldLog()
    {
        testService.Execute().Returns(returnThis: Task.CompletedTask);

        await subject.Execute();

        logger.Received().Log(logLevel: LogLevel.Information, message: "Handling a TestCommandHandler");
        logger.Received().Log(logLevel: LogLevel.Information, message: "Handled a TestCommandHandler");
    }

    [Fact]
    public async Task WhenCannotExecute_ItShouldLog()
    {
        testService.CanExecute().Returns(returnThis: false);

        await Assert.ThrowsAsync<CommandException>(testCode: () => subject.Execute());

        logger.Received().Log(logLevel: LogLevel.Warning, message: "Cannot handle a TestCommandHandler");
    }

    [Fact]
    public async Task WhenExecuteFails_ItShouldLogError()
    {
        testService.Execute().Throws(ex: new AggregateException());

        await Assert.ThrowsAsync<AggregateException>(testCode: () => subject.Execute());

        logger.Received().Log(logLevel: LogLevel.Error, message: "Failed to handle a TestCommandHandler");
    }
}
