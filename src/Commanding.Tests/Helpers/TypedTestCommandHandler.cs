using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class TypedTestCommandHandler(ITestService testService, ILogger<TypedTestCommandHandler> logger) 
        : LoggedCommandHandler<TestRequest>(logger)
{
    public override async ValueTask<bool> CanExecute(TestRequest request) => request.Id > 0 && await testService.CanExecute();
    protected override Task OnExecute(TestRequest request) => testService.Execute();
}