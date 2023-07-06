using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class TypedTestQueryService : LoggedQueryService<TestRequest, bool>
{
    private readonly ITestService testService;

    public TypedTestQueryService(ITestService testService, ILogger<TypedTestQueryService> logger)
            : base(logger) => this.testService = testService;

    public override string CommandName => nameof(TypedTestQueryService);
    public override async Task<bool> CanExecute(TestRequest request) => await testService.CanExecute() && request.Id > 0;
    protected override Task<bool> OnExecute(TestRequest request) => testService.ExecuteWithResult<bool>();
}