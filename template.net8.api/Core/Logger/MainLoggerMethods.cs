using Serilog;
using template.net8.api.Business;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Logger;

[CoreLibrary]
internal static class MainLoggerMethods
{
    internal static void LogStartingMainService()
    {
        Log.Information(MainLoggerMessageDefinitions.StartingMainService, BusinessConstants.ApiName);
    }

    internal static void LogBuilderStarting()
    {
        Log.Information(MainLoggerMessageDefinitions.BuilderStarting, BusinessConstants.ApiName);
    }

    internal static void LogBuilderStarted()
    {
        Log.Information(MainLoggerMessageDefinitions.BuilderStarted, BusinessConstants.ApiName);
    }

    internal static void LogInstallingServices()
    {
        Log.Information(MainLoggerMessageDefinitions.InstallingServices, BusinessConstants.ApiName);
    }

    internal static void LogInstalledServices()
    {
        Log.Information(MainLoggerMessageDefinitions.InstalledServices, BusinessConstants.ApiName);
    }

    internal static void LogContainerBuilding()
    {
        Log.Information(MainLoggerMessageDefinitions.ContainerBuilding, BusinessConstants.ApiName);
    }

    internal static void LogContainerBuilded()
    {
        Log.Information(MainLoggerMessageDefinitions.ContainerBuilded, BusinessConstants.ApiName);
    }

    internal static void LogStartingConfig()
    {
        Log.Information(MainLoggerMessageDefinitions.StartingConfig, BusinessConstants.ApiName);
    }

    internal static void LogCompletedConfig()
    {
        Log.Information(MainLoggerMessageDefinitions.CompletedConfig, BusinessConstants.ApiName);
    }

    internal static void LogRunningMainService()
    {
        Log.Information(MainLoggerMessageDefinitions.RunningMainService, BusinessConstants.ApiName);
    }

    internal static void LogReadyMainService()
    {
        Log.Information(MainLoggerMessageDefinitions.ReadyMainService, BusinessConstants.ApiName);
    }

    internal static void LogCriticalError(Exception ex)
    {
        Log.Fatal(ex, MainLoggerMessageDefinitions.CriticalError, BusinessConstants.ApiName);
    }

    internal static void LogInitFallBack()
    {
        Log.Warning(MainLoggerMessageDefinitions.InitFallback, BusinessConstants.ApiName);
    }

    internal static void LogShutdown()
    {
        Log.Information(MainLoggerMessageDefinitions.Shutdown, BusinessConstants.ApiName);
    }
}