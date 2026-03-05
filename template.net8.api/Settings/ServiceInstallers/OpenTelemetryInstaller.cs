using System.Diagnostics;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Microsoft.IdentityModel.Protocols.Configuration;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using template.net8.api.Core;
using template.net8.api.Core.Logger;
using template.net8.api.Core.OpenTelemetry.Options;
using template.net8.api.Settings.Interfaces;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class OpenTelemetryInstaller : IServiceInstaller
{
    /// <inheritdoc cref="IServiceInstaller.LoadOrder" />
    public short LoadOrder => 4;

    /// <inheritdoc cref="IServiceInstaller.InstallServiceAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidConfigurationException">
    ///     The Open Telemetry configuration in the appsettings file is incorrect.
    ///     The Api configuration in the appsettings file is incorrect.
    /// </exception>
    /// <exception cref="InvalidOperationException">The name of this computer cannot be obtained.</exception>
    /// <exception cref="NotSupportedException">The process is not on this computer.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        // Configure strongly typed options objects
        var openTelemetryOptions = builder.Configuration
            .GetSection(OpenTelemetryOptions.OpenTelemetry)
            .Get<OpenTelemetryOptions>();

        if (openTelemetryOptions is null)
            return Task.CompletedTask;

        OptionsValidator.ValidateOpenTelemetryOptions(openTelemetryOptions);

        var apiOptions = builder.Configuration
            .GetSection(ApiOptions.Api)
            .Get<ApiOptions>();

        OptionsValidator.ValidateApiOptions(apiOptions);
        var version = builder.Configuration.Get<ProjectOptions>()?.Version ?? "";

        if (apiOptions is null)
            throw new InvalidConfigurationException(
                "The Api configuration in the appsettings file is incorrect.");

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(
                    openTelemetryOptions.ServiceName,
                    serviceVersion: version,
                    serviceInstanceId: CoreConstants.GuidInstance.ToString())
                .AddAttributes([
                    new KeyValuePair<string, object>("service.thread.id", Environment.CurrentManagedThreadId),
                    new KeyValuePair<string, object>("service.thread.name", Thread.CurrentThread.Name ?? string.Empty),
                    new KeyValuePair<string, object>("server.address", apiOptions.Address),
                    new KeyValuePair<string, object>("service.environment", builder.Environment.EnvironmentName),
                    new KeyValuePair<string, object>("host.name", Environment.MachineName),
                    new KeyValuePair<string, object>("host.ip", HostInfo.GetHostIp()),
                    new KeyValuePair<string, object>("os.description", RuntimeInformation.OSDescription),
                    new KeyValuePair<string, object>("os.architecture", RuntimeInformation.OSArchitecture.ToString()),
                    new KeyValuePair<string, object>("process.runtime.name", ".NET 8"),
                    new KeyValuePair<string, object>("process.runtime.version", Environment.Version.ToString()),
                    new KeyValuePair<string, object>("process.user.name", Environment.UserName),
                    new KeyValuePair<string, object>("process.pid", Environment.ProcessId),
                    new KeyValuePair<string, object>("process.name", Process.GetCurrentProcess().ProcessName)
                ]))
            .WithMetrics(metricsBuilder => ConfigureMetrics(metricsBuilder, openTelemetryOptions))
            .WithTracing(tracingBuilder =>
                ConfigureTracing(tracingBuilder, openTelemetryOptions, builder.Environment));

        return Task.CompletedTask;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureMetrics(MeterProviderBuilder builder, OpenTelemetryOptions options)
    {
        if (!options.IsMetricActive)
        {
            MainLoggerMethods.LogMetricCollectorDisable();
            return;
        }

        IsMetricOpenTelemetryAvailable(options);

        builder.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddRuntimeInstrumentation()
            .AddOtlpExporter(otlpOptions =>
            {
                if (options.MetricEndpointUrl != null) otlpOptions.Endpoint = options.MetricEndpointUrl;
                otlpOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
                if (options.UseMetricHeaderApiKey())
                    otlpOptions.Headers = $"{options.MetricEndpointApiKeyHeader}={options.MetricEndpointApiKeyValue}";
            });

        MainLoggerMethods.LogMetricCollectorEnable();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureTracing(TracerProviderBuilder builder, OpenTelemetryOptions options,
        IHostEnvironment environment)
    {
        if (!options.IsTraceActive)
        {
            MainLoggerMethods.LogTraceCollectorDisable();
            return;
        }

        IsTraceOpenTelemetryAvailable(options);

        ConfigureTraceSampler(builder, environment);
        ConfigureTraceExporter(builder, options);

        MainLoggerMethods.LogTraceCollectorEnable();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void IsMetricOpenTelemetryAvailable(OpenTelemetryOptions config)
    {
        if (!config.IsValidMetricUri())
            throw new InvalidConfigurationException(
                "The OpenTelemetry Metric configuration in the appsettings file is incorrect or the endpoint for the metrics is down. Missing MetricEndpointUrl value.");
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureTraceSampler(TracerProviderBuilder builder, IHostEnvironment environment)
    {
        if (environment.EnvironmentName is Envs.Development or Envs.Local or Envs.Test)
            builder.SetSampler<AlwaysOnSampler>();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureTraceExporter(TracerProviderBuilder builder, OpenTelemetryOptions options)
    {
        builder
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(otlpOptions => ConfigureOtlpExporter(otlpOptions, options));
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureOtlpExporter(OtlpExporterOptions otlpOptions, OpenTelemetryOptions options)
    {
        if (options.TraceEndpointUrl is not null)
            otlpOptions.Endpoint = options.TraceEndpointUrl;

        otlpOptions.Protocol = OtlpExportProtocol.HttpProtobuf;

        if (options.UseTraceHeaderApiKey())
            otlpOptions.Headers = $"{options.TraceEndpointApiKeyHeader}={options.TraceEndpointApiKeyValue}";
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void IsTraceOpenTelemetryAvailable(OpenTelemetryOptions config)
    {
        if (!config.IsValidTraceUri())
            throw new InvalidConfigurationException(
                "The OpenTelemetry Trace configuration in the appsettings file is incorrect or the endpoint for the traces is down. Missing TraceEndpointUrl value.");
    }
}