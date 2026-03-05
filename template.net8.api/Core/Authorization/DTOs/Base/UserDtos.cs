using System.Numerics;
using JetBrains.Annotations;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Core.Authorization.DTOs.Base;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal abstract record UserIdTokenBaseDto : IDto, IEqualityOperators<UserIdTokenBaseDto, UserIdTokenBaseDto, bool>
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
    internal required string? FirstName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string? LastName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string Email { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string? RoleName { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal abstract record UserAccessTokenBaseDto : IDto,
    IEqualityOperators<UserAccessTokenBaseDto, UserAccessTokenBaseDto, bool>
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
    internal required string? FirstName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string? LastName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string Email { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string? RoleName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal IEnumerable<ClaimBaseDto> RoleClaims { get; } = [];

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal IEnumerable<ClaimBaseDto> UserClaims { get; } = [];
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal abstract record ClaimBaseDto([UsedImplicitly] short Id, [UsedImplicitly] string Name)
    : IDto, IEqualityOperators<ClaimBaseDto, ClaimBaseDto, bool>;