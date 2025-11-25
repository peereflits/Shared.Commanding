using System.Threading.Tasks;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal sealed class TestCommand : Command<TestRequest>
{
    public override ValueTask<bool> CanExecute(TestRequest parameters) => new(result: parameters.Id > 0);

    protected override async Task OnExecute(TestRequest parameters)
    {
        await Task.CompletedTask;
    }
}
