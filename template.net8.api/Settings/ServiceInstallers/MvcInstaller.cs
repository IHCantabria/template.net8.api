using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     MVC Service Installer
/// </summary>
[CoreLibrary]
public sealed class MvcInstaller : IServiceInstaller
{
    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 4;


    /// <summary>
    ///     Install MVC Service
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    ///     In a set operation, the <see />
    ///     property value is invalid.
    /// </exception>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.Configure<MvcOptions>(_ =>
        {
            var culture = CultureInfo.InvariantCulture;

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        });
        return Task.CompletedTask;
    }
}