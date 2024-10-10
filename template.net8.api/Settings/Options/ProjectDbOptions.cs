using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Web;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.Options;

/// <summary>
///     Project Database Options
/// </summary>
[CoreLibrary]
public sealed record ProjectDbOptions : IConnectionsOptions,
    IEqualityOperators<ProjectDbOptions, ProjectDbOptions, bool>
{
    /// <summary>
    ///     AppSettings Key for Project Database
    /// </summary>
    public static readonly string ProjectDb = $"{IConnectionsOptions.Connections}:{nameof(ProjectDb)}";

    /// <summary>
    ///     Connection String
    /// </summary>
    [Required]
    public required string ConnectionString { get; init; }

    /// <summary>
    ///     Schema
    /// </summary>
    public required string? Schema { get; init; }

    /// <summary>
    ///     Decoded Connection String
    /// </summary>
    /// <returns></returns>
    public string DecodedConnectionString => HttpUtility.HtmlDecode(ConnectionString);
}

/// <summary>
///     Project Database Options Validator
/// </summary>
[OptionsValidator]
[CoreLibrary]
public sealed partial class ProjectDbOptionsValidator : IValidateOptions<ProjectDbOptions>;