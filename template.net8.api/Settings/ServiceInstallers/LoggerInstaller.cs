using Serilog;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Extensions;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Logger Service Installer
/// </summary>
[CoreLibrary]
public sealed class LoggerInstaller : IServiceInstaller
{
    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 1;

    /// <summary>
    ///     Install Logger Service
    /// </summary>
    /// <param name="builder"></param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="IOException">Condition.</exception>
    /// <exception cref="InvalidOperationException">Condition.</exception>
    /// <exception cref="NotSupportedException">Condition.</exception>
    /// <exception cref="PathTooLongException">
    ///     When
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    ///     is too long
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    ///     The caller does not have the required permission to access the
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    /// </exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        //Remove microsoft logger instances
        builder.Logging.ClearProviders();
        //Define log min level, this is the fallback value if this value is not defined in the appsettings file.

        builder.Logging.SetMinimumLevel(LogLevel.Trace);
        //Define Serilog like default logger.

        builder.Services.AddSerilog((services, lc) =>
        {
            lc.ReadFrom.Services(services)
                .EnrichLog()
                .ConfigureMinLevels()
                .ReadFrom.Configuration(builder.Configuration);
            lc.ConfigureSinks(builder.Configuration);
        });
        return Task.CompletedTask;
    }
}