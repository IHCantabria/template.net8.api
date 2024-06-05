using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Options;

/// <summary>
///     Swagger Options
/// </summary>
[CoreLibrary]
public sealed record SwaggerOptions
{
    /// <summary>
    ///     AppSettings Key
    /// </summary>
    public static readonly string Swagger = nameof(Swagger);

    /// <summary>
    ///     Json Route
    /// </summary>
    [Required]
    public required string JsonRoute { get; init; }

    /// <summary>
    ///     Short Description
    /// </summary>
    [Required]
    public required string ShortDescription { get; init; }

    /// <summary>
    ///     UI Endpoint
    /// </summary>
    [Required]
    public required string UiEndpoint { get; init; }

    /// <summary>
    ///     Document Title
    /// </summary>
    [Required]
    public required string DocumentTitle { get; init; }

    /// <summary>
    ///     Title
    /// </summary>
    [Required]
    public required string Title { get; init; }

    /// <summary>
    ///     Version Swagger
    /// </summary>
    [Required]
    public required string VersionSwagger { get; init; }

    /// <summary>
    ///     Long Description
    /// </summary>
    [Required]
    public required string LongDescription { get; init; }

    /// <summary>
    ///     License
    /// </summary>
    [Required]
    public required string License { get; init; }

    /// <summary>
    ///     Server Url
    /// </summary>
    [Required]
    public required Uri ServerUrl { get; init; }
}

/// <summary>
///     Swagger Options Validator
/// </summary>
[OptionsValidator]
[CoreLibrary]
public sealed partial class SwaggerOptionsValidator : IValidateOptions<SwaggerOptions>;