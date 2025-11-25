using System;
using System.Threading.Tasks;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal sealed class TestCommandWithCustomException : Command<TestRequest>
{
    public override ValueTask<bool> CanExecute(TestRequest parameters) => new(result: parameters.Id > 0);

    protected override async Task OnExecute(TestRequest parameters)
    {
        await Task.CompletedTask;
    }

    protected override void OnCommandException(TestRequest parameters)
    {
        throw new ArgumentOutOfRangeException(paramName: nameof(parameters), message: $"{nameof(parameters.Id)} should be more than zero.");
    }
}
