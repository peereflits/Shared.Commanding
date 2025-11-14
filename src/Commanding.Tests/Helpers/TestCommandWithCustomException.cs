using System;
using System.Threading.Tasks;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class TestCommandWithCustomException : Command<TestRequest>
{
    public override ValueTask<bool> CanExecute(TestRequest parameters) => new(parameters.Id > 0);

    protected override async Task OnExecute(TestRequest parameters)
    {
        await Task.CompletedTask;
    }

    protected override void OnCommandException(TestRequest parameters)
    {
        throw new ArgumentOutOfRangeException($"{nameof(parameters.Id)} should be more than zero.");
    }
}