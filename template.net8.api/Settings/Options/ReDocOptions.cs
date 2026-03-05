using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;

namespace template.net8.api.Settings.Options;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed record ReDocOptions : IEqualityOperators<ReDocOptions, ReDocOptions, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public const string ReDoc = nameof(ReDoc);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string DocumentTitle { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required Uri SpecUrl { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string RoutePrefix { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[OptionsValidator]
internal sealed partial class ReDocOptionsValidator : IValidateOptions<ReDocOptions>;