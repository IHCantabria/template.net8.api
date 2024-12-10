using System.Numerics;
using System.Text.Json.Serialization;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Core.Authorization.Contracts;

/// <summary>
///     User Access Token Resource
/// </summary>
public sealed record AccessTokenResource : IPublicApiContract,
    IEqualityOperators<AccessTokenResource, AccessTokenResource, bool>
{
    /// <summary>
    ///     Jwt Access Token
    /// </summary>
    [JsonRequired]
    public required string AccessToken { get; init; } = null!;

    /// <summary>
    ///     Jwt Access Token Type
    /// </summary>
    [JsonRequired]
    public required string AccessTokenType { get; init; } = null!;

    /// <summary>
    ///     Refresh Token
    /// </summary>
    [JsonRequired]
    public required string RefreshToken { get; init; } = null!;

    /// <summary>
    ///     Refresh Token Type
    /// </summary>
    [JsonRequired]
    public required string RefreshTokenType { get; init; } = null!;
}

/// <summary>
///     User Id Token Resource
/// </summary>
public sealed record IdTokenResource : IPublicApiContract,
    IEqualityOperators<IdTokenResource, IdTokenResource, bool>
{
    /// <summary>
    ///     Jwt Id Token
    /// </summary>
    [JsonRequired]
    public required string IdToken { get; init; } = null!;

    /// <summary>
    ///     Jwt Id Token Type
    /// </summary>
    [JsonRequired]
    public required string IdTokenType { get; init; } = null!;

    /// <summary>
    ///     Access Scopes
    /// </summary>
    [JsonRequired]
    public required IEnumerable<string> Scopes { get; init; } = [];
}