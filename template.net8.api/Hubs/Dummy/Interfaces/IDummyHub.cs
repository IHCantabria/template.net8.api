using System.Diagnostics.CodeAnalysis;
using template.net8.api.Core.Contracts;
using template.net8.api.Hubs.Dummy.Contracts;

namespace template.net8.api.Hubs.Dummy.Interfaces;

/// <summary>
///     Interface Dummy Hub
/// </summary>
[SuppressMessage("ReSharper", "AsyncApostle.AsyncMethodNamingHighlighting",
    Justification =
        "RPC methods should not be suffixed with 'Async' as this would leak an implementation detail to your users.")]
[SuppressMessage("Pragma", "VSTHRD200",
    Justification =
        "RPC methods should not be suffixed with 'Async' as this would leak an implementation detail to your users.")]
public interface IDummyHub
{
    /// <summary>
    ///     Connection online
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task ConnectionOnline(HubInfoMessageResource message);

    /// <summary>
    ///     Check connection status
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task ConnectionStatus(HubInfoMessageResource message);

    /// <summary>
    ///     Notifies about a new Dummy event.
    /// </summary>
    /// <param name="message">The Dummy data.</param>
    /// <returns></returns>
    Task NewDummy(DummyHubNewDummyMessageResource message);
}