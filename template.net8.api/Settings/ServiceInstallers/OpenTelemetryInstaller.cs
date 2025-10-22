using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Protocols.Configuration;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using template.net8.api.Core;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Logger;
using template.net8.api.Core.OpenTelemetry.Options;
using template.net8.api.Settings.Interfaces;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Logger Service Installer
/// </summary>
[CoreLibrary]
public sealed class OpenTelemetryInstaller : IServiceInstaller
{
    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 4;

    /// <summary>
    ///     Install Logger Service
    /// </summary>
    /// <param name="builder"></param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">The name of this computer cannot be obtained.</exception>
    /// <exception cref="InvalidConfigurationException">Condition.</exception>
    /// <exception cref="NotSupportedException">The process is not on this computer.</exception>
    /// <exception cref="SocketException">An error is encountered when resolving the local host name.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     The length of
    ///     <paramref>
    ///         <name>hostNameOrAddress</name>
    ///     </paramref>
    ///     is greater than 255 characters.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <paramref>
    ///         <name>hostNameOrAddress</name>
    ///     </paramref>
    ///     is an invalid IP address.
    /// </exception>
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

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(
                    CoreConstants.ApiName,
                    serviceVersion: version,
                    serviceInstanceId: CoreConstants.GuidInstance.ToString())
                .AddAttributes([
                    new KeyValuePair<string, object>("service.thread.id", Environment.CurrentManagedThreadId),
                    new KeyValuePair<string, object>("service.thread.name", Thread.CurrentThread.Name ?? string.Empty),
                    new KeyValuePair<string, object>("server.address", apiOptions!.Address),
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
                otlpOptions.Endpoint = options.MetricEndpointUrl!;
                otlpOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
                if (options.UseMetricHeaderApiKey())
                    otlpOptions.Headers = $"{options.MetricEndpointApiKeyHeader}={options.MetricEndpointApiKeyValue}";
            });

        MainLoggerMethods.LogMetricCollectorEnable();
    }

    private static void ConfigureTracing(TracerProviderBuilder builder, OpenTelemetryOptions options,
        IHostEnvironment environment)
    {
        if (!options.IsTraceActive)
        {
            MainLoggerMethods.LogTraceCollectorDisable();
            return;
        }

        IsTraceOpenTelemetryAvailable(options);

        if (environment.EnvironmentName is Envs.Development or Envs.Local or Envs.Test)
            builder.SetSampler<AlwaysOnSampler>();

        builder.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddOtlpExporter(otlpOptions =>
        {
            otlpOptions.Endpoint = options.TraceEndpointUrl!;
            otlpOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
            if (options.UseTraceHeaderApiKey())
                otlpOptions.Headers = $"{options.TraceEndpointApiKeyHeader}={options.TraceEndpointApiKeyValue}";
        });

        MainLoggerMethods.LogTraceCollectorEnable();
    }

    private static void IsMetricOpenTelemetryAvailable(OpenTelemetryOptions config)
    {
        if (!config.IsValidMetricUri())
            throw new InvalidConfigurationException(
                "The OpenTelemetry Metric configuration in the appsettings file is incorrect or the endpoint for the metrics is down. Missing MetricEndpointUrl value.");
    }

    private static void IsTraceOpenTelemetryAvailable(OpenTelemetryOptions config)
    {
        if (!config.IsValidTraceUri())
            throw new InvalidConfigurationException(
                "The OpenTelemetry Trace configuration in the appsettings file is incorrect or the endpoint for the traces is down. Missing TraceEndpointUrl value.");
    }
}