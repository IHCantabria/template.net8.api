using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.OpenTelemetry.Options;

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
    ///     Log Active
    /// </summary>
    [Required]
    public required bool IsLogActive { get; set; }

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

    /// <summary>
    ///     Metric Endpoint Api Key Header
    /// </summary>
    public string? MetricEndpointApiKeyHeader { get; set; }

    /// <summary>
    ///     Metric Endpoint Api Key Value
    /// </summary>
    public string? MetricEndpointApiKeyValue { get; set; }

    /// <summary>
    ///     Metric Active
    /// </summary>
    [Required]
    public required bool IsMetricActive { get; set; }

    /// <summary>
    ///     Metric Endpoint Url
    /// </summary>
    public Uri? MetricEndpointUrl { get; set; }

    /// <summary>
    ///     Trace Endpoint Api Key Header
    /// </summary>
    public string? TraceEndpointApiKeyHeader { get; set; }

    /// <summary>
    ///     Trace Endpoint Api Key Value
    /// </summary>
    public string? TraceEndpointApiKeyValue { get; set; }

    /// <summary>
    ///     Trace Active
    /// </summary>
    [Required]
    public required bool IsTraceActive { get; set; }

    /// <summary>
    ///     Trace Endpoint Url
    /// </summary>
    public Uri? TraceEndpointUrl { get; set; }

    internal bool UseLogHeaderApiKey()
    {
        if (string.IsNullOrEmpty(LogEndpointApiKeyHeader))
            return false;

        return !string.IsNullOrEmpty(LogEndpointApiKeyValue);
    }

    internal bool UseMetricHeaderApiKey()
    {
        if (string.IsNullOrEmpty(MetricEndpointApiKeyHeader))
            return false;

        return !string.IsNullOrEmpty(MetricEndpointApiKeyValue);
    }

    internal bool IsValidMetricUri()
    {
        return !string.IsNullOrEmpty(MetricEndpointUrl?.ToString());
    }

    internal bool UseTraceHeaderApiKey()
    {
        if (string.IsNullOrEmpty(TraceEndpointApiKeyHeader))
            return false;

        return !string.IsNullOrEmpty(TraceEndpointApiKeyValue);
    }

    internal bool IsValidTraceUri()
    {
        return !string.IsNullOrEmpty(TraceEndpointUrl?.ToString());
    }
}

/// <summary>
///     OpenTelemetry Options Validator
/// </summary>
[OptionsValidator]
[CoreLibrary]
public sealed partial class OpenTelemetryOptionsValidator : IValidateOptions<OpenTelemetryOptions>;