using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class TestLoggedQueryService : LoggedQueryService<bool>
{
    private readonly ITestService testService;

    public TestLoggedQueryService(ITestService testService, ILogger<TestLoggedQueryService> logger)
            : base(logger) => this.testService = testService;

    public override Task<bool> CanExecute() => testService.CanExecute();

    public override string CommandName => nameof(TestLoggedQueryService);
    protected override Task<bool> OnExecute() => testService.ExecuteWithResult<bool>();
}