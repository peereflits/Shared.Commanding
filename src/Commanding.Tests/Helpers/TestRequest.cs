namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal record TestRequest : IRequest
{
    public int Id { get; init; }
}