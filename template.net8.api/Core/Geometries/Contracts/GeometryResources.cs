using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Core.Geometries.Contracts;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed partial record CreateExtentResource : IPublicApiContract,
    IEqualityOperators<CreateExtentResource, CreateExtentResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required decimal LonMin { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required decimal LonMax { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required decimal LatMin { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required decimal LatMax { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed record ExtentResource : IPublicApiContract, IEqualityOperators<ExtentResource, ExtentResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required decimal LonMin { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required decimal LonMax { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required decimal LatMin { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required decimal LatMax { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed partial record CreatePointResource : IPublicApiContract,
    IEqualityOperators<CreatePointResource, CreatePointResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required decimal Lon { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required decimal Lat { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed record PointResource : IPublicApiContract, IEqualityOperators<PointResource, PointResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required Guid Uuid { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public decimal? Lon { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public decimal? Lat { get; init; }
}