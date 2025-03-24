using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Logger;

[CoreLibrary]
internal static class MainLoggerMessageDefinitions
{
    internal const string StartingMainService = "Starting {Name} Service";

    internal const string BuilderStarting = "Starting App Builder for {Name} Service";

    internal const string BuilderStarted = "Started App Builder for {Name} Service";

    internal const string InstallingServices = "Installing Services in the App Builder of the {Name} Service";

    internal const string InstalledServices = "Installed Services in the App Builder of the {Name} Service";

    internal const string ContainerBuilding = "Building App Container for {Name} Service";

    internal const string ContainerBuilded = "Builded App Container for {Name} Service";

    internal const string StartingConfig = "Starting {Name} Configuration";

    internal const string CompletedConfig = "Completed {Name} configuration";

    internal const string RunningMainService = "Initializing Service {Name}";

    internal const string ReadyMainService = "Service {Name} ready to use";

    internal const string CriticalError = "Stopped {Name} due to a critical not controlled exception";

    internal const string InitFallback =
        "An error occurred before or during the configuration of the main application log for {Name} , initiating Fallback log.";

    internal const string Shutdown = "Shutdown {Name}";
}