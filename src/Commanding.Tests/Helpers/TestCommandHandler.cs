using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class TestCommandHandler(ITestService testService, ILogger<TestCommandHandler> logger) : LoggedCommandHandler(logger)
{
    public override Task<bool> CanExecute() => testService.CanExecute();
    protected override Task OnExecute() => testService.Execute();
}