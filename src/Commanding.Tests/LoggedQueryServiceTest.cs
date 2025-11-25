using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Peereflits.Shared.Commanding.Tests.Helpers;
using Xunit;

namespace Peereflits.Shared.Commanding.Tests;

public sealed class LoggedQueryServiceTest
{
    private readonly MockedLogger<TestLoggedQueryService> logger;

    private readonly TestLoggedQueryService subject;
    private readonly ITestService testService;

    public LoggedQueryServiceTest()
    {
        testService = Substitute.For<ITestService>();
        testService
               .CanExecute()
               .Returns(returnThis: true);

        testService
               .ExecuteWithResult<bool>()
               .Returns(returnThis: true);

        logger = Substitute.For<MockedLogger<TestLoggedQueryService>>();
        logger
               .IsEnabled(logLevel: Arg.Any<LogLevel>())
               .Returns(returnThis: true, true, true);

        subject = new TestLoggedQueryService(testService: testService, logger: logger);
    }

    [Fact]
    public async Task WhenExecute_ItShouldReturnExpectedResult()
    {
        bool result = await subject.Execute();
        Assert.True(condition: result);
    }

    [Fact]
    public async Task WhenCanNotExecute_ItShouldThrow()
    {
        testService
               .CanExecute()
               .Returns(returnThis: false);

        _ = await Assert.ThrowsAsync<QueryException<bool>>(testCode: () => subject.Execute());
    }

    [Fact]
    public async Task WhenExecute_ItShouldLog()
    {
        _ = await subject.Execute();

        logger
               .Received()
               .Log(logLevel: LogLevel.Information, message: "Executing a TestLoggedQueryService");

        logger
               .Received()
               .Log(logLevel: LogLevel.Information, message: "Executed a TestLoggedQueryService");
    }

    [Fact]
    public async Task WhenCannotExecute_ItShouldLog()
    {
        testService
               .CanExecute()
               .Returns(returnThis: false);

        _ = await Assert.ThrowsAsync<QueryException<bool>>(testCode: () => subject.Execute());

        logger.Received().Log(logLevel: LogLevel.Warning, message: "Cannot execute a TestLoggedQueryService");
    }

    [Fact]
    public async Task WhenExecuteFails_ItShouldLogError()
    {
        testService
               .ExecuteWithResult<bool>()
               .Throws(ex: new AggregateException());

        _ = await Assert.ThrowsAsync<AggregateException>(testCode: () => subject.Execute());

        logger
               .Received()
               .Log(logLevel: LogLevel.Error, message: "Failed to execute a TestLoggedQueryService");
    }
}
