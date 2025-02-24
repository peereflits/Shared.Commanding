using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class TestLoggedQueryHandler(ITestService testService, ILogger<TestLoggedQueryHandler> logger) : LoggedQueryHandler<bool>(logger)
{
    public override Task<bool> CanExecute() => testService.CanExecute();

    protected override Task<bool> OnExecute() => testService.ExecuteWithResult<bool>();
}