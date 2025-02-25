﻿using NLog.Web;
using template.net8.api.Core.Attributes;
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
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        //Remove microsoft logger instances
        builder.Logging.ClearProviders();
        //Define log min level.
        builder.Logging.SetMinimumLevel(LogLevel.Trace);
        //Define NLog like default logger.
        builder.Host.UseNLog();
        return Task.CompletedTask;
    }
}