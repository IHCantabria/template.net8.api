using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.PipelineConfigurators;

/// <summary>
///     UI Configurator
/// </summary>
[CoreLibrary]
public sealed class UiConfigurator : IPipelineConfigurator
{
    /// <summary>
    ///     Load order of the pipeline configurator
    /// </summary>
    public short LoadOrder => 2;

    /// <summary>
    ///     Configure Pipeline for the UI
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    public Task ConfigurePipelineAsync(WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
        UseSwagger(app);
        UseReDoc(app);

        return Task.CompletedTask;
    }

    private static void UseSwagger(WebApplication app)
    {
        //Get swagger configuration from service with strongly typed options object.
        var swaggerConfiguration = app.Services.GetRequiredService<IOptions<SwaggerOptions>>().Value;
        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger(option => { option.RouteTemplate = swaggerConfiguration.JsonRoute; });
        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(option =>
        {
            option.SwaggerEndpoint(swaggerConfiguration.UiEndpoint, swaggerConfiguration.ShortDescription);
            option.DocumentTitle = swaggerConfiguration.DocumentTitle;
            option.DocExpansion(DocExpansion.List);
            //Turns off syntax highlight which causing performance issues...
            option.ConfigObject.AdditionalItems.Add("syntaxHighlight", false);
        });
    }

    private static void UseReDoc(WebApplication app)
    {
        //Get ReDoc configuration from service with strongly typed options object.
        var reDocConfiguration = app.Services.GetRequiredService<IOptions<ReDocOptions>>().Value;

        // Enable middleware to serve generated ReDoc documentation.
        app.UseReDoc(c =>
        {
            c.RoutePrefix = reDocConfiguration.RoutePrefix;
            c.DocumentTitle = reDocConfiguration.DocumentTitle;
            c.SpecUrl = reDocConfiguration.SpecUrl.ToString();
        });
    }
}