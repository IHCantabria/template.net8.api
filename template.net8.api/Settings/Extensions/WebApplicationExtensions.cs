using template.net8.Api.Core.Attributes;
using template.net8.Api.Settings.Interfaces;

namespace template.net8.Api.Settings.Extensions;

[CoreLibrary]
internal static class WebApplicationExtensions
{
    /// <exception cref="FileNotFoundException">Unable to load a dependent assembly.</exception>
    /// <exception cref="InvalidCastException">An element in the sequence cannot be cast to type .</exception>
    /// <exception cref="ArgumentNullException"> is <see langword="null" />.</exception>
    /// <exception cref="NotSupportedException">The assembly is a dynamic assembly.</exception>
    internal static async Task ConfigurePipelinesInAssemblyAsync(this WebApplication app)
    {
        //Get all Types in the assembly that implement IConfigurator, create a instance of the type and order it by LoadOrder.
        var exportedTypes = typeof(Program).Assembly.GetExportedTypes().Where(x =>
            typeof(IPipelineConfigurator).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false });
        var pipelines = exportedTypes.Select(Activator.CreateInstance).Cast<IPipelineConfigurator>()
            .OrderBy(o => o.LoadOrder);
        //Call the ConfigureServices for all IConfigurator implementations.
        //MUST BE Serial, keeping order of the LoadOrder
        foreach (var pipeline in pipelines)
            await pipeline.ConfigurePipelineAsync(app).ConfigureAwait(false);
    }
}