using Microsoft.IdentityModel.Protocols.Configuration;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Cors Service Installer
/// </summary>
[CoreLibrary]
//TODO:Move this configuration to the appsettings.json file (AllowedDomains, AllowedMethods)
public sealed class CorsInstaller : IServiceInstaller
{
    private static readonly string[] AllowedDomains =
    [
        "https://tsunami-pre.ihcantabria.com", "https://tsunamidev.ihcantabria.com", "https://tsunami.ihcantabria.com"
    ];

    private static readonly string[] IhDevelopmentDomains =
        ["http://localhost:8080", "https://localhost:8080", "http://localhost:8081", "https://localhost:8081"];

    private static readonly string[] AllowedMethods = ["GET", "POST", "PUT", "DELETE", "OPTIONS"];

    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 15;

    /// <summary>
    ///     Install Cors Service
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
        // Configure strongly typed options objects.
        var config = builder.Configuration;
        var apiOptions = config.GetSection(ApiOptions.Api).Get<ApiOptions>();
        ValidateApiOptions(apiOptions);
        AddCors(builder, apiOptions);
        return Task.CompletedTask;
    }

    private static void AddCors(IHostApplicationBuilder builder, ApiOptions? apiOptions)
    {
        if (apiOptions is null) return;

        builder.Services.AddCors(o => o.AddPolicy(apiOptions.CorsPolicy,
            policyBuilder =>
            {
                policyBuilder.AllowAnyHeader().WithExposedHeaders("Content-Disposition", "ETag");
                switch (builder.Environment.EnvironmentName)
                {
                    case Envs.Local:
                    case Envs.Test:
                        policyBuilder.WithOrigins(IhDevelopmentDomains).AllowAnyMethod().AllowCredentials();
                        break;
                    case Envs.Development:
                        policyBuilder.WithOrigins(IhDevelopmentDomains.Concat(AllowedDomains).ToArray())
                            .WithMethods(AllowedMethods).AllowCredentials();
                        break;
                    case Envs.PreProduction or Envs.Production:
                        policyBuilder.WithOrigins(AllowedDomains).WithMethods(AllowedMethods).AllowCredentials();
                        break;
                }
            }));
    }

    private static void ValidateApiOptions(ApiOptions? config)
    {
        var optionsValidator = new ApiOptionsValidator();
        if (config is null)
            throw new InvalidConfigurationException(
                "The Api configuration in the appsettings file is incorrect");

        var validation = optionsValidator.Validate(null, config);
        if (validation.Failed)
            throw new InvalidConfigurationException(validation.FailureMessage);
    }
}