using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Logger;

[CoreLibrary]
internal static class MainLoggerMessageDefinitions
{
    internal const string StartingMainService = "Starting {Name} Service.";

    internal const string BuilderStarting = "Starting App Builder for {Name} Service.";

    internal const string BuilderStarted = "Started App Builder for {Name} Service.";

    internal const string InstallingServices = "Installing Services in the App Builder of the {Name} Service.";

    internal const string InstalledServices = "Installed Services in the App Builder of the {Name} Service.";

    internal const string ContainerBuilding = "Building App Container for {Name} Service.";

    internal const string ContainerBuilded = "Builded App Container for {Name} Service.";

    internal const string StartingConfig = "Starting {Name} Configuration.";

    internal const string CompletedConfig = "Completed {Name} configuration.";

    internal const string RunningMainService = "Initializing Service {Name}.";

    internal const string ReadyMainService = "Service {Name} ready to use.";

    internal const string CriticalMainPipelineError =
        "Stopped {Name} due to a critical not controlled exception in the main pipeline.";

    internal const string CriticalUnhandledException =
        "Stopped {Name} due to a critical not controlled exception in the system.";

    internal const string UnobservedTaskException =
        "Unobserved task exception detected in a Task launched in the {Name} system.";

    internal const string ProcessExit =
        "Process {Name} is exiting.";

    internal const string DomainUnload =
        "AppDomain {Name} unloading.";

    internal const string InitFallback =
        "An error occurred before or during the configuration of the main application log for {Name} , initiating Fallback log.";

    internal const string Shutdown = "Shutdown {Name}.";

    internal const string OpenTelemetryMetricCollectorEnable = "OpenTelemetry Metric Collector is enabled for {Name}.";

    internal const string OpenTelemetryMetricCollectorDisable =
        "OpenTelemetry Metric Collector is disabled for {Name}.";

    internal const string OpenTelemetryTraceCollectorEnable = "OpenTelemetry Trace Collector is enabled for {Name}.";

    internal const string OpenTelemetryTraceCollectorDisable = "OpenTelemetry Trace Collector is disabled for {Name}.";
}