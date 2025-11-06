using System.Diagnostics.CodeAnalysis;
using Serilog;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Logger;

[CoreLibrary]
[SuppressMessage("ReSharper", "ClassTooBig",
    Justification =
        "MainLoggerMethods is designed to have all the Main Log Methods")]
internal static class MainLoggerMethods
{
    internal static void LogStartingMainService()
    {
        Log.Information(MainLoggerMessageDefinitions.StartingMainService, CoreConstants.ApiName);
    }

    internal static void LogBuilderStarting()
    {
        Log.Information(MainLoggerMessageDefinitions.BuilderStarting, CoreConstants.ApiName);
    }

    internal static void LogBuilderStarted()
    {
        Log.Information(MainLoggerMessageDefinitions.BuilderStarted, CoreConstants.ApiName);
    }

    internal static void LogInstallingServices()
    {
        Log.Information(MainLoggerMessageDefinitions.InstallingServices, CoreConstants.ApiName);
    }

    internal static void LogInstalledServices()
    {
        Log.Information(MainLoggerMessageDefinitions.InstalledServices, CoreConstants.ApiName);
    }

    internal static void LogContainerBuilding()
    {
        Log.Information(MainLoggerMessageDefinitions.ContainerBuilding, CoreConstants.ApiName);
    }

    internal static void LogContainerBuilded()
    {
        Log.Information(MainLoggerMessageDefinitions.ContainerBuilded, CoreConstants.ApiName);
    }

    internal static void LogStartingConfig()
    {
        Log.Information(MainLoggerMessageDefinitions.StartingConfig, CoreConstants.ApiName);
    }

    internal static void LogCompletedConfig()
    {
        Log.Information(MainLoggerMessageDefinitions.CompletedConfig, CoreConstants.ApiName);
    }

    internal static void LogRunningMainService()
    {
        Log.Information(MainLoggerMessageDefinitions.RunningMainService, CoreConstants.ApiName);
    }

    internal static void LogReadyMainService()
    {
        Log.Information(MainLoggerMessageDefinitions.ReadyMainService, CoreConstants.ApiName);
    }

    internal static void LogCriticalMainPipelineError(Exception ex)
    {
        Log.Fatal(ex, MainLoggerMessageDefinitions.CriticalMainPipelineError, CoreConstants.ApiName);
    }

    internal static void LogInitFallBack()
    {
        Log.Warning(MainLoggerMessageDefinitions.InitFallback, CoreConstants.ApiName);
    }

    internal static void LogShutdown()
    {
        Log.Information(MainLoggerMessageDefinitions.Shutdown, CoreConstants.ApiName);
    }

    internal static void LogMetricCollectorEnable()
    {
        Log.Information(MainLoggerMessageDefinitions.OpenTelemetryMetricCollectorEnable, CoreConstants.ApiName);
    }

    internal static void LogMetricCollectorDisable()
    {
        Log.Information(MainLoggerMessageDefinitions.OpenTelemetryMetricCollectorDisable, CoreConstants.ApiName);
    }

    internal static void LogTraceCollectorEnable()
    {
        Log.Information(MainLoggerMessageDefinitions.OpenTelemetryTraceCollectorEnable, CoreConstants.ApiName);
    }

    internal static void LogTraceCollectorDisable()
    {
        Log.Information(MainLoggerMessageDefinitions.OpenTelemetryTraceCollectorDisable, CoreConstants.ApiName);
    }

    internal static void LogCriticalUnhandledException(Exception ex)
    {
        Log.Fatal(ex, MainLoggerMessageDefinitions.CriticalUnhandledException, CoreConstants.ApiName);
    }

    internal static void LogUnobservedTaskException(Exception ex)
    {
        Log.Warning(ex, MainLoggerMessageDefinitions.UnobservedTaskException, CoreConstants.ApiName);
    }

    internal static void LogProcessExit()
    {
        Log.Information(MainLoggerMessageDefinitions.ProcessExit, CoreConstants.ApiName);
    }

    internal static void LogDomainUnload()
    {
        Log.Information(MainLoggerMessageDefinitions.DomainUnload, CoreConstants.ApiName);
    }
}