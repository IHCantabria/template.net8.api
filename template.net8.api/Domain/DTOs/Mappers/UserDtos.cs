using System.Diagnostics.CodeAnalysis;
using template.net8.api.Contracts;
using template.net8.api.Persistence.Models;

namespace template.net8.api.Domain.DTOs;

internal sealed partial record AccessTokenDto
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static implicit operator AccessTokenResource(AccessTokenDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return new AccessTokenResource
        {
            AccessToken = dto.AccessToken,
            AccessTokenType = AccessTokenType,
            RefreshToken = dto.RefreshToken,
            RefreshTokenType = RefreshTokenType
        };
    }
}

internal sealed partial record IdTokenDto
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static implicit operator IdTokenResource(IdTokenDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return new IdTokenResource
        {
            IdToken = dto.IdToken,
            IdTokenType = TokenType
        };
    }
}

[SuppressMessage(
    "Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Public visibility is required as the DTO is part of the API contract and exposed through public conversions.")]
public sealed partial record UserDto
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static implicit operator UserResource(UserDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return new UserResource
        {
            Uuid = dto.Uuid,
            Username = dto.Username,
            Email = dto.Email,
            IsDisabled = dto.IsDisabled,
            RoleId = dto.RoleId,
            RoleName = dto.RoleName,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "UnusedMember.Global",
        Justification = "Required as a named alternative for the implicit conversion operator.")]
    public static UserResource ToUserResource(UserDto dto)
    {
        return dto;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static IEnumerable<UserResource> ToCollection(
        IReadOnlyList<UserDto> dtos)
    {
        var resources = new UserResource[dtos.Count];
        for (var i = 0; i < dtos.Count; i++) resources[i] = dtos[i];
        return resources;
    }
}

[SuppressMessage(
    "Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Public visibility is required as the DTO is part of the API contract and exposed through public conversions.")]
public sealed partial record UserResetedPasswordDto
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static implicit operator UserResetedPasswordResource(UserResetedPasswordDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return new UserResetedPasswordResource
        {
            Uuid = dto.Uuid,
            Password = dto.Password,
            Email = dto.Email
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "UnusedMember.Global",
        Justification = "Required as a named alternative for the implicit conversion operator.")]
    public static UserResetedPasswordResource ToUserResetedPasswordResource(UserResetedPasswordDto dto)
    {
        return dto;
    }
}

internal sealed partial record CreateUserDto
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static implicit operator User(CreateUserDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return new User
        {
            Username = dto.Username,
            Email = dto.Email,
            IsDisabled = dto.IsDisabled,
            RoleId = dto.RoleId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            InsertDatetime = DateTime.SpecifyKind(DateTime.UtcNow,
                DateTimeKind.Unspecified),
            PasswordHash = dto.PasswordHash,
            PasswordSalt = dto.PasswordSalt,
            Uuid = dto.Uuid,
            InsertUserId = null,
            UpdateDatetime = DateTime.SpecifyKind(DateTime.UtcNow,
                DateTimeKind.Unspecified),
            UpdateUserId = null,
            Id = 0
        };
    }
}