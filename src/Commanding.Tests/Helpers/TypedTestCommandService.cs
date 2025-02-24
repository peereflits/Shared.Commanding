using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class TypedTestCommandService(ITestService testService, ILogger<TypedTestCommandService> logger) 
        : LoggedCommandService<TestRequest>(logger)
{
    public override async Task<bool> CanExecute(TestRequest request) => await testService.CanExecute() && request.Id > 0;
    protected override Task OnExecute(TestRequest request) => testService.Execute();
}