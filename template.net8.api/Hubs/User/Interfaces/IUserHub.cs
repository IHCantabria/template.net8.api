using System.Diagnostics.CodeAnalysis;
using template.net8.api.Core.Contracts;
using template.net8.api.Hubs.User.Contracts;

namespace template.net8.api.Hubs.User.Interfaces;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("ReSharper", "AsyncApostle.AsyncMethodNamingHighlighting",
    Justification =
        "RPC methods should not be suffixed with 'Async' as this would leak an implementation detail to your users.")]
[SuppressMessage("Pragma", "VSTHRD200",
    Justification =
        "RPC methods should not be suffixed with 'Async' as this would leak an implementation detail to your users.")]
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "The interface is part of the public API contract and must remain publicly accessible.")]
public interface IUserHub
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task ConnectionOnline(HubInfoMessageResource message);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task ConnectionStatus(HubInfoMessageResource message);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task CreatedUser(UserHubCreatedUserMessageResource message);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task UpdatedUser(UserHubUpdatedUserMessageResource message);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task DeletedUser(UserHubDeletedUserMessageResource message);
}