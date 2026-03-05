using System.Diagnostics.CodeAnalysis;
using Serilog;
using template.net8.api.Business;

namespace template.net8.api.Core.Logger;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("ReSharper", "ClassTooBig",
    Justification = "The class intentionally centralizes all main logging methods in a single location.")]
internal static class MainLoggerMethods
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogStartingMainService()
    {
        Log.Information(MainLoggerMessageDefinitions.StartingMainService, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogBuilderStarting()
    {
        Log.Information(MainLoggerMessageDefinitions.BuilderStarting, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogBuilderStarted()
    {
        Log.Information(MainLoggerMessageDefinitions.BuilderStarted, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogInstallingServices()
    {
        Log.Information(MainLoggerMessageDefinitions.InstallingServices, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogInstalledServices()
    {
        Log.Information(MainLoggerMessageDefinitions.InstalledServices, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogContainerBuilding()
    {
        Log.Information(MainLoggerMessageDefinitions.ContainerBuilding, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogContainerBuilded()
    {
        Log.Information(MainLoggerMessageDefinitions.ContainerBuilded, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogStartingConfig()
    {
        Log.Information(MainLoggerMessageDefinitions.StartingConfig, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogCompletedConfig()
    {
        Log.Information(MainLoggerMessageDefinitions.CompletedConfig, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogRunningMainService()
    {
        Log.Information(MainLoggerMessageDefinitions.RunningMainService, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogReadyMainService()
    {
        Log.Information(MainLoggerMessageDefinitions.ReadyMainService, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogCriticalMainPipelineError(Exception ex)
    {
        Log.Fatal(ex, MainLoggerMessageDefinitions.CriticalMainPipelineError, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogInitFallBack()
    {
        Log.Warning(MainLoggerMessageDefinitions.InitFallback, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogShutdown()
    {
        Log.Information(MainLoggerMessageDefinitions.Shutdown, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogMetricCollectorEnable()
    {
        Log.Information(MainLoggerMessageDefinitions.OpenTelemetryMetricCollectorEnable, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogMetricCollectorDisable()
    {
        Log.Information(MainLoggerMessageDefinitions.OpenTelemetryMetricCollectorDisable, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogTraceCollectorEnable()
    {
        Log.Information(MainLoggerMessageDefinitions.OpenTelemetryTraceCollectorEnable, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogTraceCollectorDisable()
    {
        Log.Information(MainLoggerMessageDefinitions.OpenTelemetryTraceCollectorDisable, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogCriticalUnhandledException(Exception ex)
    {
        Log.Fatal(ex, MainLoggerMessageDefinitions.CriticalUnhandledException, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogUnobservedTaskException(Exception ex)
    {
        Log.Warning(ex, MainLoggerMessageDefinitions.UnobservedTaskException, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogProcessExit()
    {
        Log.Information(MainLoggerMessageDefinitions.ProcessExit, BusinessConstants.ApiName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void LogDomainUnload()
    {
        Log.Information(MainLoggerMessageDefinitions.DomainUnload, BusinessConstants.ApiName);
    }
}