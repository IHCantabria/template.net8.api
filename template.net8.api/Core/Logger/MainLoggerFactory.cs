using Microsoft.IdentityModel.Protocols.Configuration;
using Serilog;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Logger.Extensions;
using template.net8.api.Core.Logger.Sinks;

namespace template.net8.api.Core.Logger;

[CoreLibrary]
internal static class SerilogLoggersFactory
{
    private static readonly MemorySink MemorySink = new();

    /// <summary>
    ///     Main Log Factory
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     When
    ///     <paramref>
    ///         <name>value</name>
    ///     </paramref>
    ///     is <code>null</code>
    /// </exception>
    internal static void MainLogFactory()
    {
        Log.Logger = new LoggerConfiguration()
            .EnrichLog()
            .ConfigureMinLevels()
            .WriteTo.Sink(MemorySink) // Temporal memory sink
            .CreateBootstrapLogger();
    }

    /// <summary>
    ///     Real Application Log Factory
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     When
    ///     <paramref>
    ///         <name>value</name>
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
    /// <exception cref="InvalidConfigurationException">
    ///     The OpenTelemetry configuration in the appsettings file is incorrect.
    ///     There was a problem trying to connecte to the OpenTelemetry endpoint
    /// </exception>
    internal static void RealApplicationLogFactory(ConfigurationManager builderConfiguration,
        string envName, string version)
    {
        Log.Logger = new LoggerConfiguration()
            .EnrichLog()
            .ConfigureMinLevels()
            .ReadFrom.Configuration(builderConfiguration)
            .ConfigureSinks(builderConfiguration, envName, version)
            .CreateLogger();

        MemorySink.FlushToLogger(Log.Logger);
    }

    /// <summary>
    ///     Fallback Log Factory
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     When
    ///     <paramref>
    ///         <name>value</name>
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
    internal static void FallbackLogFactory()
    {
        Log.Logger = new LoggerConfiguration()
            .EnrichLog()
            .ConfigureMinLevels()
            .ConfigureSinkLocal()
            .CreateLogger();

        MemorySink.FlushToLogger(Log.Logger);
        MainLoggerMethods.LogInitFallBack();
    }
}