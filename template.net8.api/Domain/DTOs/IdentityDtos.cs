using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using template.net8.api.Core.Authorization;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Domain.DTOs;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "Design",
    "CA1515:Consider making public types internal",
    Justification = "Public visibility is required as this type is part of the application request contract.")]
public sealed record IdentityDto : IDto, IEqualityOperators<IdentityDto, IdentityDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SetsRequiredMembers]
    internal IdentityDto()
    {
        UserUuid = null;
        UserRoleName = null;
        UserIdentifier = null;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SetsRequiredMembers]
    internal IdentityDto(Guid? userUuid, string? userRoleName, string? userIdentifier)
    {
        UserUuid = userUuid;
        UserRoleName = userRoleName;
        UserIdentifier = userIdentifier;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required Guid? UserUuid { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? UserIdentifier { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? UserRoleName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private bool HasUserIdentifier()
    {
        return UserIdentifier is not null;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal bool IsGenie()
    {
        return HasUserIdentifier() && UserIdentifier == GenieIdentityConstants.Identifier;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal bool HasRole()
    {
        return UserRoleName is not null;
    }
}