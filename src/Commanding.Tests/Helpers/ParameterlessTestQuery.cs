using System.Threading.Tasks;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class ParameterlessTestQuery : Query<bool>
{
    protected override Task<bool> OnExecute() => Task.FromResult(true);
}