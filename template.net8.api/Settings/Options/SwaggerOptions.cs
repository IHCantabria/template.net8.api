using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;

namespace template.net8.api.Settings.Options;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
public sealed record SwaggerOptions : IEqualityOperators<SwaggerOptions, SwaggerOptions, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public const string Swagger = nameof(Swagger);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string JsonRoute { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string ShortDescription { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string UiEndpoint { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string DocumentTitle { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string Title { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string VersionSwagger { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string LongDescription { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string License { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required Uri ServerUrl { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[OptionsValidator]
internal sealed partial class SwaggerOptionsValidator : IValidateOptions<SwaggerOptions>;