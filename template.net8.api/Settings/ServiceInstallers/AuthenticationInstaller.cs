using Microsoft.IdentityModel.Tokens;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;

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
    public short LoadOrder => 18;

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
        ArgumentNullException.ThrowIfNull(builder);

        // Commented because Authentication is not implemented in this template. Install Microsoft.AspNetCore.Authentication.JwtBearer
        // configure strongly typed settings objects
        //var authenticationOptions = builder.Configuration.GetSection(JwtOptions.Jwt).Get<JwtOptions>();
        //_config = authenticationOptions;
        //builder.Services.AddAuthentication(x =>
        //    {
        //        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //    })
        //    .AddJwtBearer(ConfigureJwtBearer);
        return Task.CompletedTask;
    }

    //private void ConfigureJwtBearer(JwtBearerOptions options)
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
    //    options.SaveToken = true;
    //    options.IncludeErrorDetails = true;
    //    options.Events = new JwtBearerEvents
    //    {
    //        OnAuthenticationFailed = _ => HandleTokenAuthenticationFailedAsync(),
    //        OnMessageReceived = context =>
    //        {
    //            // Make authentication optional if Authorization header is missing for DEV environment
    //            if (ShouldAuthenticate(context, _config)) return Task.CompletedTask;

    //            var result = TokenFactory.GenerateGenieAccessToken(_config!).Try();
    //            if (result.IsFaulted)
    //                throw new InternalServerErrorException(MessageDefinitions.GenericServerError,
    //                    result.ExtractException());

    //            context.Token = result.ExtractData(); // Create Valid Token

    //            return Task.CompletedTask;
    //        }
    //    };
    //}

    private static bool LifetimeValidator(DateTime? notBefore,
        DateTime? expires,
        SecurityToken securityToken,
        TokenValidationParameters validationParameters)
    {
        return expires != null && expires > DateTime.Now;
    }

    private static Task HandleTokenAuthenticationFailedAsync()
    {
        return Task.CompletedTask;
    }

    //private static bool ShouldAuthenticate(MessageReceivedContext? context, JwtOptions? config)
    //{
    //    if (context is null) return true;
    //    if (config is null) return true;

    //    var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") is not Envs.Production;

    //    return !string.IsNullOrEmpty(context.HttpContext.Request.Headers.Authorization) || !isDev;
    //}
}