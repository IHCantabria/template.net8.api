using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Options;

/// <summary>
///     Swagger Security Options
/// </summary>
[CoreLibrary]
public sealed record SwaggerSecurityOptions : IEqualityOperators<SwaggerSecurityOptions, SwaggerSecurityOptions, bool>
{
    /// <summary>
    ///     AppSettings Key
    /// </summary>
    public const string SwaggerSecurity = nameof(SwaggerSecurity);

    /// <summary>
    ///     Description
    /// </summary>
    [Required]
    public required string Description { get; init; } = null!;

    /// <summary>
    ///     Name
    /// </summary>
    [Required]
    public required string Name { get; init; } = null!;

    /// <summary>
    ///     Scheme Id
    /// </summary>
    [Required]
    public required string SchemeId { get; init; } = null!;

    /// <summary>
    ///     Scheme Name
    /// </summary>
    [Required]
    public required string SchemeName { get; init; } = null!;

    /// <summary>
    ///     Bearer Format
    /// </summary>
    [Required]
    public required string BearerFormat { get; init; } = null!;
}

/// <summary>
///     Swagger Security Options Validator
/// </summary>
[OptionsValidator]
[CoreLibrary]
public sealed partial class SwaggerSecurityOptionsValidator : IValidateOptions<SwaggerSecurityOptions>;