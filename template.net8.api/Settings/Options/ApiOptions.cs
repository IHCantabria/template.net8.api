using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Options;

/// <summary>
///     Api Options class to hold the Api Options
/// </summary>
[CoreLibrary]
public sealed record ApiOptions : IEqualityOperators<ApiOptions, ApiOptions, bool>
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