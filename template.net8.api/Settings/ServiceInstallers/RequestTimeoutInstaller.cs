using System.Net;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http.Timeouts;
using template.net8.api.Core.Timeout;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class RequestTimeoutInstaller : IServiceInstaller
{
    /// <inheritdoc cref="IServiceInstaller.LoadOrder" />
    public short LoadOrder => 22;

    /// <inheritdoc cref="IServiceInstaller.InstallServiceAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddRequestTimeouts(static options =>
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