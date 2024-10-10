using template.net8.api.Core.Attributes;

namespace template.net8.api.Logger;

[CoreLibrary]
internal static class MainLoggerMessageDefinitions
{
    internal const string StartingConfig = "Starting {Name} Configuration";

    internal const string CompletedConfig = "Completed {Name} configuration";

    internal const string CriticalError = "Stopped {Name} due to a critical not controlled exception";

    internal const string Shutdown = "Shutdown {Name}";
}