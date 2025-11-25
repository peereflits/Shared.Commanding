using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal sealed class TestLoggedQueryHandler
(
    ITestService testService
  , ILogger<TestLoggedQueryHandler> logger
) : LoggedQueryHandler<bool>(logger: logger)
{
    public override ValueTask<bool> CanExecute() => testService.CanExecute();

    protected override Task<bool> OnExecute() => testService.ExecuteWithResult<bool>();
}
