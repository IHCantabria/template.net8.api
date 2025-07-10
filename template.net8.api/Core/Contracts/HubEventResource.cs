using System.Text.Json.Serialization;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Interfaces;
using template.net8.api.Core.Json;

namespace template.net8.api.Core.Contracts;

/// <summary>
///     Hub Event Resource
/// </summary>
[CoreLibrary]
public sealed record HubEventResource : IPublicApiContract
{
    /// <summary>Event Name</summary>
    [JsonRequired]
    [JsonConverter(typeof(CamelCaseStringConverter))]
    public required string Name { get; init; }

    /// <summary>Event Description</summary>
    [JsonRequired]
    public required string Description { get; init; }

    /// <summary>Event Path</summary>
    [JsonRequired]
    public required string Path { get; init; }

    /// <summary>Event Type</summary>
    [JsonRequired]
    public required string Type { get; init; }

    /// <summary>Fields</summary>
    [JsonRequired]
    [JsonConverter(typeof(CamelCaseStringEnumerableConverter))]
    public required IEnumerable<string> Fields { get; init; }
}

/// <summary>
///     Hub Info Message Resource
/// </summary>
[CoreLibrary]
public sealed record HubInfoMessageResource : IPublicApiContract
{
    /// <summary>
    ///     Message
    /// </summary>
    public required string Message { get; init; } = string.Empty;

    /// <summary>
    ///     Connection Id
    /// </summary>
    public required string ConnectionId { get; init; } = string.Empty;
}