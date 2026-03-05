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
public sealed record QueryGetUserParamsDto : IDto,
    IEqualityOperators<QueryGetUserParamsDto, QueryGetUserParamsDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required Guid Key { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "Design",
    "CA1515:Consider making public types internal",
    Justification = "Public visibility is required as this type is part of the application request contract.")]
public sealed record QueryLoginUserParamsDto : IDto,
    IEqualityOperators<QueryLoginUserParamsDto, QueryLoginUserParamsDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string Password { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "Design",
    "CA1515:Consider making public types internal",
    Justification = "Public visibility is required as this type is part of the application request contract.")]
public sealed record QueryAccessUserParamsDto : IDto, IIdentity,
    IEqualityOperators<QueryAccessUserParamsDto, QueryAccessUserParamsDto, bool>
{
    /// <inheritdoc cref="IIdentity.Identity" />
    public required IdentityDto Identity { get; set; }
}