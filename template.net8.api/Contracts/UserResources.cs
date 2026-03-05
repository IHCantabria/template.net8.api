using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Contracts;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed record AccessTokenResource : IPublicApiContract,
    IEqualityOperators<AccessTokenResource, AccessTokenResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string AccessToken { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string AccessTokenType { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string RefreshToken { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string RefreshTokenType { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed record IdTokenResource : IPublicApiContract,
    IEqualityOperators<IdTokenResource, IdTokenResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string IdToken { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string IdTokenType { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed record UserResource : IPublicApiContract, IEqualityOperators<UserResource, UserResource, bool>
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
    public required string Username { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    [EmailAddress]
    public required string Email { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required bool IsDisabled { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public short? RoleId { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public string? RoleName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public string? FirstName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public string? LastName { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed record UserCreatedResource : IPublicApiContract,
    IEqualityOperators<UserCreatedResource, UserCreatedResource, bool>
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
    public required string Username { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string Email { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required bool IsDisabled { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public short? RoleId { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public string? FirstName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public string? LastName { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed record UserResetedPasswordResource : IPublicApiContract,
    IEqualityOperators<UserResetedPasswordResource, UserResetedPasswordResource, bool>
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
    public required string Password { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string Email { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed record UserStateResource : IPublicApiContract,
    IEqualityOperators<UserStateResource, UserStateResource, bool>
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
    public required string Username { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required bool IsDisabled { get; init; }
}