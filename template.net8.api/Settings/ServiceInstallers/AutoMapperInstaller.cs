﻿using template.net8.Api.Core.Attributes;
using template.net8.Api.Settings.Interfaces;

namespace template.net8.Api.Settings.ServiceInstallers;

/// <summary>
///     AutoMapper Service Installer
/// </summary>
[CoreLibrary]
public sealed class AutoMapperInstaller : IServiceInstaller
{
    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 10;

    /// <summary>
    ///     Install AutoMapper Service
    ///     Install AutoMapper Service
    /// </summary>
    /// <param name="builder"></param>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddAutoMapper(typeof(Program).Assembly);
        return Task.CompletedTask;
    }
}