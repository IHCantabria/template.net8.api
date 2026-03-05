using System.Text;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Tokens;
using template.net8.api.Core;
using template.net8.api.Settings.Events;
using template.net8.api.Settings.Interfaces;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class AuthenticationInstaller : IServiceInstaller
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private JwtOptions? _config;

    /// <inheritdoc cref="IServiceInstaller.LoadOrder" />
    public short LoadOrder => 18;

    /// <inheritdoc cref="IServiceInstaller.InstallServiceAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        RegisterServices(builder.Services);
        var (jwtOptions, environment) = LoadAndValidateOptions(builder);

        ConfigureIdentityModelLogging(environment);
        ConfigureAuthentication(builder.Services, environment);

        _config = jwtOptions;
        return Task.CompletedTask;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<AppJwtBearerEvents>();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static (JwtOptions? jwtOptions, IWebHostEnvironment Environment) LoadAndValidateOptions(
        WebApplicationBuilder builder)
    {
        var jwtOptions = builder.Configuration
            .GetSection(JwtOptions.Jwt)
            .Get<JwtOptions>();

        OptionsValidator.ValidateJwtOptions(jwtOptions);

        return (jwtOptions, builder.Environment);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureIdentityModelLogging(IHostEnvironment environment)
    {
        if (environment.EnvironmentName is not (Envs.Development or Envs.Local or Envs.Test))
            return;

        IdentityModelEventSource.ShowPII = true;
        IdentityModelEventSource.LogCompleteSecurityArtifact = true;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private void ConfigureAuthentication(IServiceCollection services, IHostEnvironment environment)
    {
        services.AddAuthentication(static x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => ConfigureJwtBearer(x, environment));
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private void ConfigureJwtBearer(JwtBearerOptions options, IHostEnvironment environment)
    {
        if (_config is null)
            throw new InvalidConfigurationException("The Jwt configuration in the appsettings file is incorrect.");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidIssuer = _config.Issuer,
            IssuerSigningKeys =
                [new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret))],
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = _config.Audience,
            ValidateLifetime = true,
            LifetimeValidator = LifetimeValidator
        };
        if (environment.EnvironmentName is Envs.Development or Envs.Local or Envs.Test)
            options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.IncludeErrorDetails = true;
        options.EventsType = typeof(AppJwtBearerEvents);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static bool LifetimeValidator(DateTime? notBefore,
        DateTime? expires,
        SecurityToken securityToken,
        TokenValidationParameters validationParameters)
    {
        return expires != null && expires > DateTime.Now;
    }
}