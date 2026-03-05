using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using template.net8.api.Core.Interfaces;
using template.net8.api.Domain.Interfaces;

namespace template.net8.api.Domain.DTOs;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "Design",
    "CA1515:Consider making public types internal",
    Justification = "Public visibility is required as this type is part of the application request contract.")]
public sealed record CommandCreateUserParamsDto : IDto, IIdentity,
    IEqualityOperators<CommandCreateUserParamsDto, CommandCreateUserParamsDto, bool>
{
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
    public required string Password { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string ConfirmPassword { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required short? RoleId { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? FirstName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? LastName { get; init; }

    /// <inheritdoc cref="IIdentity.Identity" />
    public required IdentityDto Identity { get; set; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "Design",
    "CA1515:Consider making public types internal",
    Justification = "Public visibility is required as this type is part of the application request contract.")]
public sealed record CommandResetUserPasswordParamsDto : IDto, IIdentity,
    IEqualityOperators<CommandResetUserPasswordParamsDto, CommandResetUserPasswordParamsDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required Guid Key { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string Password { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string ConfirmPassword { get; init; }

    /// <inheritdoc cref="IIdentity.Identity" />
    public required IdentityDto Identity { get; set; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "Design",
    "CA1515:Consider making public types internal",
    Justification = "Public visibility is required as this type is part of the application request contract.")]
public sealed record CommandUpdateUserParamsDto : IDto, IIdentity,
    IEqualityOperators<CommandUpdateUserParamsDto, CommandUpdateUserParamsDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required Guid Key { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? Username { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required bool? IsDisabled { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? Email { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required short? RoleId { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? FirstName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? LastName { get; init; }

    /// <inheritdoc cref="IIdentity.Identity" />
    public required IdentityDto Identity { get; set; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "Design",
    "CA1515:Consider making public types internal",
    Justification = "Public visibility is required as this type is part of the application request contract.")]
public sealed record CommandDisableUserParamsDto : IDto, IIdentity,
    IEqualityOperators<CommandDisableUserParamsDto, CommandDisableUserParamsDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required Guid Key { get; init; }

    /// <inheritdoc cref="IIdentity.Identity" />
    public required IdentityDto Identity { get; set; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "Design",
    "CA1515:Consider making public types internal",
    Justification = "Public visibility is required as this type is part of the application request contract.")]
public sealed record CommandEnableUserParamsDto : IDto, IIdentity,
    IEqualityOperators<CommandEnableUserParamsDto, CommandEnableUserParamsDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required Guid Key { get; init; }

    /// <inheritdoc cref="IIdentity.Identity" />
    public required IdentityDto Identity { get; set; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "Design",
    "CA1515:Consider making public types internal",
    Justification = "Public visibility is required as this type is part of the application request contract.")]
public sealed record CommandDeleteUserParamsDto : IDto, IIdentity,
    IEqualityOperators<CommandDeleteUserParamsDto, CommandDeleteUserParamsDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required Guid Key { get; init; }

    /// <inheritdoc cref="IIdentity.Identity" />
    public required IdentityDto Identity { get; set; }
}