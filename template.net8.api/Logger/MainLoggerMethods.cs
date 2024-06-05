using System.Globalization;

namespace template.net8.api.Logger;

internal static class MainLoggerMethods
{
    internal static void LogMainInitConfig(NLog.Logger logger, string name)
    {
        logger.Info(CultureInfo.InvariantCulture, "Starting {name} Configuration.", name);
    }

    internal static void LogMainEndConfig(NLog.Logger logger, string name)
    {
        logger.Info(CultureInfo.InvariantCulture, "Completed {name} configuration.", name);
    }

    internal static void LogMainError(NLog.Logger logger, Exception ex, string name)
    {
        logger.Error(ex, CultureInfo.InvariantCulture,
            "Stopped {name} due to a critical not controlled exception.", name);
    }

    internal static void LogMainEnd(NLog.Logger logger, string name)
    {
        logger.Info(CultureInfo.InvariantCulture, "Shutdown {name}.", name);
    }
}