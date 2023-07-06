using System;
using System.Threading.Tasks;
using Peereflits.Shared.Commanding.Tests.Helpers;
using Xunit;

namespace Peereflits.Shared.Commanding.Tests;

public class CommandTest
{
    private readonly TestCommand subject;

    public CommandTest()
    {
        subject = new TestCommand();
    }

    [Fact]
    public async Task WhenRequestIsInvalid_ItShouldReturnFalse()
    {
        // Arrange
        var invalidRequest = new TestRequest { Id = 0 };

        bool result = await subject.CanExecute(invalidRequest);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task WhenCanNotExecuteRequest_ItShouldThrow()
    {
        // Arrange
        var invalidRequest = new TestRequest { Id = 0 };

        // Assert
        await Assert.ThrowsAsync<CommandException>(() => subject.Execute(invalidRequest));
    }

    [Fact]
    public async Task WhenRequestIsValid_ItShouldReturnTrue()
    {
        // Arrange
        var validRequest = new TestRequest { Id = 1 };

        bool result = await subject.CanExecute(validRequest);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task WhenCanExecuteRequest_ItShouldNotThrow()
    {
        // Arrange
        var validRequest = new TestRequest { Id = 1 };

        Exception result = await Record.ExceptionAsync(() => subject.Execute(validRequest));

        // Assert
        Assert.Null(result);
    }
}