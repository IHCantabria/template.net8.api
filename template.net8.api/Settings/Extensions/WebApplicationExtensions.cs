using System.Globalization;
using Microsoft.AspNetCore.Localization;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.Extensions;

[CoreLibrary]
internal static class WebApplicationExtensions
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
    internal static async Task ConfigurePipelinesInAssemblyAsync(this WebApplication app)
    {
        var pipelines = GetPipelineConfigurators();
        //Call the ConfigureServices for all IConfigurator implementations.
        //MUST BE Serial, keeping order of the LoadOrder
        foreach (var pipeline in pipelines.OrderBy(o => o.LoadOrder))
            await pipeline.ConfigurePipelineAsync(app).ConfigureAwait(false);
    }

    private static IEnumerable<IPipelineConfigurator> GetPipelineConfigurators()
    {
        //Get all Types in the assembly that implement IConfigurator, create a instance of the type and order it by LoadOrder.
        var exportedTypes = typeof(Program).Assembly.GetExportedTypes().Where(x =>
            typeof(IPipelineConfigurator).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false });
        return exportedTypes.Select(Activator.CreateInstance).Cast<IPipelineConfigurator>();
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>name</name>
    ///     </paramref>
    ///     is null.
    /// </exception>
    /// <exception cref="CultureNotFoundException">
    ///     <paramref>
    ///         <name>name</name>
    ///     </paramref>
    ///     is not a valid culture name. For more information,
    ///     see the Notes to Callers section.
    /// </exception>
    internal static void ConfigureLocalizationMiddleware(this WebApplication app)
    {
        // Define the supported cultures
        var defaultCulture = new CultureInfo("en");
        var supportedCultures = new[] { defaultCulture, new CultureInfo("es") };
        // Configure the Request Localization options
        var requestLocalizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en"), // Default culture
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        };
        // Add the Request Localization middleware
        app.UseRequestLocalization(requestLocalizationOptions);
    }
}