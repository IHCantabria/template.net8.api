using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Tokens;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Authentication Service Installer
/// </summary>
[CoreLibrary]
public sealed class AuthenticationInstaller : IServiceInstaller
{
    // Commented because Authentication is not implemented in this template.
    //private JwtOptions? _config;

    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 19;

    /// <summary>
    ///     Install Authentication Services
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        // Commented because Authentication is not implemented in this template.
        ArgumentNullException.ThrowIfNull(builder);
        //builder.Services.AddScoped<AppJwtBearerEvents>();
        // configure strongly typed settings objects
        //var authenticationOptions = builder.Configuration.GetSection(JwtOptions.Jwt).Get<JwtOptions>();
        //ValidateJwtOptions(authenticationOptions);
        //_config = authenticationOptions;
        //var environment = builder.Environment;
        //if (environment.EnvironmentName is Envs.Development or Envs.Local or Envs.Test)
        //{
        //    IdentityModelEventSource.ShowPII = true;
        //    IdentityModelEventSource.LogCompleteSecurityArtifact = true;
        //}
        //builder.Services.AddAuthentication(x =>
        //    {
        //        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //    })
        //    .AddJwtBearer(x => ConfigureJwtBearer(x, environment));
        return Task.CompletedTask;
    }

    //private void ConfigureJwtBearer(JwtBearerOptions options, IWebHostEnvironment environment)
    //{
    //    options.TokenValidationParameters = new TokenValidationParameters
    //    {
    //        ValidateIssuerSigningKey = true,
    //        ValidIssuer = _config!.Issuer,
    //        IssuerSigningKeys =
    //            [new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret))],
    //        ValidateIssuer = true,
    //        ValidateAudience = true,
    //        ValidAudience = _config.Audience,
    //        ValidateLifetime = true,
    //        LifetimeValidator = LifetimeValidator
    //    };
    //    if (environment.EnvironmentName is Envs.Development or Envs.Local or Envs.Test)
    //        options.RequireHttpsMetadata = false;
    //    options.SaveToken = true;
    //    options.IncludeErrorDetails = true;
    //    options.EventsType = typeof(AppJwtBearerEvents);
    //}

    private static bool LifetimeValidator(DateTime? notBefore,
        DateTime? expires,
        SecurityToken securityToken,
        TokenValidationParameters validationParameters)
    {
        return expires != null && expires > DateTime.Now;
    }

    private static void ValidateJwtOptions(JwtOptions? config)
    {
        var optionsValidator = new JwtOptionsValidator();
        if (config is null)
            throw new InvalidConfigurationException(
                "The Jwt configuration in the appsettings file is incorrect");

        var validation = optionsValidator.Validate(null, config);
        if (validation.Failed)
            throw new InvalidConfigurationException(validation.FailureMessage);
    }
}