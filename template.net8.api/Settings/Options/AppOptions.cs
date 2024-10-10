using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Options;

/// <summary>
///     App Options class to hold the App Options
/// </summary>
[CoreLibrary]
public sealed record AppOptions : IEqualityOperators<AppOptions, AppOptions, bool>
{
    /// <summary>
    ///     Current Environment
    /// </summary>
    [Required]
    public required string Env { get; set; }
}

/// <summary>
///     App Options Validator
/// </summary>
[OptionsValidator]
[CoreLibrary]
public sealed partial class AppOptionsValidator : IValidateOptions<AppOptions>;