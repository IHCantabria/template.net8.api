using template.net8.api.Core.Attributes;
using template.net8.api.Domain.Persistence.Repositories;
using template.net8.api.Domain.Persistence.Repositories.Interfaces;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Repository Services Installer
/// </summary>
[CoreLibrary]
public sealed class RepositoriesInstaller : IServiceInstaller
{
    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 9;

    /// <summary>
    ///     Install Repository Services
    /// </summary>
    /// <param name="builder"></param>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddScoped(typeof(IGenericDbRepositoryScopedDbContext<,>),
            typeof(GenericDbRepositoryScopedDbContext<,>));
        builder.Services.AddScoped(typeof(IGenericDbRepositoryScopedDbContext<>),
            typeof(GenericDbRepositoryScopedDbContext<>));
        builder.Services.AddScoped(typeof(IGenericDbRepositoryTransientDbContext<,>),
            typeof(GenericDbRepositoryTransientDbContext<,>));
        builder.Services.AddScoped(typeof(IGenericDbRepositoryTransientDbContext<>),
            typeof(GenericDbRepositoryTransientDbContext<>));
        return Task.CompletedTask;
    }
}