using System.Net;
using Microsoft.AspNetCore.Http.Timeouts;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Timeout;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Request Timeout Service Installer
/// </summary>
[CoreLibrary]
public sealed class RequestTimeoutInstaller : IServiceInstaller
{
    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 22;

    /// <summary>
    ///     Install Request Timeout Service
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddRequestTimeouts(options =>
        {
            options.DefaultPolicy = new RequestTimeoutPolicy
            {
                Timeout = RequestConstants.RequestDefaultTimeout,
                TimeoutStatusCode = (short?)HttpStatusCode.RequestTimeout
            };
            options.AddPolicy(RequestConstants.RequestQueryGenericPolicy, new RequestTimeoutPolicy
            {
                Timeout = RequestConstants.RequestQueryGenericTimeout,
                TimeoutStatusCode = (short?)HttpStatusCode.RequestTimeout
            });
            options.AddPolicy(RequestConstants.RequestCommandGenericPolicy, new RequestTimeoutPolicy
            {
                Timeout = RequestConstants.RequestCommandGenericTimeout,
                TimeoutStatusCode = (short?)HttpStatusCode.RequestTimeout
            });
        });
        return Task.CompletedTask;
    }
}