using Microsoft.IdentityModel.Protocols.Configuration;
using template.net8.api.Core;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;
using CorsOptions = template.net8.api.Settings.Options.CorsOptions;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Cors Service Installer
/// </summary>
[CoreLibrary]
public sealed class CorsInstaller : IServiceInstaller
{
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
    /// <exception cref="InvalidConfigurationException">Condition.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        // Configure strongly typed options objects.
        var config = builder.Configuration;
        var corsOptions = config.GetSection(CorsOptions.Cors).Get<CorsOptions>();
        OptionsValidator.ValidateCorsOptions(corsOptions);
        AddCors(builder, corsOptions);
        return Task.CompletedTask;
    }

    private static void AddCors(WebApplicationBuilder builder, CorsOptions? apiOptions)
    {
        if (apiOptions is null) return;

        builder.Services.AddCors(o => o.AddPolicy(apiOptions.CorsPolicy,
            policyBuilder =>
            {
                policyBuilder.AllowAnyHeader().WithOrigins(apiOptions.ArrayAllowedOrigins.ToArray())
                    .WithExposedHeaders("Content-Disposition", "ETag").WithMethods(AllowedMethods).AllowCredentials();
            }));
    }
}