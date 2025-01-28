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
    public short LoadOrder => 5;

    /// <summary>
    ///     Install MVC Service
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     In a set operation, the
    ///     <see>
    ///         <cref>P:System.Globalization.CultureInfo.Name</cref>
    ///     </see>
    ///     property value is invalid.
    /// </exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.Configure<MvcOptions>(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;

            var culture = CultureInfo.InvariantCulture;

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        });
        return Task.CompletedTask;
    }
}