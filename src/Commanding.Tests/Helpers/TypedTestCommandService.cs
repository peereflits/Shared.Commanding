using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal sealed class TypedTestCommandService
(
    ITestService testService
  , ILogger<TypedTestCommandService> logger
)
        : LoggedCommandService<TestRequest>(logger: logger)
{
    public override async ValueTask<bool> CanExecute(TestRequest request) => request.Id > 0 && await testService.CanExecute();
    protected override Task OnExecute(TestRequest request) => testService.Execute();
}
