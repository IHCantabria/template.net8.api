namespace template.net8.api;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class ApiRoutes
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string HubsAccess = "/hubs";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static class IdentityController
    {
        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        private const string ControllerIdentity = "identity";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string PathController = ControllerIdentity;

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string Login = "login";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string Access = "access";
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static class UsersController
    {
        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        private const string ControllerIdentity = "users";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string PathController = ControllerIdentity;

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string CreateUser = "";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string UpdateUser = "{user-key}";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string DisableUser = "{user-key}/disable";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string EnableUser = "{user-key}/enable";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string ResetUserPassword = "{user-key}/reset-password";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string DeleteUser = "{user-key}";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string GetUsers = "";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string GetUser = "{user-key}";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string Hubs = "events";
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static class UsersHub
    {
        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        private const string HubName = "users";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string PathHub = $"{HubsAccess}/{HubName}";
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static class HealthController
    {
        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        private const string ControllerIdentity = "";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string PathController = ControllerIdentity;

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string HealthCheck = "";
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static class SystemController
    {
        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        private const string ControllerIdentity = "systems";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string PathController = ControllerIdentity;

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string GetErrorCodes = "error-codes";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string GetVersion = "version";
    }
}