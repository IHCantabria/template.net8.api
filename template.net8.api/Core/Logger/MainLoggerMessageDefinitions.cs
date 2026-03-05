namespace template.net8.api.Core.Logger;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class MainLoggerMessageDefinitions
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string StartingMainService = "Starting {Name} Service.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string BuilderStarting = "Starting App Builder for {Name} Service.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string BuilderStarted = "Started App Builder for {Name} Service.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string InstallingServices = "Installing Services in the App Builder of the {Name} Service.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string InstalledServices = "Installed Services in the App Builder of the {Name} Service.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string ContainerBuilding = "Building App Container for {Name} Service.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string ContainerBuilded = "Builded App Container for {Name} Service.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string StartingConfig = "Starting {Name} Configuration.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string CompletedConfig = "Completed {Name} configuration.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string RunningMainService = "Initializing Service {Name}.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string ReadyMainService = "Service {Name} ready to use.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string CriticalMainPipelineError =
        "Stopped {Name} due to a critical not controlled exception in the main pipeline.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string CriticalUnhandledException =
        "Stopped {Name} due to a critical not controlled exception in the system.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string UnobservedTaskException =
        "Unobserved task exception detected in a Task launched in the {Name} system.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string ProcessExit =
        "Process {Name} is exiting.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string DomainUnload =
        "AppDomain {Name} unloading.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string InitFallback =
        "An error occurred before or during the configuration of the main application log for {Name} , initiating Fallback log.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string Shutdown = "Shutdown {Name}.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string OpenTelemetryMetricCollectorEnable = "OpenTelemetry Metric Collector is enabled for {Name}.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string OpenTelemetryMetricCollectorDisable =
        "OpenTelemetry Metric Collector is disabled for {Name}.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string OpenTelemetryTraceCollectorEnable = "OpenTelemetry Trace Collector is enabled for {Name}.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string OpenTelemetryTraceCollectorDisable = "OpenTelemetry Trace Collector is disabled for {Name}.";
}