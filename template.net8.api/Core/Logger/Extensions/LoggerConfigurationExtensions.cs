using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using LanguageExt;
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
using template.net8.api.Core.OpenTelemetry.Options;
using template.net8.api.Settings.Options;
using Path = System.IO.Path;

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
    /// <exception cref="ArgumentException">
    ///     When any element of
    ///     <paramref>
    ///         <name>enrichers</name>
    ///     </paramref>
    ///     is <code>null</code>
    /// </exception>
    internal static LoggerConfiguration EnrichLog(this LoggerConfiguration lc)
    {
        return lc.Enrich.FromLogContext()
            .Enrich.With<ActivityEnricher>()
            .Enrich.With<RequestIdentifierEnricher>()
            .Enrich.With<ThreadIdEnricher>()
            .Enrich.With<ThreadNameEnricher>()
            .Enrich.With<ClientIpEnricher>()
            .Enrich.With(new CorrelationIdEnricher("x-correlation-id", false))
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
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager",
                LogEventLevel.Error)
            .MinimumLevel.Override("Microsoft.AspNetCore.DataProtection.Repositories.EphemeralXmlRepository",
                LogEventLevel.Error);
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
        var openTelemetryOptions = builderConfiguration.GetSection(OpenTelemetryOptions.OpenTelemetry)
            .Get<OpenTelemetryOptions>();

        if (openTelemetryOptions is null || !openTelemetryOptions.IsLogActive)
            return lc.ConfigureSinkLocal();

        OptionsValidator.ValidateOpenTelemetryOptions(openTelemetryOptions);
        var useOpenTelemetry = IsLogOpenTelemetryAvailable(openTelemetryOptions).Try();
        if (useOpenTelemetry.IsFaulted)
            throw new InvalidConfigurationException(
                "The OpenTelemetry Log configuration in the appsettings file is incorrect or the endpoint for the logs is down. There was a problem trying to connecte to the OpenTelemetry log endpoint");

        var apiOptionsConfig = builderConfiguration.GetSection(ApiOptions.Api).Get<ApiOptions>();
        OptionsValidator.ValidateApiOptions(apiOptionsConfig);

        var config = new OpenTelemetryConfig
        {
            OpenTelemetryOptions = openTelemetryOptions,
            ApiOptions = apiOptionsConfig!,
            EnvName = envName,
            Version = version
        };
        return lc.ConfigureSinkTelemetry(config);
    }

    private static LoggerConfiguration ConfigureSinkTelemetry(this LoggerConfiguration lc, OpenTelemetryConfig config)
    {
        return lc.WriteTo.Async(s => s.OpenTelemetry(x =>
        {
            x.LogsEndpoint =
                config.OpenTelemetryOptions.LogEndpointUrl
                    .ToString(); //for loki use http://localhost:3100/otlp // for seq use http://localhost:5341/ingest/otlp/v1/logs
            x.Protocol = OtlpProtocol.HttpProtobuf;
            x.HttpMessageHandler = new SocketsHttpHandler { ActivityHeadersPropagator = null };
            if (config.OpenTelemetryOptions.UseLogHeaderApiKey())
                x.Headers = new Dictionary<string, string>
                {
                    [config.OpenTelemetryOptions.LogEndpointApiKeyHeader!] =
                        config.OpenTelemetryOptions.LogEndpointApiKeyValue!
                };
            x.ResourceAttributes = new Dictionary<string, object>
            {
                ["service.name"] = config.OpenTelemetryOptions.ServiceName,
                ["service.version"] = config.Version,
                ["service.instance.id"] = CoreConstants.GuidInstance.ToString(),
                ["service.thread.id"] = Environment.CurrentManagedThreadId,
                ["service.thread.name"] = Thread.CurrentThread.Name ?? string.Empty,
                ["server.address"] = config.ApiOptions.Address,
                ["service.environment"] = config.EnvName,
                ["host.name"] = Environment.MachineName,
                ["host.ip"] = HostInfo.GetHostIp(),
                ["os.description"] = RuntimeInformation.OSDescription,
                ["os.architecture"] = RuntimeInformation.OSArchitecture.ToString(),
                ["process.runtime.name"] = ".NET 8",
                ["process.runtime.version"] = Environment.Version.ToString(),
                ["process.user.name"] = Environment.UserName,
                ["process.pid"] = Environment.ProcessId,
                ["process.name"] = Process.GetCurrentProcess().ProcessName
            };
            x.FormatProvider = CultureInfo.InvariantCulture;
        }));
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
        var logPath = Path.Combine(AppContext.BaseDirectory, "logs", "log.txt");
        return lc.WriteTo.Async(c => c.File(new CompactJsonFormatter(), logPath,
            rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, buffered: true));
    }

    private static Try<bool> IsLogOpenTelemetryAvailable(OpenTelemetryOptions config)
    {
        return () =>
        {
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, config.LogEndpointUrl);

            if (config.UseLogHeaderApiKey())
                request.Headers.Add(config.LogEndpointApiKeyHeader!, config.LogEndpointApiKeyValue);

            // Send a minimal valid OpenTelemetry log entry
            request.Content = new ByteArrayContent([]);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-protobuf");

            // Check if client responds
            using var response = client.Send(request, HttpCompletionOption.ResponseHeadersRead);
            return response.IsSuccessStatusCode;
        };
    }
}

/// <summary>
///     OpenTelemetry Config
/// </summary>
[CoreLibrary]
public sealed record OpenTelemetryConfig
{
    /// <summary>
    ///     Environment Name
    /// </summary>
    public required string EnvName { get; init; } = null!;

    /// <summary>
    ///     version
    /// </summary>
    public required string Version { get; init; } = null!;

    /// <summary>
    ///     OpenTelemetry Options
    /// </summary>
    public required OpenTelemetryOptions OpenTelemetryOptions { get; init; } = null!;

    /// <summary>
    ///     Api Options
    /// </summary>
    public required ApiOptions ApiOptions { get; init; } = null!;
}