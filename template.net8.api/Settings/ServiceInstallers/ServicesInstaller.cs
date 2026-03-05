using System.Reflection;
using JetBrains.Annotations;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Interfaces;
using template.net8.api.Settings.Interfaces;
using ZLinq;
using ZLinq.Linq;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class ServicesInstaller : IServiceInstaller
{
    /// <inheritdoc cref="IServiceInstaller.LoadOrder" />
    public short LoadOrder => 11;

    /// <inheritdoc cref="IServiceInstaller.InstallServiceAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        //Should be Serial
        foreach (var serviceType in GetExportedServiceTypes())
        {
            var interfaceType = GetInterfaceType(serviceType);

            if (interfaceType is not null) RegisterService(builder.Services, serviceType, interfaceType);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static ValueEnumerable<ArrayWhere<Type>, Type> GetExportedServiceTypes()
    {
        return typeof(Program).Assembly
            .GetTypes()
            .Where(static t => typeof(IServiceImplementation).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static Type? GetInterfaceType(Type serviceType)
    {
        return serviceType.GetInterfaces().SingleOrDefault(i => i.Name == $"I{serviceType.Name}");
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void RegisterService(IServiceCollection services, Type serviceType, Type interfaceType)
    {
        var serviceLifetimeAttribute =
            serviceType
                .GetCustomAttributes<ServiceLifetimeAttribute>().FirstOrDefault();

        switch (serviceLifetimeAttribute?.ServiceLifetime ?? ServiceLifetime.Scoped)
        {
            case ServiceLifetime.Scoped:
                services.AddScoped(interfaceType, serviceType);
                break;
            case ServiceLifetime.Transient:
                services.AddTransient(interfaceType, serviceType);
                break;
            case ServiceLifetime.Singleton:
                services.AddSingleton(interfaceType, serviceType);
                break;
            default:
                services.AddScoped(interfaceType, serviceType);
                break;
        }
    }
}