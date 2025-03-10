using Serilog;
using template.net8.api.Business;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Logger;

[CoreLibrary]
internal static class MainLoggerMethods
{
    internal static void LogMainInitConfig()
    {
        Log.Information(MainLoggerMessageDefinitions.StartingConfig, BusinessConstants.ApiName);
    }

    internal static void LogMainEndConfig()
    {
        Log.Information(MainLoggerMessageDefinitions.CompletedConfig, BusinessConstants.ApiName);
    }

    internal static void LogCriticalError(Exception ex)
    {
        Log.Error(ex, MainLoggerMessageDefinitions.CriticalError, BusinessConstants.ApiName);
    }

    internal static void LogMainShutdown()
    {
        Log.Information(MainLoggerMessageDefinitions.Shutdown, BusinessConstants.ApiName);
    }
}