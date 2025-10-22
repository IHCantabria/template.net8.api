using System.Numerics;
using System.Text.Json.Serialization;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Core.Contracts;

/// <summary>
///     Info Resource
/// </summary>
[CoreLibrary]
public sealed record InfoResource : IPublicApiContract, IEqualityOperators<InfoResource, InfoResource, bool>
{
    /// <summary>
    ///     Name
    /// </summary>
    [JsonRequired]
    public required string Version { get; init; } = null!;

    /// <summary>
    ///     Name
    /// </summary>
    [JsonRequired]
    public required short Status { get; init; }
}