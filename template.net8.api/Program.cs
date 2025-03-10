using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Serilog;
using template.net8.api.Core.Logger;
using template.net8.api.Settings.Extensions;

[assembly: ComVisible(false), CLSCompliant(false)]
MainLoggerFactory.MainLogFactory();
MainLoggerMethods.LogMainInitConfig();
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
    MainLoggerMethods.LogMainEndConfig();

    //Launch the App.
    await app.RunAsync().ConfigureAwait(false);
}
catch (Exception e)
{
    // NLog: catch setup errors
    MainLoggerMethods.LogCriticalError(e);
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    MainLoggerMethods.LogMainShutdown();
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