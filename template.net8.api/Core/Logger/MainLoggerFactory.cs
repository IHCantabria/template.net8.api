using System.Diagnostics.CodeAnalysis;
using Microsoft.IdentityModel.Protocols.Configuration;
using Serilog;
using template.net8.api.Core.Logger.Extensions;
using template.net8.api.Core.Logger.Sinks;

namespace template.net8.api.Core.Logger;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class SerilogLoggersFactory
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static readonly MemorySink MemorySink = new();

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Given depth must be positive.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static void MainLogFactory()
    {
        Log.Logger = new LoggerConfiguration()
            .EnrichLog()
            .ConfigureMinLevels()
            .WriteTo.Async(static a => a.Sink(MemorySink)) // Temporal memory sink
            .CreateBootstrapLogger();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Given depth must be positive.</exception>
    /// <exception cref="InvalidConfigurationException">Condition.</exception>
    /// <exception cref="InvalidOperationException">When the logger is already created</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
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
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="InvalidOperationException">When the logger is already created</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
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