using System.Numerics;
using JetBrains.Annotations;
using template.net8.api.Core.Authorization.DTOs.Base;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Domain.DTOs;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed partial record AccessTokenDto : IDto,
    IEqualityOperators<AccessTokenDto, AccessTokenDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string AccessToken { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static string AccessTokenType => "Bearer";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string RefreshToken { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static string RefreshTokenType => "Guid";
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed partial record IdTokenDto : IDto, IEqualityOperators<IdTokenDto, IdTokenDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string IdToken { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static string TokenType => "Bearer";
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
public sealed partial record UserDto : IDto, IEqualityOperators<UserDto, UserDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required Guid Uuid { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string Username { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required bool IsDisabled { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required short? RoleId { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? RoleName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? FirstName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? LastName { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
public sealed partial record UserResetedPasswordDto : IDto,
    IEqualityOperators<UserResetedPasswordDto, UserResetedPasswordDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required Guid Uuid { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string Password { get; set; } = "";
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed record UserCredentialsDto : IDto, IEqualityOperators<UserCredentialsDto, UserCredentialsDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string PasswordHash { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string PasswordSalt { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed partial record CreateUserDto : IDto,
    IEqualityOperators<CreateUserDto, CreateUserDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required Guid Uuid { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string Username { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string Email { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required bool IsDisabled { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string PasswordHash { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string PasswordSalt { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required short? RoleId { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal string? FirstName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal string? LastName { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed record CreateUserPasswordDto : IDto,
    IEqualityOperators<CreateUserPasswordDto, CreateUserPasswordDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string Password { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string Pepper { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed record VerifyUserPasswordDto : IDto,
    IEqualityOperators<VerifyUserPasswordDto, VerifyUserPasswordDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string Password { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string Pepper { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string Salt { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string Hash { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed record UserIdTokenDto : UserIdTokenBaseDto,
    IEqualityOperators<UserIdTokenDto, UserIdTokenDto, bool>;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed record UserAccessTokenDto : UserAccessTokenBaseDto,
    IEqualityOperators<UserAccessTokenDto, UserAccessTokenDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal new required IEnumerable<ClaimDto> RoleClaims { [UsedImplicitly] get; init; } = [];

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal new required IEnumerable<ClaimDto> UserClaims { [UsedImplicitly] get; init; } = [];
}