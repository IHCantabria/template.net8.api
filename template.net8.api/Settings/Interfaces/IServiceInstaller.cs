using template.net8.Api.Core.Attributes;

namespace template.net8.Api.Settings.Interfaces;

/// <summary>
///     Interface for Service Installer
/// </summary>
[CoreLibrary]
public interface IServiceInstaller
{
    /// <summary>
    ///     Lord Order of the Service
    /// </summary>
    short LoadOrder { get; }

    /// <summary>
    ///     Install Service Async method to install the service in the WebApplicationBuilder.
    /// </summary>
    /// <param name="builder"></param>
    Task InstallServiceAsync(WebApplicationBuilder builder);
}