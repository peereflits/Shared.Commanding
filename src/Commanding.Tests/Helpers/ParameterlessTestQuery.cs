using System.Threading.Tasks;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal sealed class ParameterlessTestQuery : Query<bool>
{
    protected override Task<bool> OnExecute() => Task.FromResult(result: true);
}
