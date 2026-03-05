using System.Diagnostics.CodeAnalysis;
using LanguageExt;
using NetTopologySuite.Geometries;
using template.net8.api.Core.Geometries.DTOs;

namespace template.net8.api.Core.Geometries;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "ReSharper",
    "UnusedType.Global",
    Justification = "General-purpose helper type; usage depends on consumer requirements.")]
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification = "General-purpose helper methods; not all members are used in every scenario.")]
internal static class GeometryUtils
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentException">If the ring is not closed, or has too few points</exception>
    internal static Try<Geometry> CreateExtentFromExtentDto(CreateExtentDto extent)
    {
        return () =>
        {
            // Check that the input array has exactly four coordinates
            if (!extent.IsValid())
                return new LanguageExt.Common.Result<Geometry>(
                    new GeometryExtentNotValidException("Extent is not valid"));

            var linearRing = new LinearRing([
                new Coordinate((double)extent.LonMin, (double)extent.LatMin),
                new Coordinate((double)extent.LonMax, (double)extent.LatMin),
                new Coordinate((double)extent.LonMax, (double)extent.LatMax),
                new Coordinate((double)extent.LonMin, (double)extent.LatMax),
                new Coordinate((double)extent.LonMin, (double)extent.LatMin)
            ]);
            return new Polygon(linearRing) { SRID = 4326 };
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static Try<Geometry> CreatePointFromPointDto(CreatePointDto point)
    {
        return () =>
        {
            // Check that the input array has exactly four coordinates
            if (!point.IsValid())
                return new LanguageExt.Common.Result<Geometry>(
                    new GeometryPointNotValidException("Point is not valid"));

            return new Point(
                new Coordinate((double)point.Lon, (double)point.Lat)
            ) { SRID = 4326 };
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static Try<ExtentDto> CreateExtentDtoFromGeometry(Geometry geometry)
    {
        return () =>
        {
            if (geometry == null || geometry.IsEmpty)
                return new LanguageExt.Common.Result<ExtentDto>(
                    new GeometryExtentNotValidException("Input geometry is null or empty."));

            // Extract the Envelope from the Geometry
            var envelope = geometry.Envelope;

            Coordinate[] coordinates = envelope.Coordinates;

            // Check if the geometry is a polygon and if it has enough coordinates
            return geometry is Polygon && coordinates.Length >= 4
                ? new ExtentDto
                {
                    LonMin = new decimal(coordinates[0].X),
                    LatMin = new decimal(coordinates[0].Y),
                    LonMax = new decimal(coordinates[2].X),
                    LatMax = new decimal(coordinates[2].Y)
                }
                : new LanguageExt.Common.Result<ExtentDto>(
                    new GeometryExtentNotValidException("Input geometry is not a valid extent"));
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static Try<PointDto> CreatePointDtoFromGeometry(Geometry geometry, Guid uuid)
    {
        return () =>
        {
            if (geometry == null || geometry.IsEmpty)
                return new LanguageExt.Common.Result<PointDto>(
                    new GeometryPointNotValidException("Input geometry is null or empty."));

            // Extract the Envelope from the Geometry
            var envelope = geometry.Envelope;

            Coordinate[] coordinates = envelope.Coordinates;

            // Check if the geometry is a polygon and if it has enough coordinates
            return geometry is Point && coordinates.Length == 1
                ? new PointDto
                {
                    Uuid = uuid,
                    Lon = new decimal(coordinates[0].X),
                    Lat = new decimal(coordinates[0].Y)
                }
                : new LanguageExt.Common.Result<PointDto>(
                    new GeometryPointNotValidException("Input geometry is not a valid point"));
        };
    }
}