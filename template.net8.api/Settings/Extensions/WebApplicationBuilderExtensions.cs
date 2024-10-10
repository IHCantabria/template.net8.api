using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.Extensions;

[CoreLibrary]
internal static class WebApplicationBuilderExtensions
{
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     or
    ///     <paramref>
    ///         <name>keySelector</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static async Task InstallServicesInAssemblyAsync(this WebApplicationBuilder builder)
    {
        var services = GetServiceInstallers();
        //Call the InstallServices for all IInstaller implementations.
        //MUST BE Serial, keeping order of the LoadOrder
        foreach (var service in services.OrderBy(c => c.LoadOrder))
            await service.InstallServiceAsync(builder).ConfigureAwait(false);
    }

    private static IEnumerable<IServiceInstaller> GetServiceInstallers()
    {
        //Get all Types in the assembly that implement IInstaller, create a instance of the type and order it by LoadOrder.
        var exportedTypes = typeof(Program).Assembly.GetExportedTypes().Where(x =>
            typeof(IServiceInstaller).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false });
        return exportedTypes.Select(Activator.CreateInstance).Cast<IServiceInstaller>();
    }
}