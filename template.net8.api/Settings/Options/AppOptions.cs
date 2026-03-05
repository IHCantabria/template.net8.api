using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;

namespace template.net8.api.Settings.Options;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed record AppOptions : IEqualityOperators<AppOptions, AppOptions, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string Env { get; set; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[OptionsValidator]
internal sealed partial class AppOptionsValidator : IValidateOptions<AppOptions>;