using System.Numerics;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Core.DTOs;

/// <summary>
///     Extent DTO
/// </summary>
[CoreLibrary]
public sealed partial record ExtentDto : IDto, IEqualityOperators<ExtentDto, ExtentDto, bool>
{
    /// <summary>
    ///     Longitude Min WGS84
    /// </summary>
    public required decimal LonMin { get; init; }

    /// <summary>
    ///     Longitude Max WGS84
    /// </summary>
    public required decimal LonMax { get; init; }

    /// <summary>
    ///     Latitude Min WGS84
    /// </summary>
    public required decimal LatMin { get; init; }

    /// <summary>
    ///     Latitude Max WGS84
    /// </summary>

    public required decimal LatMax { get; init; }
}

/// <summary>
///     Create Extent DTO
/// </summary>
[CoreLibrary]
public sealed record CreateExtentDto : IDto, IEqualityOperators<CreateExtentDto, CreateExtentDto, bool>
{
    /// <summary>
    ///     Longitude Min WGS84
    /// </summary>
    public required decimal LonMin { get; init; }

    /// <summary>
    ///     Longitude Max WGS84
    /// </summary>
    public required decimal LonMax { get; init; }

    /// <summary>
    ///     Latitude Min WGS84
    /// </summary>
    public required decimal LatMin { get; init; }

    /// <summary>
    ///     Latitude Max WGS84
    /// </summary>

    public required decimal LatMax { get; init; }

    /// <summary>
    ///     Extent Is Valid
    /// </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        return LonMin >= -180 && LonMax <= 180 && LatMin >= -90 && LatMax <= 90 && LonMin < LonMax &&
               LatMin < LatMax;
    }
}

/// <summary>
///     Point DTO
/// </summary>
[CoreLibrary]
public sealed partial record PointDto : IDto, IEqualityOperators<PointDto, PointDto, bool>
{
    /// <summary>
    ///     Universal Unique Identifier
    /// </summary>
    public required Guid Uuid { get; init; }

    /// <summary>
    ///     Longitude WGS84
    /// </summary>
    public decimal? Lon { get; init; }

    /// <summary>
    ///     Latitude WGS84
    /// </summary>
    public decimal? Lat { get; init; }
}

/// <summary>
///     Create Point DTO
/// </summary>
[CoreLibrary]
public sealed record CreatePointDto : IDto, IEqualityOperators<CreatePointDto, CreatePointDto, bool>
{
    /// <summary>
    ///     Longitude WGS84
    /// </summary>
    public required decimal Lon { get; init; }

    /// <summary>
    ///     Latitude WGS84
    /// </summary>
    public required decimal Lat { get; init; }

    /// <summary>
    ///     Extent Is Valid
    /// </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        return Lon is >= -180 and <= 180 && Lat is >= -90 and <= 90;
    }
}