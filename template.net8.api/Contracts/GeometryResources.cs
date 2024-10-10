using System.Numerics;
using System.Text.Json.Serialization;
using template.net8.api.Communications.Interfaces;

namespace template.net8.api.Contracts;

/// <summary>
///     Create Extent Resource
/// </summary>
public sealed partial record CreateExtentResource : IPublicApiContract,
    IEqualityOperators<CreateExtentResource, CreateExtentResource, bool>
{
    /// <summary>
    ///     Longitude Min WGS84
    /// </summary>
    [JsonRequired]
    public required decimal LonMin { get; init; }

    /// <summary>
    ///     Longitude Max WGS84
    /// </summary>
    [JsonRequired]
    public required decimal LonMax { get; init; }

    /// <summary>
    ///     Latitude Min WGS84
    /// </summary>
    [JsonRequired]
    public required decimal LatMin { get; init; }

    /// <summary>
    ///     Latitude Max WGS84
    /// </summary>
    [JsonRequired]
    public required decimal LatMax { get; init; }
}

/// <summary>
///     Extent Resource
/// </summary>
public sealed record ExtentResource : IPublicApiContract, IEqualityOperators<ExtentResource, ExtentResource, bool>
{
    /// <summary>
    ///     Longitude Min WGS84
    /// </summary>
    [JsonRequired]
    public required decimal LonMin { get; init; }

    /// <summary>
    ///     Longitude Max WGS84
    /// </summary>
    [JsonRequired]
    public required decimal LonMax { get; init; }

    /// <summary>
    ///     Latitude Min WGS84
    /// </summary>
    [JsonRequired]
    public required decimal LatMin { get; init; }

    /// <summary>
    ///     Latitude Max WGS84
    /// </summary>
    [JsonRequired]
    public required decimal LatMax { get; init; }
}

/// <summary>
///     Create Point Resource
/// </summary>
public sealed partial record CreatePointResource : IPublicApiContract,
    IEqualityOperators<CreatePointResource, CreatePointResource, bool>
{
    /// <summary>
    ///     Longitude WGS84
    /// </summary>
    [JsonRequired]
    public required decimal Lon { get; init; }

    /// <summary>
    ///     Latitude WGS84
    /// </summary>
    [JsonRequired]
    public required decimal Lat { get; init; }
}

/// <summary>
///     Point Resource
/// </summary>
public sealed record PointResource : IPublicApiContract, IEqualityOperators<PointResource, PointResource, bool>
{
    /// <summary>
    ///     Universal Unique Identifier
    /// </summary>
    [JsonRequired]
    public required Guid Uuid { get; init; }

    /// <summary>
    ///     Longitude WGS84
    /// </summary>
    [JsonRequired]
    public decimal? Lon { get; init; }

    /// <summary>
    ///     Latitude WGS84
    /// </summary>
    [JsonRequired]
    public decimal? Lat { get; init; }
}