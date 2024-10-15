using System.Reflection;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Interfaces;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Business Services Installer
/// </summary>
[CoreLibrary]
public sealed class ServicesInstaller : IServiceInstaller
{
    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 12;

    /// <summary>
    ///     Install Business Services
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
        var serviceTypes = GetExportedServiceTypes();
        //Should be Serial
        foreach (var serviceType in serviceTypes)
        {
            var interfaceType = GetInterfaceType(serviceType);

            if (interfaceType is not null) RegisterService(builder.Services, serviceType, interfaceType);
        }

        return Task.CompletedTask;
    }

    private static IEnumerable<Type> GetExportedServiceTypes()
    {
        return typeof(Program).Assembly
            .GetExportedTypes()
            .Where(t => typeof(IServiceImplementation).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
    }

    private static Type? GetInterfaceType(Type serviceType)
    {
        return serviceType.GetInterfaces().SingleOrDefault(i => i.Name == $"I{serviceType.Name}");
    }

    private static void RegisterService(IServiceCollection services, Type serviceType, Type interfaceType)
    {
        var serviceLifetimeAttribute =
            serviceType
                .GetCustomAttributes<ServiceLifetimeAttribute>().FirstOrDefault();
        var serviceLifetime = serviceLifetimeAttribute?.ServiceLifetime ?? ServiceLifetime.Scoped;

        switch (serviceLifetime)
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