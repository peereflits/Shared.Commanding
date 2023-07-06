using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class TypedTestCommandService : LoggedCommandService<TestRequest>
{
    private readonly ITestService testService;

    public TypedTestCommandService(ITestService testService, ILogger<TypedTestCommandService> logger)
            : base(logger) => this.testService = testService;

    public override string CommandName => nameof(TypedTestCommandService);
    public override async Task<bool> CanExecute(TestRequest request) => await testService.CanExecute() && request.Id > 0;
    protected override Task OnExecute(TestRequest request) => testService.Execute();
}