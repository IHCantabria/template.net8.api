using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using template.net8.Api.Core.Attributes;
using template.net8.Api.Settings.Interfaces;

namespace template.net8.Api.Settings.PipelineConfigurators;

/// <summary>
///     Health Configurator
/// </summary>
[CoreLibrary]
public sealed class HealthConfigurator : IPipelineConfigurator
{
    /// <summary>
    ///     Load order of the pipeline configurator
    /// </summary>
    public short LoadOrder => 3;

    /// <summary>
    ///     Configure Health Checks Middleware
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    public Task ConfigurePipelineAsync(WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        //HealthCheck Middleware
        app.MapHealthChecks("/healthchecks", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.MapHealthChecksUI(options => { options.UIPath = "/monitor"; });

        return Task.CompletedTask;
    }
}