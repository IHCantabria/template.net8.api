using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.Options;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed record JwtOptions : ISecurityOptions, IEqualityOperators<JwtOptions, JwtOptions, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public const string Jwt = $"{ISecurityOptions.Security}:{nameof(Jwt)}";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string Audience { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string Issuer { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string Secret { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required TimeSpan? TokenLifetime { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[OptionsValidator]
internal sealed partial class JwtOptionsValidator : IValidateOptions<JwtOptions>;