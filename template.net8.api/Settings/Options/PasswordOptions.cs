using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.Options;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed record PasswordOptions : IEqualityOperators<PasswordOptions, PasswordOptions, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public const string Password = $"{ISecurityOptions.Security}:{nameof(Password)}";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string Pepper { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[OptionsValidator]
internal sealed partial class PasswordOptionsValidator : IValidateOptions<PasswordOptions>;