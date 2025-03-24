using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Serilog;
using template.net8.api.Core.Logger;
using template.net8.api.Core.Logger.Extensions;
using template.net8.api.Settings.Extensions;

[assembly: ComVisible(false), CLSCompliant(false)]
SerilogLoggersFactory.MainLogFactory();
MainLoggerMethods.LogStartingMainService();
try
{
    //Create the App Builder.
    MainLoggerMethods.LogBuilderStarting();
    var builder = WebApplication.CreateBuilder(args);
    MainLoggerMethods.LogBuilderStarted();

    //Install the builder services.
    MainLoggerMethods.LogInstallingServices();
    await builder.InstallServicesInAssemblyAsync().ConfigureAwait(false);
    MainLoggerMethods.LogInstalledServices();

    //Create the App Builder container.
    MainLoggerMethods.LogContainerBuilding();
    var app = builder.Build();
    MainLoggerMethods.LogContainerBuilded();

    //Configure the app services.
    MainLoggerMethods.LogStartingConfig();
    await app.ConfigurePipelinesInAssemblyAsync().ConfigureAwait(false);
    MainLoggerMethods.LogCompletedConfig();

    //Launch the App.
    MainLoggerMethods.LogRunningMainService();
    await app.RunAsync().ConfigureAwait(false);
    MainLoggerMethods.LogReadyMainService();
}
catch (Exception e)
{
    if (!Log.Logger.CurrentLoggerHasSinks())
        SerilogLoggersFactory.FallbackLogFactory();
    // NLog: catch setup errors
    MainLoggerMethods.LogCriticalError(e);
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    MainLoggerMethods.LogShutdown();
    await Log.CloseAndFlushAsync().ConfigureAwait(false);
}

namespace template.net8.api
{
    /// <summary>
    ///     Program class to start the template.net8.API. This class is used to configure the API and
    ///     launch it. Expose the Program class to use in the Integration Tests.
    /// </summary>
    [UsedImplicitly]
    public sealed class Program;
}