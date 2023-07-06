using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class TestCommandService : LoggedCommandService
{
    private readonly ITestService testService;

    public TestCommandService(ITestService testService, ILogger<TestCommandService> logger)
            : base(logger) => this.testService = testService;

    public override string CommandName => nameof(TestCommandService);
    public override Task<bool> CanExecute() => testService.CanExecute();
    protected override Task OnExecute() => testService.Execute();
}