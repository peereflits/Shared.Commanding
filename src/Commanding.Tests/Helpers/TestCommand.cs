using System.Threading.Tasks;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class TestCommand : Command<TestRequest>
{
    public override ValueTask<bool> CanExecute(TestRequest parameters) => new(parameters.Id > 0);

    protected override async Task OnExecute(TestRequest parameters)
    {
        await Task.CompletedTask;
    }
}