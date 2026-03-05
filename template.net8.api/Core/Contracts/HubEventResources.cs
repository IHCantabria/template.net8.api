using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;
using template.net8.api.Core.Interfaces;
using template.net8.api.Core.Json;

namespace template.net8.api.Core.Contracts;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed record HubEventResource : IPublicApiContract, IEqualityOperators<HubEventResource, HubEventResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    [JsonConverter(typeof(CamelCaseStringConverter))]
    public required string Name { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string Description { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string Path { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string Type { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    [JsonConverter(typeof(CamelCaseStringEnumerableConverter))]
    public required IEnumerable<string> Fields { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed record HubInfoMessageResource : IPublicApiContract,
    IEqualityOperators<HubInfoMessageResource, HubInfoMessageResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string Message { get; init; } = string.Empty;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string ConnectionId { get; init; } = string.Empty;
}