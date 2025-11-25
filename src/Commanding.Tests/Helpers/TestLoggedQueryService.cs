using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal sealed class TestLoggedQueryService
(
    ITestService testService
  , ILogger<TestLoggedQueryService> logger
) : LoggedQueryService<bool>(logger: logger)
{
    public override ValueTask<bool> CanExecute() => testService.CanExecute();

    protected override Task<bool> OnExecute() => testService.ExecuteWithResult<bool>();
}
