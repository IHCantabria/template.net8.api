using System.Diagnostics.CodeAnalysis;
using template.net8.api.Contracts;
using template.net8.api.Core.Interfaces;
using template.net8.api.Domain.DTOs;

namespace template.net8.api.Persistence.Models;

internal partial class User
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static implicit operator UserCreatedResource(User entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new UserCreatedResource
        {
            Username = entity.Username,
            Email = entity.Email,
            IsDisabled = entity.IsDisabled,
            RoleId = entity.RoleId,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Uuid = entity.Uuid
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static UserCreatedResource ToUserCreatedResource(
        User entity)
    {
        return entity;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static implicit operator UserResetedPasswordDto(User entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new UserResetedPasswordDto
        {
            Uuid = entity.Uuid,
            Email = entity.Email,
            Password = ""
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static UserResetedPasswordDto ToUserResetedPasswordDto(
        User entity)
    {
        return entity;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static implicit operator UserStateResource(User entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new UserStateResource
        {
            Username = entity.Username,
            IsDisabled = entity.IsDisabled,
            Uuid = entity.Uuid
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static UserStateResource ToUserStateResource(
        User entity)
    {
        return entity;
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class UserProjections
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static IProjection<User, UserIdTokenDto> UserIdTokenProjection =>
        new Projection<User, UserIdTokenDto>(static p => new UserIdTokenDto
        {
            Uuid = p.Uuid,
            Email = p.Email,
            FirstName = p.FirstName,
            LastName = p.LastName,
            RoleName = p.Role != null ? p.Role.Name : null,
            Username = p.Username
        });

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static IProjection<User, UserAccessTokenDto> UserAccessTokenProjection =>
        new Projection<User, UserAccessTokenDto>(static p => new UserAccessTokenDto
        {
            Uuid = p.Uuid,
            Email = p.Email,
            FirstName = p.FirstName,
            LastName = p.LastName,
            RoleName = p.Role != null ? p.Role.Name : null,
            Username = p.Username,
            RoleClaims = p.Role != null
                ? p.Role.Claims.Select(static c => new ClaimDto(c.Id, c.Name))
                : Enumerable.Empty<ClaimDto>(),
            UserClaims = p.Claims.Select(static c => new ClaimDto(c.Id, c.Name))
        });

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static IProjection<User, UserCredentialsDto> UserCredentialsProjection =>
        new Projection<User, UserCredentialsDto>(static p => new UserCredentialsDto
        {
            PasswordHash = p.PasswordHash,
            PasswordSalt = p.PasswordSalt
        });

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static IProjection<User, UserDto> UserProjection =>
        new Projection<User, UserDto>(static p => new UserDto
        {
            Uuid = p.Uuid,
            Email = p.Email,
            FirstName = p.FirstName,
            IsDisabled = p.IsDisabled,
            LastName = p.LastName,
            RoleId = p.RoleId,
            RoleName = p.Role != null ? p.Role.Name : null,
            Username = p.Username
        });
}