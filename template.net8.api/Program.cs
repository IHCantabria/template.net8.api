using System.Runtime.InteropServices;
using JetBrains.Annotations;
using LinqKit;
using Serilog;
using template.net8.api.Core;
using template.net8.api.Core.Logger;
using template.net8.api.Core.Logger.Extensions;
using template.net8.api.Settings.Extensions;
using ZLinq;

[assembly: ComVisible(false), CLSCompliant(false)]
// Define ZLinq DropIn for the assembly to generate optimized Linq queries
[assembly: ZLinqDropIn(CoreConstants.ApiName, DropInGenerateTypes.Collection)]
SerilogLoggersFactory.MainLogFactory();
MainLoggerMethods.LogStartingMainService();

//Configure Optimize LinqToSQL calls
LinqKitExtension.QueryOptimizer = ExpressionOptimizer.visit;
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
//TODO: Review all the comments in the code and update them.  DOcument the implementatiosn using the Inheritdoc tag for the Interfaces. Example below
// <inheritdoc cref="ISaveNotificationsDispatcher.ManageSaveEarthquakeNotificationsAsync" />