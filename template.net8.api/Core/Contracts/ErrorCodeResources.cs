using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Core.Contracts;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed partial record ErrorCodeResource : IPublicApiContract,
    IEqualityOperators<ErrorCodeResource, ErrorCodeResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string Key { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string Description { get; init; }
}