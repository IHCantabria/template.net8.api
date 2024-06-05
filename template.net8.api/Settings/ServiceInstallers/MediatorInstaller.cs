using template.net8.Api.Core.Attributes;
using template.net8.Api.Settings.Extensions;
using template.net8.Api.Settings.Interfaces;

namespace template.net8.Api.Settings.ServiceInstallers;

/// <summary>
///     Meidator Service Installer
/// </summary>
[CoreLibrary]
public sealed class MediatorInstaller : IServiceInstaller
{
    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 14;

    /// <summary>
    ///     Install Mediator Service
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssemblyContaining<Program>();
            c.AddBehaviours();
            c.AddPostProcesses();
        });
        return Task.CompletedTask;
    }
}