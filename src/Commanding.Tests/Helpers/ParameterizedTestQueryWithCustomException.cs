using System;
using System.Threading.Tasks;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal class ParameterizedTestQueryWithCustomException : Query<TestRequest, bool>
{
    public override string CommandName => nameof(ParameterizedTestQuery);

    public override async Task<bool> CanExecute(TestRequest parameters) => await Task.FromResult(parameters.Id > 0);

    protected override async Task<bool> OnExecute(TestRequest parameters) => await Task.FromResult(true);

    protected override void OnCommandException(TestRequest parameters)
    {
        throw new ArgumentOutOfRangeException($"{nameof(parameters.Id)} should be more than zero.");
    }
}