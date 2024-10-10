using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Cors Service Installer
/// </summary>
[CoreLibrary]
public sealed class CorsInstaller : IServiceInstaller
{
    private const string IhCantabriaDomain = "https://*.ihcantabria.com";
    private static readonly string[] AllowedMethods = ["GET", "POST", "OPTIONS"];

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
        AddCors(builder, apiOptions);
        return Task.CompletedTask;
    }

    private static void AddCors(IHostApplicationBuilder builder, ApiOptions? apiOptions)
    {
        if (apiOptions is null) return;

        builder.Services.AddCors(o => o.AddPolicy(apiOptions.CorsPolicy,
            policyBuilder =>
            {
                policyBuilder.AllowAnyHeader().WithExposedHeaders("Content-Disposition");
                switch (builder.Environment.EnvironmentName)
                {
                    case Envs.Local:
                        policyBuilder.AllowAnyOrigin().AllowAnyMethod();
                        break;
                    case Envs.Development:
                        policyBuilder.AllowAnyOrigin().WithMethods(AllowedMethods);
                        break;
                    case Envs.PreProduction or Envs.Production:
                        policyBuilder.SetIsOriginAllowedToAllowWildcardSubdomains().WithOrigins(IhCantabriaDomain)
                            .WithMethods(AllowedMethods);
                        break;
                    default:
                        policyBuilder.AllowAnyOrigin().AllowAnyMethod();
                        break;
                }
            }));
    }
}