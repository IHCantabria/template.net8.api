using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Core.Geometries.DTOs;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed partial record ExtentDto : IDto, IEqualityOperators<ExtentDto, ExtentDto, bool>
{
    /// <summary>
    ///     Longitude Min WGS84
    /// </summary>
    internal required decimal LonMin { get; init; }

    /// <summary>
    ///     Longitude Max WGS84
    /// </summary>
    internal required decimal LonMax { get; init; }

    /// <summary>
    ///     Latitude Min WGS84
    /// </summary>
    internal required decimal LatMin { get; init; }

    /// <summary>
    ///     Latitude Max WGS84
    /// </summary>
    internal required decimal LatMax { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Public visibility is required because the type is exposed through a public conversion operator.")]
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
    ///     ADD DOCUMENTATION
    /// </summary>
    internal bool IsValid()
    {
        return LonMin >= -180 && LonMax <= 180 && LatMin >= -90 && LatMax <= 90 && LonMin < LonMax &&
               LatMin < LatMax;
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed partial record PointDto : IDto, IEqualityOperators<PointDto, PointDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required Guid Uuid { get; init; }

    /// <summary>
    ///     Longitude WGS84
    /// </summary>
    internal decimal? Lon { get; init; }

    /// <summary>
    ///     Latitude WGS84
    /// </summary>
    internal decimal? Lat { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Public visibility is required because the type is exposed through a public conversion operator.")]
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
    ///     ADD DOCUMENTATION
    /// </summary>
    internal bool IsValid()
    {
        return Lon is >= -180 and <= 180 && Lat is >= -90 and <= 90;
    }
}