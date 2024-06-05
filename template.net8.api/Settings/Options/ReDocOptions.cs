using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Options;

/// <summary>
///     ReDoc Options for the ReDoc Document Generator
/// </summary>
[CoreLibrary]
public sealed record ReDocOptions
{
    /// <summary>
    ///     AppSettings Key for ReDoc Options
    /// </summary>
    public static readonly string ReDoc = nameof(ReDoc);

    /// <summary>
    ///     Document Title for the ReDoc Document
    /// </summary>
    [Required]
    public required string DocumentTitle { get; init; }

    /// <summary>
    ///     Spec Url for the ReDoc Document
    /// </summary>
    [Required]
    public required Uri SpecUrl { get; init; }

    /// <summary>
    ///     Route Prefix for the ReDoc Document
    /// </summary>
    [Required]
    public required string RoutePrefix { get; init; }
}

/// <summary>
///     ReDoc Options Validator
/// </summary>
[OptionsValidator]
[CoreLibrary]
public sealed partial class ReDocOptionsValidator : IValidateOptions<ReDocOptions>;