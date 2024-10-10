using System.Numerics;
using System.Text.Json.Serialization;
using template.net8.api.Communications.Interfaces;

namespace template.net8.api.Contracts;

/// <summary>
///     Dummy Resource
/// </summary>
public sealed record DummyResource : IPublicApiContract, IEqualityOperators<DummyResource, DummyResource, bool>
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
    public required string Text { get; init; }
}

/// <summary>
///     Dummy Resource
/// </summary>
public sealed record DummyCreatedResource : IPublicApiContract,
    IEqualityOperators<DummyCreatedResource, DummyCreatedResource, bool>
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
    public required string Text { get; init; }
}