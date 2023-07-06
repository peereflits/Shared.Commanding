using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Peereflits.Shared.Commanding.Tests.Helpers;
using Xunit;

namespace Peereflits.Shared.Commanding.Tests;

public class LoggedCommandServiceTest
{
    private readonly ITestService testService;
    private readonly MockedLogger<TestCommandService> logger;

    private readonly TestCommandService subject;

    public LoggedCommandServiceTest()
    {
        testService = Substitute.For<ITestService>();
        testService.CanExecute()
                   .Returns(Task.FromResult(true));

        logger = Substitute.For<MockedLogger<TestCommandService>>();

        subject = new TestCommandService(testService, logger);
    }

    [Fact]
    public async Task WhenCanNotExecuteRequest_ItShouldThrow()
    {
        testService.CanExecute().Returns(Task.FromResult(false));

        await Assert.ThrowsAsync<CommandException>(() => subject.Execute());
    }

    [Fact]
    public async Task WhenCanExecuteRequest_ItShouldNotThrow()
    {
        Exception result = await Record.ExceptionAsync(() => subject.Execute());

        Assert.Null(result);
    }
        
    [Fact]
    public async Task WhenExecute_ItShouldLog()
    {
        testService.Execute().Returns(Task.CompletedTask);

        await  subject.Execute();

        logger.Received().Log(LogLevel.Information, Arg.Is<string>(x=> x == "Executing a TestCommandService"));
        logger.Received().Log(LogLevel.Information, Arg.Is<string>(x=> x == "Executed a TestCommandService"));
    }

    [Fact]
    public async Task WhenCannotExecute_ItShouldLog()
    {
        testService.CanExecute().Returns(Task.FromResult(false));

        await Assert.ThrowsAsync<CommandException>(() => subject.Execute());

        logger.Received().Log(LogLevel.Warning, Arg.Is<string>(x=> x == "Cannot execute a TestCommandService"));
    }

    [Fact]
    public async Task WhenExecuteFails_ItShouldLogError()
    {
        testService.Execute().Throws(new AggregateException());

        await Assert.ThrowsAsync<AggregateException>(() => subject.Execute());

        logger.Received().Log(LogLevel.Error, Arg.Is<string>(x=> x.Contains("Failed to execute a TestCommandService")));
    }
}