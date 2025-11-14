using System;
using System.Threading.Tasks;
using Peereflits.Shared.Commanding.Tests.Helpers;
using Xunit;

namespace Peereflits.Shared.Commanding.Tests;

public class QueryTest
{
    private readonly Query<TestRequest, bool> subject = new ParameterizedTestQuery();

    [Fact]
    public async Task WhenExecutingWithoutRequest_ItShouldReturnExpectedResult()
    {
        Query<bool> subject1 = new ParameterlessTestQuery();

        bool result = await subject1.Execute();

        Assert.True(result);
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

        Task Act() => subject.Execute(invalidRequest);

        await Assert.ThrowsAsync<QueryException<TestRequest, bool>>(Act);
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
}