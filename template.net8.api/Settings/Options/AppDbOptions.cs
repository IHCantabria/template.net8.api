using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Web;
using Microsoft.Extensions.Options;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.Options;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed record AppDbOptions : IConnectionsOptions,
    IEqualityOperators<AppDbOptions, AppDbOptions, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public const string AppDb = $"{IConnectionsOptions.Connections}:{nameof(AppDb)}";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string ConnectionString { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? Schema { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public string DecodedConnectionString => HttpUtility.HtmlDecode(ConnectionString);
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[OptionsValidator]
internal sealed partial class AppDbOptionsValidator : IValidateOptions<AppDbOptions>;