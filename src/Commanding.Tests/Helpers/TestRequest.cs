namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal sealed record TestRequest : IRequest
{
    public int Id { get; init; }
}
