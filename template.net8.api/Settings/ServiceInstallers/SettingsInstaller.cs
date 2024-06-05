using Microsoft.Extensions.Options;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Settings Services Installer
/// </summary>
[CoreLibrary]
public sealed class SettingsInstaller : IServiceInstaller
{
    private const string PackageJsonFile = "package.json";

    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 5;

    /// <summary>
    ///     Install Settings services
    /// </summary>
    /// <param name="builder"></param>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var config = builder.Configuration;
        //Load extra config files
        builder.Configuration.AddJsonFile(PackageJsonFile, false, false);

        //Install Options services
        builder.Services.Configure<ApiOptions>(config.GetSection(ApiOptions.Api));
        builder.Services.AddSingleton<IValidateOptions<ApiOptions>, ApiOptionsValidator>();

        builder.Services.Configure<ProjectDbOptions>(config.GetSection(ProjectDbOptions.ProjectDb));
        builder.Services.AddSingleton<IValidateOptions<ProjectDbOptions>, ProjectDbOptionsValidator>();

        builder.Services.Configure<ProjectOptions>(config);
        builder.Services.AddSingleton<IValidateOptions<ProjectOptions>, ProjectOptionsValidator>();

        builder.Services.Configure<ReDocOptions>(config.GetSection(ReDocOptions.ReDoc));
        builder.Services.AddSingleton<IValidateOptions<ReDocOptions>, ReDocOptionsValidator>();

        builder.Services.Configure<SwaggerOptions>(config.GetSection(SwaggerOptions.Swagger));
        builder.Services.AddSingleton<IValidateOptions<SwaggerOptions>, SwaggerOptionsValidator>();

        return Task.CompletedTask;
    }
}