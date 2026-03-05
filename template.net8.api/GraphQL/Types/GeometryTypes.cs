using HotChocolate.Data.Sorting;
using JetBrains.Annotations;
using NetTopologySuite.Geometries;

namespace template.net8.api.GraphQL.Types;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal class PointSortInputType : SortInputType<Point>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="descriptor" /> is <see langword="null" />.</exception>
    protected override void Configure(ISortInputTypeDescriptor<Point> descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        descriptor.Ignore(static f => f.Boundary);
        descriptor.Ignore(static f => f.Envelope);
        descriptor.Ignore(static f => f.Factory);
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal class PolygonSortInputType : SortInputType<Polygon>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="descriptor" /> is <see langword="null" />.</exception>
    protected override void Configure(ISortInputTypeDescriptor<Polygon> descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        descriptor.Ignore(static f => f.Boundary);
        descriptor.Ignore(static f => f.Envelope);
        descriptor.Ignore(static f => f.ExteriorRing);
        descriptor.Ignore(static f => f.Factory);
        descriptor.Ignore(static f => f.Shell);
    }
}