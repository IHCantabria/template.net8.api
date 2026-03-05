using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using template.net8.api.Core.Authorization;
using template.net8.api.Settings.Extensions;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class AuthorizationInstaller : IServiceInstaller
{
    /// <inheritdoc cref="IServiceInstaller.LoadOrder" />
    public short LoadOrder => 19;

    /// <inheritdoc cref="IServiceInstaller.InstallServiceAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
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