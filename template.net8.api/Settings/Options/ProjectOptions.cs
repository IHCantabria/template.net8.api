using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Options;

/// <summary>
///     Project Options
/// </summary>
[CoreLibrary]
public sealed record ProjectOptions : IEqualityOperators<ProjectOptions, ProjectOptions, bool>
{
    /// <summary>
    ///     Version of the project
    /// </summary>
    [Required]
    public required string Version { get; init; }
}

/// <summary>
///     Project Options Validator
/// </summary>
[OptionsValidator]
[CoreLibrary]
public sealed partial class ProjectOptionsValidator : IValidateOptions<ProjectOptions>;