using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.Options;

/// <summary>
///     Password Options for the user password hashing
/// </summary>
[CoreLibrary]
public sealed record PasswordOptions : IEqualityOperators<PasswordOptions, PasswordOptions, bool>
{
    /// <summary>
    ///     AppSettings Key for Password
    /// </summary>
    public static readonly string Password = $"{ISecurityOptions.Security}:{nameof(Password)}";

    /// <summary>
    ///     Pepper
    /// </summary>
    [Required]
    public required string Pepper { get; init; } = null!;
}

/// <summary>
///     Password Options Validator
/// </summary>
[OptionsValidator]
[CoreLibrary]
public sealed partial class PasswordOptionsValidator : IValidateOptions<PasswordOptions>;