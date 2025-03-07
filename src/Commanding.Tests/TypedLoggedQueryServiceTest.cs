﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Peereflits.Shared.Commanding.Tests.Helpers;
using Xunit;

namespace Peereflits.Shared.Commanding.Tests;

public class TypedLoggedQueryServiceTest
{
    private readonly ITestService testService;
    private readonly MockedLogger<TypedTestQueryService> logger;

    private readonly TypedTestQueryService subject;

    public TypedLoggedQueryServiceTest()
    {
        testService = Substitute.For<ITestService>();
        testService.CanExecute()
                   .Returns(Task.FromResult(true));

        logger = Substitute.For<MockedLogger<TypedTestQueryService>>();
        logger.IsEnabled(Arg.Any<LogLevel>())
              .Returns(true, true, true);

        subject = new TypedTestQueryService(testService, logger);
    }

    [Fact]
    public async Task WhenRequestIsInvalid_ItShouldReturnFalse()
    {
        var invalidRequest = new TestRequest { Id = 0 };

        bool result = await subject.CanExecute(invalidRequest);

        Assert.False(result);
    }

    [Fact]
    public async Task WhenCanNotExecuteRequest_ItShouldThrow()
    {
        var invalidRequest = new TestRequest { Id = 0 };

        await Assert.ThrowsAsync<QueryException<TestRequest, bool>>(() => subject.Execute(invalidRequest));
    }

    [Fact]
    public async Task WhenRequestIsValid_ItShouldReturnTrue()
    {
        var validRequest = new TestRequest { Id = 1 };

        bool result = await subject.CanExecute(validRequest);

        Assert.True(result);
    }

    [Fact]
    public async Task WhenCanExecuteRequest_ItShouldNotThrow()
    {
        var validRequest = new TestRequest { Id = 1 };

        Exception? result = await Record.ExceptionAsync(() => subject.Execute(validRequest));

        Assert.Null(result);
    }
        
    [Fact]
    public async Task WhenExecute_ItShouldLog()
    {
        testService.Execute().Returns(Task.CompletedTask);

        await  subject.Execute(new TestRequest { Id = 1 });

        logger.Received().Log(LogLevel.Information, Arg.Is<string>(x=> x.Contains("Executing a TypedTestQueryService with")));
        logger.Received().Log(LogLevel.Information, Arg.Is<string>(x=> x.Contains("Executed a TypedTestQueryService with")));
    }

    [Fact]
    public async Task WhenCannotExecute_ItShouldLog()
    {
        testService.CanExecute().Returns(Task.FromResult(false));

        await Assert.ThrowsAsync<QueryException<TestRequest, bool>>(() => subject.Execute(new TestRequest { Id = 1 }));

        logger.Received().Log(LogLevel.Warning, Arg.Is<string>(x=> x.Contains("Cannot execute a TypedTestQueryService with")));
    }

    [Fact]
    public async Task WhenExecuteFails_ItShouldLogError()
    {
        testService.ExecuteWithResult<bool>().Throws(new AggregateException());

        await Assert.ThrowsAsync<AggregateException>(() => subject.Execute(new TestRequest { Id = 1 }));

        logger.Received().Log(LogLevel.Error, Arg.Is<string>(x=> x.Contains("Failed to execute a TypedTestQueryService with")));
    }
}