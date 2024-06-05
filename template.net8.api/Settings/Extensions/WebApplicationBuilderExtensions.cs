using template.net8.Api.Core.Attributes;
using template.net8.Api.Settings.Interfaces;

namespace template.net8.Api.Settings.Extensions;

[CoreLibrary]
internal static class WebApplicationBuilderExtensions
{
    /// <exception cref="ArgumentNullException"> source or predicate is <see langword="null" />.</exception>
    /// <exception cref="InvalidCastException">An element in the sequence cannot be cast to type TResult />.</exception>
    /// <exception cref="FileNotFoundException">Unable to load a dependent assembly.</exception>
    /// <exception cref="NotSupportedException">The assembly is a dynamic assembly.</exception>
    internal static async Task InstallServicesInAssemblyAsync(this WebApplicationBuilder builder)
    {
        //Get all Types in the assembly that implement IInstaller, create a instance of the type and order it by LoadOrder.
        var exportedTypes = typeof(Program).Assembly.GetExportedTypes().Where(x =>
            typeof(IServiceInstaller).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false });
        var services = exportedTypes.Select(Activator.CreateInstance).Cast<IServiceInstaller>()
            .OrderBy(c => c.LoadOrder);
        //Call the InstallServices for all IInstaller implementations.
        //MUST BE Serial, keeping order of the LoadOrder
        foreach (var service in services) await service.InstallServiceAsync(builder).ConfigureAwait(false);
    }
}