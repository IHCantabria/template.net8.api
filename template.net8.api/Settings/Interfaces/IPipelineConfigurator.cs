using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Interfaces;

/// <summary>
///     Interface for Pipeline Configurator
/// </summary>
[CoreLibrary]
public interface IPipelineConfigurator
{
    /// <summary>
    ///     Load Order for the Pipeline Configurator
    /// </summary>
    short LoadOrder { get; }

    /// <summary>
    ///     Configure Pipeline Async method to configure the pipeline for the application.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    Task ConfigurePipelineAsync(WebApplication app);
}