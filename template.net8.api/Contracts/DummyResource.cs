using System.Text.Json.Serialization;
using template.net8.api.Communications.Interfaces;

namespace template.net8.api.Contracts;

/// <summary>
///     Dummy Resource
/// </summary>
public sealed record DummyResource : IPublicApiContract
{
    /// <summary>
    ///     Text
    /// </summary>
    [JsonRequired]
    public required string Text { get; init; }
}