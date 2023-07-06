using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class TestLoggedQueryHandler : LoggedQueryHandler<bool>
{
    private readonly ITestService testService;

    public TestLoggedQueryHandler(ITestService testService, ILogger<TestLoggedQueryHandler> logger)
            : base(logger) => this.testService=testService;

    public override Task<bool> CanExecute() => testService.CanExecute();

    public override string CommandName => nameof(TestLoggedQueryHandler);
    protected override Task<bool> OnExecute() => testService.ExecuteWithResult<bool>();
}