using System.Globalization;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Protocols.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Formatting.Compact;
using Serilog.Sinks.OpenTelemetry;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Logger.Enrichers;
using template.net8.api.Core.Logger.Options;

namespace template.net8.api.Core.Logger.Extensions;

[CoreLibrary]
internal static class LoggerConfigurationExtensions
{
    /// <summary>
    ///     Enrich Log Extension
    /// </summary>
    /// <param name="lc"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    internal static LoggerConfiguration EnrichLog(this LoggerConfiguration lc)
    {
        return lc.Enrich.FromLogContext()
            .Enrich.With<ActivityEnricher>()
            .Enrich.With<RequestIdentifierEnricher>()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .Enrich.WithClientIp()
            .Enrich.WithCorrelationId()
            .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
                .WithDefaultDestructurers()
                .WithDestructurers([new DbUpdateExceptionDestructurer()]));
    }

    /// <summary>
    ///     Configure Min Levels Log Extension
    /// </summary>
    /// <param name="lc"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    internal static LoggerConfiguration ConfigureMinLevels(this LoggerConfiguration lc)
    {
        return lc.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Information)
            .MinimumLevel.Override("Program", LogEventLevel.Information)
            .MinimumLevel.Override("Npgsql", LogEventLevel.Information)
            .MinimumLevel.Override(CoreConstants.ApiName, LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning);
    }

    /// <summary>
    ///     Configure Log Sinks Extension
    /// </summary>
    /// <param name="lc"></param>
    /// <param name="builderConfiguration"></param>
    /// <param name="envName"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    /// <exception cref="InvalidConfigurationException">
    ///     The OpenTelemetry configuration in the appsettings file is incorrect.
    ///     There was a problem trying to connecte to the OpenTelemetry endpoint
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     When
    ///     <paramref>
    ///         <name>sinkConfiguration</name>
    ///     </paramref>
    ///     is <code>null</code>
    /// </exception>
    /// <exception cref="IOException">Condition.</exception>
    /// <exception cref="InvalidOperationException">Condition.</exception>
    /// <exception cref="NotSupportedException">Condition.</exception>
    /// <exception cref="PathTooLongException">
    ///     When
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    ///     is too long
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    ///     The caller does not have the required permission to access the
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    /// </exception>
    internal static LoggerConfiguration ConfigureSinks(this LoggerConfiguration lc,
        ConfigurationManager builderConfiguration, string envName, string version)
    {
        var config = builderConfiguration.GetSection(OpenTelemetryOptions.OpenTelemetry).Get<OpenTelemetryOptions>();

        if (config is null)
            return lc.ConfigureSinkLocal();

        ValidateOpenTelemetryOptions(config);
        var useOpenTelemetry = IsOpenTelemetryAvailable(config);
        if (!useOpenTelemetry)
            throw new InvalidConfigurationException(
                "The OpenTelemetry configuration in the appsettings file is incorrect or the endpoint is down. There was a problem trying to connecte to the OpenTelemetry endpoint");

        return lc.ConfigureSinkTelemetry(config, envName, version);
    }

    private static LoggerConfiguration ConfigureSinkTelemetry(this LoggerConfiguration lc, OpenTelemetryOptions config,
        string envName, string version)
    {
        return lc.WriteTo.OpenTelemetry(x =>
        {
            x.LogsEndpoint =
                config.LogEndpointUrl
                    .ToString(); //for loki use http://localhost:3100/otlp // for seq use http://localhost:5341/ingest/otlp/v1/logs
            x.Protocol = OtlpProtocol.HttpProtobuf;
            x.HttpMessageHandler = new SocketsHttpHandler { ActivityHeadersPropagator = null };
            if (config.UseHeaderApiKey())
                x.Headers = new Dictionary<string, string>
                {
                    [config.LogEndpointApiKeyHeader!] = config.LogEndpointApiKeyValue!
                };
            x.ResourceAttributes = new Dictionary<string, object>
            {
                ["service.name"] = CoreConstants.ApiName,
                ["service.version"] = version,
                ["service.environment"] = envName
            };
            x.FormatProvider = CultureInfo.InvariantCulture;
        });
    }

    /// <summary>
    ///     Configure Log Sink Local
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     When
    ///     <paramref>
    ///         <name>sinkConfiguration</name>
    ///     </paramref>
    ///     is <code>null</code>
    /// </exception>
    /// <exception cref="IOException">Condition.</exception>
    /// <exception cref="InvalidOperationException">Condition.</exception>
    /// <exception cref="NotSupportedException">Condition.</exception>
    /// <exception cref="PathTooLongException">
    ///     When
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    ///     is too long
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    ///     The caller does not have the required permission to access the
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    /// </exception>
    internal static LoggerConfiguration ConfigureSinkLocal(this LoggerConfiguration lc)
    {
        return lc.WriteTo.Async(c => c.File(new CompactJsonFormatter(), "logs/log.txt",
            rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, buffered: true));
    }

    private static void ValidateOpenTelemetryOptions(OpenTelemetryOptions? config)
    {
        var optionsValidator = new OpenTelemetryOptionsValidator();
        if (config is null)
            throw new InvalidConfigurationException(
                "The OpenTelemetry configuration in the appsettings file is incorrect");

        var validation = optionsValidator.Validate(null, config);
        if (validation.Failed)
            throw new InvalidConfigurationException(validation.FailureMessage);
    }

    private static bool IsOpenTelemetryAvailable(OpenTelemetryOptions config)
    {
        using var client = new HttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Post, config.LogEndpointUrl);

        if (config.UseHeaderApiKey())
            request.Headers.Add(config.LogEndpointApiKeyHeader!, config.LogEndpointApiKeyValue);

        // Send a minimal valid OpenTelemetry log entry
        request.Content = new ByteArrayContent([]);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-protobuf");

        // Check if client responds
        using var response = client.Send(request, HttpCompletionOption.ResponseHeadersRead);
        return response.IsSuccessStatusCode;
    }
}