using template.net8.api.Core.Attributes;
using template.net8.api.Hubs.Extensions;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.PipelineConfigurators;

/// <summary>
///     Hubs Configurator
/// </summary>
[CoreLibrary]
public sealed class HubsConfigurator : IPipelineConfigurator
{
    /// <summary>
    ///     Load order of the pipeline configurator
    /// </summary>
    public short LoadOrder => 5;

    /// <summary>
    ///     Configure Pipeline for the Hubs
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public Task ConfigurePipelineAsync(WebApplication app)
    {
        app.ConfigureHubs();

        return Task.CompletedTask;
    }
}