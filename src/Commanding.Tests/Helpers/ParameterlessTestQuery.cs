using System.Threading.Tasks;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class ParameterlessTestQuery : Query<bool>
{
    public override string CommandName => nameof(ParameterlessTestQuery);

    public override async Task<bool> Execute() => await Task.FromResult(true);
}