using System.Net;
using Microsoft.AspNetCore.Http.Timeouts;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Request Timeout Service Installer
/// </summary>
[CoreLibrary]
public sealed class RequestTimeoutInstaller : IServiceInstaller
{
    private readonly TimeSpan _timeout = TimeSpan.FromMilliseconds(8000);

    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 21;


    /// <summary>
    ///     Install Request Timeout Service
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddRequestTimeouts(options =>
        {
            options.DefaultPolicy = new RequestTimeoutPolicy
            {
                Timeout = _timeout,
                TimeoutStatusCode = (short?)HttpStatusCode.RequestTimeout
            };
        });
        return Task.CompletedTask;
    }
}