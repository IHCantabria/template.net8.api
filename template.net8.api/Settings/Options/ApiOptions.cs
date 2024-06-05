using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using template.net8.Api.Core.Attributes;

namespace template.net8.Api.Settings.Options;

/// <summary>
///     Api Options class to hold the Api Options
/// </summary>
[CoreLibrary]
public sealed record ApiOptions
{
    /// <summary>
    ///     AppSettings key for the Api Options
    /// </summary>
    public static readonly string Api = nameof(Api);

    /// <summary>
    ///     Name of the Api
    /// </summary>
    [Required]
    public required string Name { get; init; }

    /// <summary>
    ///     Cors Policy
    /// </summary>
    [Required]
    public required string CorsPolicy { get; init; }
}

/// <summary>
///     Api Options Validator
/// </summary>
[OptionsValidator]
[CoreLibrary]
public sealed partial class ApiOptionsValidator : IValidateOptions<ApiOptions>;