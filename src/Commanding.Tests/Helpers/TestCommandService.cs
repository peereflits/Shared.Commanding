using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class TestCommandService(ITestService testService, ILogger<TestCommandService> logger) : LoggedCommandService(logger)
{
    public override Task<bool> CanExecute() => testService.CanExecute();
    protected override Task OnExecute() => testService.Execute();
}