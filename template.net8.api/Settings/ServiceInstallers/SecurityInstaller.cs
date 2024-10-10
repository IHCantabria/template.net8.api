using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Security Service Installer
/// </summary>
[CoreLibrary]
public sealed class SecurityInstaller : IServiceInstaller
{
    private readonly TimeSpan _maxAge = TimeSpan.FromDays(365);

    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 20;

    /// <summary>
    ///     Install Security Service
    /// </summary>
    /// <param name="builder"></param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = _maxAge;
        });

        return Task.CompletedTask;
    }
}