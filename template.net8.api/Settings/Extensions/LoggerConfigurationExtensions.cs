using System.Globalization;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Formatting.Compact;
using Serilog.Sinks.OpenTelemetry;
using template.net8.api.Business;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.Extensions;

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
            .MinimumLevel.Override(BusinessConstants.ApiName, LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning);
    }

    /// <summary>
    ///     Configure Log Sinks Extension
    /// </summary>
    /// <param name="lc"></param>
    /// <param name="builderConfiguration"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">When any argument is <code>null</code>.</exception>
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
        ConfigurationManager builderConfiguration)
    {
        var config = GetValidatedOpenTelemetryOptions(builderConfiguration);

        return lc.WriteTo.FallbackChain(wt => wt.OpenTelemetry(x =>
            {
                x.LogsEndpoint = config.LogEndpointUrl.ToString(); //seq, for loki use http://localhost:3100/otlp loki
                x.Protocol = OtlpProtocol.HttpProtobuf;
                if (config.UseHeaderApiKey())
                    x.Headers = new Dictionary<string, string>
                    {
                        [config.LogEndpointApiKeyHeader!] = config.LogEndpointApiKeyValue!
                    };
                x.ResourceAttributes = new Dictionary<string, object>
                {
                    ["service.name"] = BusinessConstants.ApiName
                };
                x.FormatProvider = CultureInfo.InvariantCulture;
            }),
            wt => wt.File(new CompactJsonFormatter(), config.LogFallbackFilePath,
                rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, buffered: true));
    }

    private static OpenTelemetryOptions GetValidatedOpenTelemetryOptions(ConfigurationManager builderConfiguration)
    {
        var config = builderConfiguration.GetSection(OpenTelemetryOptions.OpenTelemetry).Get<OpenTelemetryOptions>();
        if (config is null)
            throw new ArgumentException("The OpenTelemetry configuration in the appsettings file is missing.");

        var openTelemetryOptionsValidator = new OpenTelemetryOptionsValidator();
        var validation = openTelemetryOptionsValidator.Validate(null, config);
        if (validation.Failed)
            throw new ArgumentException(validation.FailureMessage);

        return config;
    }
}