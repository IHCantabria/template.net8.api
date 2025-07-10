using Serilog;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Logger;

[CoreLibrary]
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

    internal static void LogCriticalError(Exception ex)
    {
        Log.Fatal(ex, MainLoggerMessageDefinitions.CriticalError, CoreConstants.ApiName);
    }

    internal static void LogInitFallBack()
    {
        Log.Warning(MainLoggerMessageDefinitions.InitFallback, CoreConstants.ApiName);
    }

    internal static void LogShutdown()
    {
        Log.Information(MainLoggerMessageDefinitions.Shutdown, CoreConstants.ApiName);
    }
}