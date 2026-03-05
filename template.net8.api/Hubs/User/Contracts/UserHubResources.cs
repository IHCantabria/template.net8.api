using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Hubs.User.Contracts;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed record UserHubCreatedUserMessageResource : IPublicApiContract,
    IEqualityOperators<UserHubCreatedUserMessageResource, UserHubCreatedUserMessageResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string Message { get; init; } = string.Empty;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string Uuid { get; init; } = string.Empty;
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed record UserHubUpdatedUserMessageResource : IPublicApiContract,
    IEqualityOperators<UserHubUpdatedUserMessageResource, UserHubUpdatedUserMessageResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string Message { get; init; } = string.Empty;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string Uuid { get; init; } = string.Empty;
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed record UserHubDeletedUserMessageResource : IPublicApiContract,
    IEqualityOperators<UserHubDeletedUserMessageResource, UserHubDeletedUserMessageResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string Message { get; init; } = string.Empty;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string Uuid { get; init; } = string.Empty;
}