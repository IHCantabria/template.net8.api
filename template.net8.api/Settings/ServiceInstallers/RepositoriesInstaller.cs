using JetBrains.Annotations;
using template.net8.api.Persistence.Repositories;
using template.net8.api.Persistence.Repositories.Interfaces;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class RepositoriesInstaller : IServiceInstaller
{
    /// <inheritdoc cref="IServiceInstaller.LoadOrder" />
    public short LoadOrder => 10;

    /// <inheritdoc cref="IServiceInstaller.InstallServiceAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddScoped(typeof(IGenericDbRepositoryWriteContext<,>),
            typeof(GenericDbRepositoryWriteContext<,>));
        builder.Services.AddScoped(typeof(IGenericDbRepositoryReadContext<,>),
            typeof(GenericDbRepositoryReadContext<,>));
        builder.Services.AddScoped(typeof(IUnitOfWork<>),
            typeof(UnitOfWork<>));
        return Task.CompletedTask;
    }
}