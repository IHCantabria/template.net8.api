using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.InteropServices;
using LanguageExt;
using Microsoft.IdentityModel.Protocols.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.Destructurers;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Formatting.Compact;
using Serilog.Sinks.OpenTelemetry;
using template.net8.api.Business;
using template.net8.api.Core.Logger.Enrichers;
using template.net8.api.Core.OpenTelemetry.Options;
using template.net8.api.Settings.Options;
using Path = System.IO.Path;

namespace template.net8.api.Core.Logger.Extensions;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class LoggerConfigurationExtensions
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Given depth must be positive.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static LoggerConfiguration EnrichLog(this LoggerConfiguration lc)
    {
        var destructures = new List<IExceptionDestructurer> { new DbUpdateExceptionDestructurer() };
        destructures.AddRange(DestructuringOptionsBuilder.DefaultDestructurers);

        return lc.Enrich.FromLogContext()
            .Enrich.With<ActivityEnricher>()
            .Enrich.With<RequestIdentifierEnricher>()
            .Enrich.With<ThreadIdEnricher>()
            .Enrich.With<ThreadNameEnricher>()
            .Enrich.With<ClientIpEnricher>()
            .Enrich.With(new CorrelationIdEnricher("x-correlation-id", false))
            .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
                .WithDestructurers(destructures)
                .WithDestructuringDepth(1)
                .WithoutReflectionBasedDestructurer());
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static LoggerConfiguration ConfigureMinLevels(this LoggerConfiguration lc)
    {
        return lc.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Information)
            .MinimumLevel.Override("Program", LogEventLevel.Information)
            .MinimumLevel.Override("Npgsql", LogEventLevel.Information)
            .MinimumLevel.Override(BusinessConstants.ApiName, LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager",
                LogEventLevel.Error)
            .MinimumLevel.Override("Microsoft.AspNetCore.DataProtection.Repositories.EphemeralXmlRepository",
                LogEventLevel.Error);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="InvalidConfigurationException">Condition.</exception>
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
        if (apiOptionsConfig is null)
            throw new InvalidConfigurationException(
                "The Api configuration in the appsettings file is incorrect.");

        var config = new OpenTelemetryConfig
        {
            OpenTelemetryOptions = openTelemetryOptions,
            ApiOptions = apiOptionsConfig,
            EnvName = envName,
            Version = version
        };
        return lc.ConfigureSinkTelemetry(config);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static LoggerConfiguration ConfigureSinkTelemetry(this LoggerConfiguration lc, OpenTelemetryConfig config)
    {
        return lc.WriteTo.Async(s => s.OpenTelemetry(x =>
        {
            x.LogsEndpoint =
                config.OpenTelemetryOptions.LogEndpointUrl
                    .ToString(); //for loki use http://localhost:3100/otlp // for seq use http://localhost:5341/ingest/otlp/v1/logs
            x.Protocol = OtlpProtocol.HttpProtobuf;
            x.HttpMessageHandler = new SocketsHttpHandler { ActivityHeadersPropagator = null };
            if (config.OpenTelemetryOptions.UseLogHeaderApiKey() &&
                config.OpenTelemetryOptions.LogEndpointApiKeyHeader != null &&
                config.OpenTelemetryOptions.LogEndpointApiKeyValue != null)
                x.Headers = new Dictionary<string, string>
                {
                    [config.OpenTelemetryOptions.LogEndpointApiKeyHeader] =
                        config.OpenTelemetryOptions.LogEndpointApiKeyValue
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
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static LoggerConfiguration ConfigureSinkLocal(this LoggerConfiguration lc)
    {
        var logPath = Path.Combine(AppContext.BaseDirectory, "logs", "log.txt");
        return lc.WriteTo.Async(c => c.File(new CompactJsonFormatter(), logPath,
            rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, buffered: true));
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static Try<bool> IsLogOpenTelemetryAvailable(OpenTelemetryOptions config)
    {
        return () =>
        {
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, config.LogEndpointUrl);

            if (config.UseLogHeaderApiKey() && config.LogEndpointApiKeyHeader != null)
                request.Headers.Add(config.LogEndpointApiKeyHeader, config.LogEndpointApiKeyValue);

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
///     ADD DOCUMENTATION
/// </summary>
internal sealed record OpenTelemetryConfig : IEqualityOperators<OpenTelemetryConfig, OpenTelemetryConfig, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string EnvName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required string Version { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required OpenTelemetryOptions OpenTelemetryOptions { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public required ApiOptions ApiOptions { get; init; }
}