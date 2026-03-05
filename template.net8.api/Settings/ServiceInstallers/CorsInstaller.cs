using JetBrains.Annotations;
using Microsoft.IdentityModel.Protocols.Configuration;
using template.net8.api.Core;
using template.net8.api.Settings.Interfaces;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class CorsInstaller : IServiceInstaller
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static readonly string[] AllowedMethods = ["GET", "POST", "PUT", "DELETE", "OPTIONS"];

    /// <inheritdoc cref="IServiceInstaller.LoadOrder" />
    public short LoadOrder => 15;

    /// <inheritdoc cref="IServiceInstaller.InstallServiceAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidConfigurationException">
    ///     The Cors configuration in the appsettings file is incorrect.
    /// </exception>
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

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
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