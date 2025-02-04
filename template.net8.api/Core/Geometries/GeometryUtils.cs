using LanguageExt;
using NetTopologySuite.Geometries;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Geometries.DTOs;

namespace template.net8.api.Core.Geometries;

[CoreLibrary]
internal static class GeometryUtils
{
    /// <exception cref="ArgumentException">If the ring is not closed, or has too few points</exception>
    internal static Try<Geometry> CreateExtentFromExtentDto(CreateExtentDto extent)
    {
        return () =>
        {
            // Check that the input array has exactly four coordinates
            if (!extent.IsValid())
                return new LanguageExt.Common.Result<Geometry>(new CoreException("Extent is not valid"));

            var linearRing = new LinearRing([
                new Coordinate((double)extent.LonMin, (double)extent.LatMin),
                new Coordinate((double)extent.LonMax, (double)extent.LatMin),
                new Coordinate((double)extent.LonMax, (double)extent.LatMax),
                new Coordinate((double)extent.LonMin, (double)extent.LatMax),
                new Coordinate((double)extent.LonMin, (double)extent.LatMin)
            ]);
            var geom = new Polygon(linearRing) { SRID = 4326 };
            return geom;
        };
    }

    internal static Try<Geometry> CreatePointFromPointDto(CreatePointDto point)
    {
        return () =>
        {
            // Check that the input array has exactly four coordinates
            if (!point.IsValid())
                return new LanguageExt.Common.Result<Geometry>(new CoreException("Point is not valid"));

            var geom = new Point(
                new Coordinate((double)point.Lon, (double)point.Lat)
            ) { SRID = 4326 };
            return geom;
        };
    }

    /// <exception cref="OverflowException">
    ///     <paramref>
    ///         <name>value</name>
    ///     </paramref>
    ///     is greater than <see cref="System.Decimal.MaxValue">Decimal.MaxValue</see> or less than
    ///     <see cref="System.Decimal.MinValue">Decimal.MinValue</see>.
    ///     -or-
    ///     <paramref>
    ///         <name>value</name>
    ///     </paramref>
    ///     is <see cref="System.Double.NaN" />, <see cref="System.Double.PositiveInfinity" />, or
    ///     <see cref="System.Double.NegativeInfinity" />.
    /// </exception>
    internal static Try<ExtentDto> CreateExtentDtoFromGeometry(Geometry geometry)
    {
        return () =>
        {
            if (geometry == null || geometry.IsEmpty)
                return new LanguageExt.Common.Result<ExtentDto>(new CoreException("Input geometry is null or empty."));

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
                : new LanguageExt.Common.Result<ExtentDto>(new CoreException("Input geometry is not a valid extent"));
        };
    }

    /// <exception cref="OverflowException">
    ///     <paramref>
    ///         <name>value</name>
    ///     </paramref>
    ///     is greater than <see cref="System.Decimal.MaxValue">Decimal.MaxValue</see> or less than
    ///     <see cref="System.Decimal.MinValue">Decimal.MinValue</see>.
    ///     -or-
    ///     <paramref>
    ///         <name>value</name>
    ///     </paramref>
    ///     is <see cref="System.Double.NaN" />, <see cref="System.Double.PositiveInfinity" />, or
    ///     <see cref="System.Double.NegativeInfinity" />.
    /// </exception>
    internal static Try<PointDto> CreatePointDtoFromGeometry(Geometry geometry, Guid uuid)
    {
        return () =>
        {
            if (geometry == null || geometry.IsEmpty)
                return new LanguageExt.Common.Result<PointDto>(new CoreException("Input geometry is null or empty."));

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
                : new LanguageExt.Common.Result<PointDto>(new CoreException("Input geometry is not a valid point"));
        };
    }
}