using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Logger.Options;

/// <summary>
///     OpenTelemetry Options class to hold the OpenTelemetry Options
/// </summary>
[CoreLibrary]
public sealed record OpenTelemetryOptions : IEqualityOperators<OpenTelemetryOptions, OpenTelemetryOptions, bool>
{
    /// <summary>
    ///     AppSettings key for the Open Telemetry Options
    /// </summary>
    public static readonly string OpenTelemetry = nameof(OpenTelemetry);

    /// <summary>
    ///     Log Endpoint Api Key Header
    /// </summary>
    public string? LogEndpointApiKeyHeader { get; set; }

    /// <summary>
    ///     Log Endpoint Api Key Value
    /// </summary>
    public string? LogEndpointApiKeyValue { get; set; }

    /// <summary>
    ///     Log Endpoint Url
    /// </summary>
    [Required]
    public required Uri LogEndpointUrl { get; set; }

    internal bool UseHeaderApiKey()
    {
        if (string.IsNullOrEmpty(LogEndpointApiKeyHeader))
            return false;

        return !string.IsNullOrEmpty(LogEndpointApiKeyValue);
    }
}

/// <summary>
///     OpenTelemetry Options Validator
/// </summary>
[OptionsValidator]
[CoreLibrary]
public sealed partial class OpenTelemetryOptionsValidator : IValidateOptions<OpenTelemetryOptions>;