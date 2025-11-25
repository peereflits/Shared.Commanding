using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal sealed class TypedTestQueryService
(
    ITestService testService
  , ILogger<TypedTestQueryService> logger
)
        : LoggedQueryService<TestRequest, bool>(logger: logger)
{
    public override async ValueTask<bool> CanExecute(TestRequest request) => request.Id > 0 && await testService.CanExecute();
    protected override Task<bool> OnExecute(TestRequest request) => testService.ExecuteWithResult<bool>();
}
