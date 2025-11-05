using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;
using ZLinq;
using ZLinq.Linq;

namespace template.net8.api.Settings.Extensions;

[CoreLibrary]
internal static class WebApplicationBuilderExtensions
{
    /// <summary>
    ///     Install the services in the assembly.
    /// </summary>
    internal static async Task InstallServicesInAssemblyAsync(this WebApplicationBuilder builder)
    {
        var services = GetServiceInstallers();
        //Call the InstallServices for all IInstaller implementations.
        //MUST BE Serial, keeping order of the LoadOrder
        foreach (var service in services.OrderBy(c => c.LoadOrder))
            await service.InstallServiceAsync(builder).ConfigureAwait(false);
    }

    private static ValueEnumerable<Cast<ArrayWhereSelect<Type, object?>, object?, IServiceInstaller>, IServiceInstaller>
        GetServiceInstallers()
    {
        //Get all Types in the assembly that implement IInstaller, create a instance of the type and order it by LoadOrder.
        var exportedTypes = typeof(Program).Assembly.GetExportedTypes().Where(x =>
            typeof(IServiceInstaller).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false });
        return exportedTypes.Select(Activator.CreateInstance).Cast<IServiceInstaller>();
    }
}