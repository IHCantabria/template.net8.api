using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Options;

/// <summary>
///     Cors Options class to hold the Cors Options
/// </summary>
[CoreLibrary]
public sealed record CorsOptions : IEqualityOperators<CorsOptions, CorsOptions, bool>
{
    /// <summary>
    ///     AppSettings key for the Cors Options
    /// </summary>
    public static readonly string Cors = nameof(Cors);

    /// <summary>
    ///     Cors Policy
    /// </summary>
    [Required]
    public required string CorsPolicy { get; init; }

    /// <summary>
    ///     Cors Allowed Origins
    /// </summary>
    [Required]
    public required string AllowedOrigins { get; init; }

    /// <summary>
    ///     Array of Allowed Origins
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> ArrayAllowedOrigins => AllowedOrigins.Split(";");
}

/// <summary>
///     Cors Options Validator
/// </summary>
[OptionsValidator]
[CoreLibrary]
public sealed partial class CorsOptionsValidator : IValidateOptions<CorsOptions>;