using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;

namespace template.net8.api.Settings.Options;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed record CorsOptions : IEqualityOperators<CorsOptions, CorsOptions, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public const string Cors = nameof(Cors);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string CorsPolicy { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? AllowedOrigins { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public IEnumerable<string> ArrayAllowedOrigins =>
        string.IsNullOrEmpty(AllowedOrigins) ? [] : AllowedOrigins.Split(';');
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[OptionsValidator]
internal sealed partial class CorsOptionsValidator : IValidateOptions<CorsOptions>;