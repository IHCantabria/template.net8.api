using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.Options;

/// <summary>
///     Jwt Options for the authentication token generation
/// </summary>
[CoreLibrary]
public sealed record JwtOptions : ISecurityOptions, IEqualityOperators<JwtOptions, JwtOptions, bool>
{
    /// <summary>
    ///     AppSettings Key for Jwt
    /// </summary>
    public static readonly string Jwt = $"{ISecurityOptions.Security}:{nameof(Jwt)}";

    /// <summary>
    ///     Audience
    /// </summary>
    [Required]
    public required string Audience { get; init; } = null!;

    /// <summary>
    ///     Issuer
    /// </summary>
    [Required]
    public required string Issuer { get; init; } = null!;

    /// <summary>
    ///     Secret
    /// </summary>
    [Required]
    public required string Secret { get; init; } = null!;

    /// <summary>
    ///     Token Lifetime
    /// </summary>
    public required TimeSpan? TokenLifetime { get; set; }
}

/// <summary>
///     Jwt Options Validator
/// </summary>
[OptionsValidator]
[CoreLibrary]
public sealed partial class JwtOptionsValidator : IValidateOptions<JwtOptions>;