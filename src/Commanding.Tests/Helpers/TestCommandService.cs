using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal sealed class TestCommandService
(
    ITestService testService
  , ILogger<TestCommandService> logger
) : LoggedCommandService(logger: logger)
{
    public override ValueTask<bool> CanExecute() => testService.CanExecute();
    protected override Task OnExecute() => testService.Execute();
}
