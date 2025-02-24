using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class TypedTestQueryService(ITestService testService, ILogger<TypedTestQueryService> logger) 
        : LoggedQueryService<TestRequest, bool>(logger)
{
    public override async Task<bool> CanExecute(TestRequest request) => await testService.CanExecute() && request.Id > 0;
    protected override Task<bool> OnExecute(TestRequest request) => testService.ExecuteWithResult<bool>();
}