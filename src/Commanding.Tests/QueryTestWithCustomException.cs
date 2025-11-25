using System;
using System.Threading.Tasks;
using Peereflits.Shared.Commanding.Tests.Helpers;
using Xunit;

namespace Peereflits.Shared.Commanding.Tests;

public sealed class QueryTestWithCustomException
{
    [Fact]
    public async Task WhenCanNotExecuteRequest_ItShouldThrow()
    {
        var invalidRequest = new TestRequest { Id = 0 };
        var subject = new ParameterizedTestQueryWithCustomException();

        Task Act() => subject.Execute(parameters: invalidRequest);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(testCode: Act);
    }
}
