using System.Globalization;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Logger;

[CoreLibrary]
internal static class MainLoggerMethods
{
    internal static void LogMainInitConfig(NLog.Logger logger, string name)
    {
        logger.Info(CultureInfo.InvariantCulture, MainLoggerMessageDefinitions.StartingConfig, name);
    }

    internal static void LogMainEndConfig(NLog.Logger logger, string name)
    {
        logger.Info(CultureInfo.InvariantCulture, MainLoggerMessageDefinitions.CompletedConfig, name);
    }

    internal static void LogCriticalError(NLog.Logger logger, Exception ex, string name)
    {
        logger.Error(ex, CultureInfo.InvariantCulture, MainLoggerMessageDefinitions.CriticalError, name);
    }

    internal static void LogMainShutdown(NLog.Logger logger, string name)
    {
        logger.Info(CultureInfo.InvariantCulture, MainLoggerMessageDefinitions.Shutdown, name);
    }
}