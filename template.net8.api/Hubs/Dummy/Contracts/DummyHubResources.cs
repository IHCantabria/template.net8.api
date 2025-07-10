using template.net8.api.Core.Interfaces;

namespace template.net8.api.Hubs.Dummy.Contracts;

/// <summary>
///     Dummy Hub New Dummy Message Resource
/// </summary>
public sealed record DummyHubNewDummyMessageResource : IPublicApiContract
{
    /// <summary>
    ///     Message
    /// </summary>
    public required string Message { get; init; } = string.Empty;

    /// <summary>
    ///     Dummy Key
    /// </summary>
    public required string DummyKey { get; init; } = string.Empty;
}