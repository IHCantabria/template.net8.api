using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;

namespace template.net8.api.Settings.Options;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed record ApiOptions : IEqualityOperators<ApiOptions, ApiOptions, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public const string Api = nameof(Api);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string Name { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string Address { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[OptionsValidator]
internal sealed partial class ApiOptionsValidator : IValidateOptions<ApiOptions>;