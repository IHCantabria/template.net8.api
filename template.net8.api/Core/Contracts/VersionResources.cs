using System.Numerics;
using System.Text.Json.Serialization;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Core.Contracts;

/// <summary>
///     Version Resource
/// </summary>
[CoreLibrary]
public sealed record VersionResource : IPublicApiContract, IEqualityOperators<VersionResource, VersionResource, bool>
{
    /// <summary>
    ///     id
    /// </summary>
    [JsonRequired]
    public required short Id { get; init; }

    /// <summary>
    ///     Name
    /// </summary>
    [JsonRequired]
    public required string Name { get; init; } = null!;

    /// <summary>
    ///     Tag
    /// </summary>
    [JsonRequired]
    public required string Tag { get; init; } = null!;

    /// <summary>
    ///     Date UTC
    /// </summary>
    [JsonRequired]
    public required DateTime Date { get; init; }
}