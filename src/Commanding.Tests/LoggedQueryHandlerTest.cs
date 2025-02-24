using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Peereflits.Shared.Commanding.Tests.Helpers;
using Xunit;

namespace Peereflits.Shared.Commanding.Tests;

public class LoggedQueryHandlerTest
{
    private readonly ITestService testService;
    private readonly MockedLogger<TestLoggedQueryHandler> logger;

    private readonly TestLoggedQueryHandler subject;

    public LoggedQueryHandlerTest()
    {
        testService = Substitute.For<ITestService>();
        testService.CanExecute()
                   .Returns(Task.FromResult(true));
        testService.ExecuteWithResult<bool>()
                   .Returns(Task.FromResult(true));

        logger = Substitute.For<MockedLogger<TestLoggedQueryHandler>>();
        logger.IsEnabled(Arg.Any<LogLevel>())
              .Returns(true, true, true);

        subject = new TestLoggedQueryHandler(testService, logger);
    }

    [Fact]
    public async Task WhenExecute_ItShouldReturnExpectedResult()
    {
        bool result = await subject.Execute();
        Assert.True(result);
    }

    [Fact]
    public async Task WhenCanNotExecute_ItShouldThrow()
    {
        testService.CanExecute()
                   .Returns(Task.FromResult(false));

        _ = await Assert.ThrowsAsync<QueryException<bool>>(() => subject.Execute());
    }

    [Fact]
    public async Task WhenExecute_ItShouldLog()
    {
        _ = await subject.Execute();

        logger.Received().Log(LogLevel.Information, Arg.Is<string>(x=> x == "Handling a TestLoggedQueryHandler"));
        logger.Received().Log(LogLevel.Information, Arg.Is<string>(x=> x == "Handled a TestLoggedQueryHandler"));
    }

    [Fact]
    public async Task WhenCannotExecute_ItShouldLog()
    {
        testService.CanExecute()
                   .Returns(Task.FromResult(false));

        _ = await Assert.ThrowsAsync<QueryException<bool>>(() => subject.Execute());

        logger.Received().Log(LogLevel.Warning, Arg.Is<string>(x => x == "Cannot handle a TestLoggedQueryHandler"));
    }

    [Fact]
    public async Task WhenExecuteFails_ItShouldLogError()
    {
        testService.ExecuteWithResult<bool>().Throws(new AggregateException());

        _ = await Assert.ThrowsAsync<AggregateException>(() => subject.Execute());

        logger.Received().Log(LogLevel.Error, Arg.Is<string>(x => x.Contains("Failed to handle a TestLoggedQueryHandler")));
    }
}