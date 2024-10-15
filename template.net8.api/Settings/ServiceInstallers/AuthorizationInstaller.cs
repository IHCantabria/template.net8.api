using Microsoft.AspNetCore.Authorization;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Authorization;
using template.net8.api.Settings.Extensions;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Authorization Service Installer
/// </summary>
[CoreLibrary]
public sealed class AuthorizationInstaller : IServiceInstaller
{
    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 20;

    /// <summary>
    ///     Install Repository Services
    /// </summary>
    /// <param name="builder"></param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddSingleton<IAuthorizationHandler, ClaimRequirementHandler>();

        var authorizationBuilder = builder.Services.AddAuthorizationBuilder();
        var isProduction = builder.Environment.EnvironmentName == Envs.Production;

        authorizationBuilder.AddPolicies(isProduction);

        return Task.CompletedTask;
    }
}