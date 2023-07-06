using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class TestCommandHandler : LoggedCommandHandler
{
    private readonly ITestService testService;

    public TestCommandHandler(ITestService testService, ILogger<TestCommandHandler> logger)
            : base(logger) => this.testService = testService;

    public override string CommandName => nameof(TestCommandHandler);
    public override Task<bool> CanExecute() => testService.CanExecute();
    protected override Task OnExecute() => testService.Execute();
}