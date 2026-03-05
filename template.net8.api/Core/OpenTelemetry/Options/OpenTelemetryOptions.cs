using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;

namespace template.net8.api.Core.OpenTelemetry.Options;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed record OpenTelemetryOptions : IEqualityOperators<OpenTelemetryOptions, OpenTelemetryOptions, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public const string OpenTelemetry = nameof(OpenTelemetry);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required bool IsLogActive { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required string ServiceName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? LogEndpointApiKeyHeader { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? LogEndpointApiKeyValue { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required Uri LogEndpointUrl { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? MetricEndpointApiKeyHeader { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? MetricEndpointApiKeyValue { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required bool IsMetricActive { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required Uri? MetricEndpointUrl { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? TraceEndpointApiKeyHeader { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string? TraceEndpointApiKeyValue { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Required]
    public required bool IsTraceActive { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required Uri? TraceEndpointUrl { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal bool UseLogHeaderApiKey()
    {
        if (string.IsNullOrEmpty(LogEndpointApiKeyHeader))
            return false;

        return !string.IsNullOrEmpty(LogEndpointApiKeyValue);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal bool UseMetricHeaderApiKey()
    {
        if (string.IsNullOrEmpty(MetricEndpointApiKeyHeader))
            return false;

        return !string.IsNullOrEmpty(MetricEndpointApiKeyValue);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal bool IsValidMetricUri()
    {
        return !string.IsNullOrEmpty(MetricEndpointUrl?.ToString());
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal bool UseTraceHeaderApiKey()
    {
        if (string.IsNullOrEmpty(TraceEndpointApiKeyHeader))
            return false;

        return !string.IsNullOrEmpty(TraceEndpointApiKeyValue);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal bool IsValidTraceUri()
    {
        return !string.IsNullOrEmpty(TraceEndpointUrl?.ToString());
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[OptionsValidator]
internal sealed partial class OpenTelemetryOptionsValidator : IValidateOptions<OpenTelemetryOptions>;