using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Contracts;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed partial record CommandCreateUserParamsResource : IPublicApiContract,
    IEqualityOperators<CommandCreateUserParamsResource, CommandCreateUserParamsResource, bool>
{
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
    public required string Password { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string ConfirmPassword { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public string? FirstName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public string? LastName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required short RoleId { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed partial record CommandUpdateUserParamsResource : IPublicApiContract,
    IEqualityOperators<CommandUpdateUserParamsResource, CommandUpdateUserParamsResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    [FromRoute(Name = "user-key")]
    public required Guid Key { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    [FromBody]
    public required CommandUpdateUserParamsBodyResource Body { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed record CommandUpdateUserParamsBodyResource : IPublicApiContract,
    IEqualityOperators<CommandUpdateUserParamsBodyResource, CommandUpdateUserParamsBodyResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public string? Username { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public bool IsDisabled { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public short? RoleId { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public string? FirstName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public string? LastName { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed partial record CommandResetUserPasswordParamsResource : IPublicApiContract,
    IEqualityOperators<CommandResetUserPasswordParamsResource, CommandResetUserPasswordParamsResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    [FromRoute(Name = "user-key")]
    public required Guid Key { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    [FromBody]
    public required CommandResetUserPasswordParamsBodyResource Body { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed record CommandResetUserPasswordParamsBodyResource : IPublicApiContract,
    IEqualityOperators<CommandResetUserPasswordParamsBodyResource, CommandResetUserPasswordParamsBodyResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string Password { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string ConfirmPassword { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed partial record CommandDisableUserParamsResource : IPublicApiContract,
    IEqualityOperators<CommandDisableUserParamsResource, CommandDisableUserParamsResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    [FromRoute(Name = "user-key")]
    public required Guid Key { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed partial record CommandEnableUserParamsResource : IPublicApiContract,
    IEqualityOperators<CommandEnableUserParamsResource, CommandEnableUserParamsResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    [FromRoute(Name = "user-key")]
    public required Guid Key { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Contracts must remain public to allow proper discovery and schema generation by OpenAPI.")]
public sealed partial record CommandDeleteUserParamsResource : IPublicApiContract,
    IEqualityOperators<CommandDeleteUserParamsResource, CommandDeleteUserParamsResource, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    [FromRoute(Name = "user-key")]
    public required Guid Key { get; init; }
}