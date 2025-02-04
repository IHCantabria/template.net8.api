using HotChocolate.Data.Sorting;
using JetBrains.Annotations;
using NetTopologySuite.Geometries;

namespace template.net8.api.GraphQL.Types;

/// <summary>
/// </summary>
[UsedImplicitly]
internal class PointSortInputType : SortInputType<Point>
{
    /// <summary>
    ///     Configure Point Sort Input Type
    /// </summary>
    /// <param name="descriptor"></param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    protected override void Configure(ISortInputTypeDescriptor<Point> descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        descriptor.Ignore(f => f.Boundary);
        descriptor.Ignore(f => f.Envelope);
        descriptor.Ignore(f => f.Factory);
    }
}

[UsedImplicitly]
internal class PolygonSortInputType : SortInputType<Polygon>
{
    /// <summary>
    ///     Configure Polygon Sort Input Type
    /// </summary>
    /// <param name="descriptor"></param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    protected override void Configure(ISortInputTypeDescriptor<Polygon> descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        descriptor.Ignore(f => f.Boundary);
        descriptor.Ignore(f => f.Envelope);
        descriptor.Ignore(f => f.ExteriorRing);
        descriptor.Ignore(f => f.Factory);
        descriptor.Ignore(f => f.Shell);
    }
}