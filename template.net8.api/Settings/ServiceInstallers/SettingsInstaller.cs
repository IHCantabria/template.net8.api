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
    public short LoadOrder => 6;

    /// <summary>
    ///     Install Settings services
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
        var config = builder.Configuration;
        //Load extra config files
        builder.Configuration.AddJsonFile(PackageJsonFile, false, false);
        InstallCoreOptionsServices(builder, config);
        InstallUiOptionsServices(builder, config);
        //Commented because it is not implemented in the template
        //InstallSecurityOptionsServices(builder, config);
        InstallExternalOptionsServices(builder, config);

        return Task.CompletedTask;
    }

    private static void InstallCoreOptionsServices(IHostApplicationBuilder builder, ConfigurationManager config)
    {
        //Install Options services
        builder.Services.Configure<ApiOptions>(config.GetSection(ApiOptions.Api));
        builder.Services.AddSingleton<IValidateOptions<ApiOptions>, ApiOptionsValidator>();

        builder.Services.Configure<AppOptions>(options => { options.Env = builder.Environment.EnvironmentName; });
        builder.Services.AddSingleton<IValidateOptions<AppOptions>, AppOptionsValidator>();

        builder.Services.Configure<ProjectOptions>(config);
        builder.Services.AddSingleton<IValidateOptions<ProjectOptions>, ProjectOptionsValidator>();

        builder.Services.Configure<OpenTelemetryOptions>(config);
        builder.Services.AddSingleton<IValidateOptions<OpenTelemetryOptions>, OpenTelemetryOptionsValidator>();
    }

    private static void InstallUiOptionsServices(IHostApplicationBuilder builder, ConfigurationManager config)
    {
        builder.Services.Configure<ReDocOptions>(config.GetSection(ReDocOptions.ReDoc));
        builder.Services.AddSingleton<IValidateOptions<ReDocOptions>, ReDocOptionsValidator>();

        builder.Services.Configure<SwaggerOptions>(config.GetSection(SwaggerOptions.Swagger));
        builder.Services.AddSingleton<IValidateOptions<SwaggerOptions>, SwaggerOptionsValidator>();
    }

    private static void InstallSecurityOptionsServices(IHostApplicationBuilder builder, ConfigurationManager config)
    {
        builder.Services.Configure<SwaggerSecurityOptions>(config.GetSection(SwaggerSecurityOptions.SwaggerSecurity));
        builder.Services.AddSingleton<IValidateOptions<SwaggerSecurityOptions>, SwaggerSecurityOptionsValidator>();

        builder.Services.Configure<JwtOptions>(config.GetSection(JwtOptions.Jwt));
        builder.Services.AddSingleton<IValidateOptions<JwtOptions>, JwtOptionsValidator>();

        builder.Services.Configure<PasswordOptions>(config.GetSection(PasswordOptions.Password));
        builder.Services.AddSingleton<IValidateOptions<PasswordOptions>, PasswordOptionsValidator>();
    }

    private static void InstallExternalOptionsServices(IHostApplicationBuilder builder, ConfigurationManager config)
    {
    }
}