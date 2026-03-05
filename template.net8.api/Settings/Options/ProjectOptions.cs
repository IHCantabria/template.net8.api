using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;

namespace template.net8.api.Settings.Options;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed record ProjectOptions : IEqualityOperators<ProjectOptions, ProjectOptions, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string Version { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[OptionsValidator]
internal sealed partial class ProjectOptionsValidator : IValidateOptions<ProjectOptions>;