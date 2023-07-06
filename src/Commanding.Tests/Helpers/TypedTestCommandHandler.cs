using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class TypedTestCommandHandler : LoggedCommandHandler<TestRequest>
{
    private readonly ITestService testService;

    public TypedTestCommandHandler(ITestService testService, ILogger<TypedTestCommandHandler> logger)
            : base(logger) => this.testService = testService;

    public override string CommandName => nameof(TypedTestCommandHandler);
    public override async Task<bool> CanExecute(TestRequest request) => await testService.CanExecute() && request.Id > 0;
    protected override Task OnExecute(TestRequest request) => testService.Execute();
}