﻿using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     OpenApi Service Installer
/// </summary>
[CoreLibrary]
public sealed class OpenApiInstaller : IServiceInstaller
{
    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 17;

    /// <summary>
    ///     Install OpenApi Service
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
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        return Task.CompletedTask;
    }
}