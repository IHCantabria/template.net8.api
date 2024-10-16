using System.Numerics;
using System.Text.Json.Serialization;
using template.net8.api.Communications.Interfaces;

namespace template.net8.api.Contracts;

/// <summary>
///     Dummy Resource
/// </summary>
public sealed partial record ErrorCodeResource : IPublicApiContract,
    IEqualityOperators<ErrorCodeResource, ErrorCodeResource, bool>
{
    /// <summary>
    ///     Key
    /// </summary>
    [JsonRequired]
    public required string Key { get; init; }

    /// <summary>
    ///     Text
    /// </summary>
    [JsonRequired]
    public required string Description { get; init; }
}