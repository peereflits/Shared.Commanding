using System;
using System.Threading.Tasks;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal sealed class ParameterizedTestQueryWithCustomException : Query<TestRequest, bool>
{
    public override ValueTask<bool> CanExecute(TestRequest parameters) => new(result: parameters.Id > 0);

    protected override async Task<bool> OnExecute(TestRequest parameters) => await Task.FromResult(result: true);

    protected override void OnCommandException(TestRequest parameters)
    {
        throw new ArgumentOutOfRangeException(paramName: nameof(parameters), message: $"{nameof(parameters.Id)} should be more than zero.");
    }
}
