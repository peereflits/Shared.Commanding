using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Peereflits.Shared.Commanding.Tests.Helpers;
using Xunit;

namespace Peereflits.Shared.Commanding.Tests;

public sealed class TypedLoggedQueryServiceTest
{
    private readonly MockedLogger<TypedTestQueryService> logger;

    private readonly TypedTestQueryService subject;
    private readonly ITestService testService;

    public TypedLoggedQueryServiceTest()
    {
        testService = Substitute.For<ITestService>();
        testService
               .CanExecute()
               .Returns(returnThis: true);

        logger = Substitute.For<MockedLogger<TypedTestQueryService>>();
        logger
               .IsEnabled(logLevel: Arg.Any<LogLevel>())
               .Returns(returnThis: true, true, true);

        subject = new TypedTestQueryService(testService: testService, logger: logger);
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

        await Assert.ThrowsAsync<QueryException<TestRequest, bool>>(testCode: () => subject.Execute(parameters: invalidRequest));
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
               .Log(logLevel: LogLevel.Information, message: Arg.Is<string>(predicate: x => x.Contains("Executing a TypedTestQueryService with")));

        logger
               .Received()
               .Log(logLevel: LogLevel.Information, message: Arg.Is<string>(predicate: x => x.Contains("Executed a TypedTestQueryService with")));
    }

    [Fact]
    public async Task WhenCannotExecute_ItShouldLog()
    {
        testService
               .CanExecute()
               .Returns(returnThis: false);

        await Assert.ThrowsAsync<QueryException<TestRequest, bool>>(testCode: () => subject.Execute(parameters: new TestRequest { Id = 1 }));

        logger
               .Received()
               .Log(logLevel: LogLevel.Warning, message: Arg.Is<string>(predicate: x => x.Contains("Cannot execute a TypedTestQueryService with")));
    }

    [Fact]
    public async Task WhenExecuteFails_ItShouldLogError()
    {
        testService
               .ExecuteWithResult<bool>()
               .Throws(ex: new AggregateException());

        await Assert.ThrowsAsync<AggregateException>(testCode: () => subject.Execute(parameters: new TestRequest { Id = 1 }));

        logger
               .Received()
               .Log(logLevel: LogLevel.Error, message: Arg.Is<string>(predicate: x => x.Contains("Failed to execute a TypedTestQueryService with")));
    }
}
