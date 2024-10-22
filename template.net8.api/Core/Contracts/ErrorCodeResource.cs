using System.Numerics;
using System.Text.Json.Serialization;
using template.net8.api.Communications.Interfaces;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Contracts;

/// <summary>
///     Error Code Resource
/// </summary>
[CoreLibrary]
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