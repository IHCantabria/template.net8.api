using System.Runtime.InteropServices;
using LinqKit;
using Serilog;
using template.net8.api.Business;
using template.net8.api.Core.Logger;
using template.net8.api.Core.Logger.Extensions;
using template.net8.api.Settings.Extensions;
using template.net8.api.Settings.Handlers;
using ZLinq;


//TODO: Replace ADD DOCUMENTATION for real documentation and remove this comment
[assembly: ComVisible(false), CLSCompliant(false)]
// Define ZLinq DropIn for the assembly to generate optimized Linq queries
[assembly: ZLinqDropIn(BusinessConstants.ApiName, DropInGenerateTypes.Collection)]
SerilogLoggersFactory.MainLogFactory();
MainLoggerMethods.LogStartingMainService();

//Register Global Error and Exit Handlers
GlobalErrorAndExitHandlers.Register();

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
    MainLoggerMethods.LogCriticalMainPipelineError(e);
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    MainLoggerMethods.LogShutdown();
    await Log.CloseAndFlushAsync().ConfigureAwait(false);
}