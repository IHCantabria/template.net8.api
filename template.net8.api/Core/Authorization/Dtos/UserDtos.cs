using System.Numerics;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Core.Authorization.DTOs;

/// <summary>
///     User Id Token Base DTO
/// </summary>
[CoreLibrary]
public abstract record UserIdTokenBaseDto : IDto, IEqualityOperators<UserIdTokenBaseDto, UserIdTokenBaseDto, bool>
{
    /// <summary>
    ///     Universal Unique Identifier
    /// </summary>
    public required Guid Uuid { get; init; }

    /// <summary>
    ///     Username
    /// </summary>
    public required string Username { get; init; } = null!;

    /// <summary>
    ///     First Name
    /// </summary>
    public required string? FirstName { get; init; }

    /// <summary>
    ///     Last Name
    /// </summary>
    public required string? LastName { get; init; }

    /// <summary>
    ///     Email Address
    /// </summary>
    public required string Email { get; init; } = null!;

    /// <summary>
    ///     Role Name
    /// </summary>
    public required string? RoleName { get; init; }

    /// <summary>
    ///     User Scopes
    /// </summary>
    public required IEnumerable<string> UserScopes { get; init; } = [];

    /// <summary>
    ///     Role Scopes
    /// </summary>
    public required IEnumerable<string> RoleScopes { get; init; } = [];
}

/// <summary>
///     User Id Token With Scopes Base DTO
/// </summary>
[CoreLibrary]
public abstract record UserIdTokenWithScopesBaseDto : IDto,
    IEqualityOperators<UserIdTokenWithScopesBaseDto, UserIdTokenWithScopesBaseDto, bool>
{
    /// <summary>
    ///     Universal Unique Identifier
    /// </summary>
    public required Guid Uuid { get; init; }

    /// <summary>
    ///     Username
    /// </summary>
    public required string Username { get; init; } = null!;

    /// <summary>
    ///     First Name
    /// </summary>
    public required string? FirstName { get; init; }

    /// <summary>
    ///     Last Name
    /// </summary>
    public required string? LastName { get; init; }

    /// <summary>
    ///     Email Address
    /// </summary>
    public required string Email { get; init; } = null!;

    /// <summary>
    ///     Role Name
    /// </summary>
    public required string? RoleName { get; init; }

    /// <summary>
    ///     Scopes
    /// </summary>
    public required IEnumerable<string> Scopes { get; init; } = [];
}

/// <summary>
///     User Access Token Base DTO
/// </summary>
[CoreLibrary]
public abstract record UserAccessTokenBaseDto : IDto,
    IEqualityOperators<UserAccessTokenBaseDto, UserAccessTokenBaseDto, bool>
{
    /// <summary>
    ///     Universal Unique Identifier
    /// </summary>
    public required Guid Uuid { get; init; }

    /// <summary>
    ///     Username
    /// </summary>
    public required string Username { get; init; } = null!;

    /// <summary>
    ///     First Name
    /// </summary>
    public required string? FirstName { get; init; }

    /// <summary>
    ///     Last Name
    /// </summary>
    public required string? LastName { get; init; }

    /// <summary>
    ///     Email Address
    /// </summary>
    public required string Email { get; init; } = null!;

    /// <summary>
    ///     Role Name
    /// </summary>
    public required string? RoleName { get; init; }

    /// <summary>
    ///     User Scopes
    /// </summary>
    public virtual required IEnumerable<UserScopeBaseDto> UserScopes { get; init; } = [];

    /// <summary>
    ///     Role Scopes
    /// </summary>
    public virtual required IEnumerable<UserScopeBaseDto> RoleScopes { get; init; } = [];
}

/// <summary>
///     User Access Token With Scopes Base DTO
/// </summary>
[CoreLibrary]
public abstract record UserAccessTokenWithScopeBaseDto : IDto,
    IEqualityOperators<UserAccessTokenWithScopeBaseDto, UserAccessTokenWithScopeBaseDto, bool>
{
    /// <summary>
    ///     Universal Unique Identifier
    /// </summary>
    public required Guid Uuid { get; init; }

    /// <summary>
    ///     Username
    /// </summary>
    public required string Username { get; init; } = null!;

    /// <summary>
    ///     First Name
    /// </summary>
    public required string? FirstName { get; init; }

    /// <summary>
    ///     Last Name
    /// </summary>
    public required string? LastName { get; init; }

    /// <summary>
    ///     Email Address
    /// </summary>
    public required string Email { get; init; } = null!;

    /// <summary>
    ///     Role Name
    /// </summary>
    public required string? RoleName { get; init; }

    /// <summary>
    ///     Scope
    /// </summary>
    public virtual UserScopeBaseDto? Scope { get; init; }
}

/// <summary>
///     Access Token DTO
/// </summary>
[CoreLibrary]
public sealed record AccessTokenDto : IDto, IEqualityOperators<AccessTokenDto, AccessTokenDto, bool>
{
    /// <summary>
    ///     Jwt Access Token
    /// </summary>
    public string AccessToken { get; init; } = null!;

    /// <summary>
    ///     Jwt Access Token Type
    /// </summary>
    private static string AccessTokenType => "Bearer";

    /// <summary>
    ///     Refresh Token
    /// </summary>
    public string RefreshToken { get; init; } = null!;

    /// <summary>
    ///     Refresh Token Type
    /// </summary>
    private static string RefreshTokenType => "Guid";
}

/// <summary>
///     Id Token DTO
/// </summary>
[CoreLibrary]
public sealed record IdTokenDto : IDto, IEqualityOperators<IdTokenDto, IdTokenDto, bool>
{
    /// <summary>
    ///     Jwt Id Token
    /// </summary>
    public string IdToken { get; init; } = null!;

    /// <summary>
    ///     Scopes
    /// </summary>
    public IEnumerable<string> Scopes { get; init; } = null!;

    /// <summary>
    ///     Token Type
    /// </summary>
    private static string TokenType => "Bearer";
}

/// <summary>
///     User Scope Base DTO
/// </summary>
[CoreLibrary]
public abstract record UserScopeBaseDto : IDto, IEqualityOperators<UserScopeBaseDto, UserScopeBaseDto, bool>
{
    /// <summary>
    ///     Name
    /// </summary>
    public required string Name { get; init; } = null!;

    /// <summary>
    ///     Claims
    /// </summary>
    public virtual required IEnumerable<ClaimBaseDto> Claims { get; init; } = [];
}

/// <summary>
///     Claim Base DTO
/// </summary>
[CoreLibrary]
public abstract record ClaimBaseDto : IDto, IEqualityOperators<ClaimBaseDto, ClaimBaseDto, bool>
{
    /// <summary>
    ///     id
    /// </summary>
    public required short Id { get; init; }

    /// <summary>
    ///     Name
    /// </summary>
    public required string Name { get; init; } = null!;
}