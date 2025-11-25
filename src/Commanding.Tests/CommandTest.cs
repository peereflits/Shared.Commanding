using System;
using System.Threading.Tasks;
using Peereflits.Shared.Commanding.Tests.Helpers;
using Xunit;

namespace Peereflits.Shared.Commanding.Tests;

public sealed class CommandTest
{
    private readonly TestCommand subject = new();

    [Fact]
    public async Task WhenRequestIsInvalid_ItShouldReturnFalse()
    {
        var invalidRequest = new TestRequest { Id = 0 };

        bool result = await subject.CanExecute(parameters: invalidRequest);

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

        bool result = await subject.CanExecute(parameters: validRequest);

        Assert.True(condition: result);
    }

    [Fact]
    public async Task WhenCanExecuteRequest_ItShouldNotThrow()
    {
        var validRequest = new TestRequest { Id = 1 };

        Exception? result = await Record.ExceptionAsync(testCode: () => subject.Execute(parameters: validRequest));

        Assert.Null(@object: result);
    }
}
