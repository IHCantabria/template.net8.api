using template.net8.api.Core.Contracts;
using template.net8.api.Hubs.User;
using template.net8.api.Hubs.User.Contracts;
using template.net8.api.Hubs.User.Interfaces;

namespace template.net8.api.Hubs;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
file enum HubEventType
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    ClientCall,

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    ServerCall
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class HubsDocumentation
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static class User
    {
        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class CheckConnectionStatus
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Name = nameof(UserHub.CheckConnectionStatus);

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Type = nameof(HubEventType.ClientCall);
        }

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class ConnectionStatus
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Name = nameof(IUserHub.ConnectionStatus);

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Type = nameof(HubEventType.ServerCall);

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal static readonly string[] Fields =
                [nameof(HubInfoMessageResource.ConnectionId), nameof(HubInfoMessageResource.Message)];
        }

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class ConnectionOnline
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Name = nameof(IUserHub.ConnectionOnline);

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Type = nameof(HubEventType.ServerCall);

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal static readonly string[] Fields =
                [nameof(HubInfoMessageResource.ConnectionId), nameof(HubInfoMessageResource.Message)];
        }

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class CreatedUser
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Name = nameof(IUserHub.CreatedUser);

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Type = nameof(HubEventType.ServerCall);

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal static readonly string[] Fields =
            [
                nameof(UserHubCreatedUserMessageResource.Uuid),
                nameof(UserHubCreatedUserMessageResource.Message)
            ];
        }

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class UpdatedUser
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Name = nameof(IUserHub.UpdatedUser);

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Type = nameof(HubEventType.ServerCall);

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal static readonly string[] Fields =
            [
                nameof(UserHubUpdatedUserMessageResource.Uuid),
                nameof(UserHubUpdatedUserMessageResource.Message)
            ];
        }

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class DeletedUser
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Name = nameof(IUserHub.DeletedUser);

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Type = nameof(HubEventType.ServerCall);

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal static readonly string[] Fields =
            [
                nameof(UserHubDeletedUserMessageResource.Uuid),
                nameof(UserHubDeletedUserMessageResource.Message)
            ];
        }
    }
}