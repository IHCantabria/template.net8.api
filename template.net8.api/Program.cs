using System.Runtime.InteropServices;
using JetBrains.Annotations;
using NLog;
using NLog.Web;
using template.net8.api.Business;
using template.net8.api.Core.Logger;
using template.net8.api.Settings.Extensions;

[assembly: ComVisible(false), CLSCompliant(false)]
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
MainLoggerMethods.LogMainInitConfig(logger, BusinessConstants.ApiName);
try
{
    //Create the App Builder.
    var builder = WebApplication.CreateBuilder(args);

    //Install the builder services.
    await builder.InstallServicesInAssemblyAsync().ConfigureAwait(false);

    //Create the App Builder container.
    var app = builder.Build();

    //Configure the app services.
    await app.ConfigurePipelinesInAssemblyAsync().ConfigureAwait(false);
    MainLoggerMethods.LogMainEndConfig(logger, BusinessConstants.ApiName);

    //Launch the App.
    await app.RunAsync().ConfigureAwait(false);
}
catch (Exception e)
{
    // NLog: catch setup errors
    MainLoggerMethods.LogCriticalError(logger, e, BusinessConstants.ApiName);
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    MainLoggerMethods.LogMainShutdown(logger, BusinessConstants.ApiName);
    LogManager.Shutdown();
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