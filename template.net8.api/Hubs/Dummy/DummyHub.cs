using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using template.net8.api.Core.Contracts;
using template.net8.api.Hubs.Dummy.Interfaces;
using template.net8.api.Localize.Resources;

namespace template.net8.api.Hubs.Dummy;

/// <summary>
///     Dummy Hub
/// </summary>
[SuppressMessage("ReSharper", "AsyncApostle.AsyncMethodNamingHighlighting",
    Justification =
        "RPC methods should not be suffixed with 'Async' as this would leak an implementation detail to your users.")]
[SuppressMessage("Pragma", "VSTHRD200",
    Justification =
        "RPC methods should not be suffixed with 'Async' as this would leak an implementation detail to your users.")]
public sealed class DummyHub(IStringLocalizer<ResourceMain> localizer) : Hub<IDummyHub>
{
    private readonly IStringLocalizer<ResourceMain> _localizer =
        localizer ?? throw new ArgumentNullException(nameof(localizer));

    /// <summary>
    ///     On Connected Async Confirmation
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.ConnectionOnline(new HubInfoMessageResource
        {
            Message = _localizer["CreateDummyValidatorTextInvalidMsg"], // TODO:FIX
            ConnectionId = Context.ConnectionId
        }).ConfigureAwait(false);

        await base.OnConnectedAsync().ConfigureAwait(false);
    }

    /// <summary>
    ///     Check if the connection is still active
    /// </summary>
    [UsedImplicitly]
    public Task CheckConnectionStatus()
    {
        // Responde al cliente que hizo la petición indicando que la conexión está activa
        return Clients.Caller.ConnectionStatus(new HubInfoMessageResource
        {
            Message = _localizer["CreateDummyValidatorTextInvalidMsg"], // TODO:FIX
            ConnectionId = Context.ConnectionId
        });
    }
}