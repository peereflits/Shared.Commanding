using System.Threading.Tasks;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class TestCommand : Command<TestRequest>
{
    public override string CommandName => nameof(TestCommand);
    public override async Task<bool> CanExecute(TestRequest parameters) => await Task.FromResult(parameters.Id > 0);

    protected override async Task OnExecute(TestRequest parameters)
    {
        await Task.CompletedTask;
    }
}