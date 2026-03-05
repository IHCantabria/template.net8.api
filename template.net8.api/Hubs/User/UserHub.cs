using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using template.net8.api.Business;
using template.net8.api.Core.Contracts;
using template.net8.api.Hubs.User.Interfaces;
using template.net8.api.Localize.Resources;

namespace template.net8.api.Hubs.User;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("ReSharper", "AsyncApostle.AsyncMethodNamingHighlighting",
    Justification =
        "RPC methods should not be suffixed with 'Async' as this would leak an implementation detail to your users.")]
[SuppressMessage("Pragma", "VSTHRD200",
    Justification =
        "RPC methods should not be suffixed with 'Async' as this would leak an implementation detail to your users.")]
[SuppressMessage(
    "Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Public visibility is required because this Hub is part of the application messaging contract (MediatR).")]
[MustDisposeResource]
[Authorize(Policy = PoliciesConstants.UserReadPolicy)]
public sealed class UserHub(IStringLocalizer<ResourceMain> localizer) : Hub<IUserHub>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IStringLocalizer<ResourceMain> _localizer =
        localizer ?? throw new ArgumentNullException(nameof(localizer));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.ConnectionOnline(new HubInfoMessageResource
        {
            Message = _localizer["UserHubConnectionOnlineMsg"],
            ConnectionId = Context.ConnectionId
        }).ConfigureAwait(false);

        await base.OnConnectedAsync().ConfigureAwait(false);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public Task CheckConnectionStatus()
    {
        return Clients.Caller.ConnectionStatus(new HubInfoMessageResource
        {
            Message = _localizer["UserHubConnectionStatusMsg"],
            ConnectionId = Context.ConnectionId
        });
    }
}