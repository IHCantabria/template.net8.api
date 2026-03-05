using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using template.net8.api.Settings.Interfaces;
using ZLinq;
using ZLinq.Linq;

namespace template.net8.api.Settings.Extensions;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class WebApplicationExtensions
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static async Task ConfigurePipelinesInAssemblyAsync(this WebApplication app)
    {
        var pipelines = GetPipelineConfigurators();
        //MUST BE Serial, keeping order of the LoadOrder
        foreach (var pipeline in pipelines.OrderBy(static o => o.LoadOrder))
            await pipeline.ConfigurePipelineAsync(app).ConfigureAwait(false);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static
        ValueEnumerable<Cast<ArrayWhereSelect<Type, object?>, object?, IPipelineConfigurator>, IPipelineConfigurator>
        GetPipelineConfigurators()
    {
        //Get all Types in the assembly that implement IConfigurator, create a instance of the type and order it by LoadOrder.
        var exportedTypes = typeof(Program).Assembly.GetTypes().Where(static x =>
            typeof(IPipelineConfigurator).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false });
        return exportedTypes.Select(Activator.CreateInstance).Cast<IPipelineConfigurator>();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
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