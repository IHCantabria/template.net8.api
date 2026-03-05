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
public sealed partial record QueryGetUserParamsResource : IPublicApiContract,
    IEqualityOperators<QueryGetUserParamsResource, QueryGetUserParamsResource, bool>
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
public sealed partial record QueryLoginUserParamsResource : IPublicApiContract,
    IEqualityOperators<QueryLoginUserParamsResource, QueryLoginUserParamsResource, bool>
{
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
    public required string Password { get; init; }
}