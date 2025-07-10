using template.net8.api.Core.Attributes;

namespace template.net8.api;

[CoreLibrary]
internal static class ApiRoutes
{
    private const string HubsAccess = "/hubs";

    internal static class DummiesController
    {
        private const string ControllerIdentity = "dummies";

        internal const string PathController = $"{ControllerIdentity}";

        internal const string GetDummies = "";

        internal const string GetDummy = "{dummy-key}";

        internal const string CreateDummy = "";

        internal const string Hubs = "events";
    }

    internal static class DummiesHub
    {
        private const string HubName = "dummies";

        internal const string PathHub = $"{HubsAccess}/{HubName}";
    }

    [CoreLibrary]
    internal static class HealthController
    {
        private const string ControllerIdentity = "";

        internal const string PathController = $"{ControllerIdentity}";

        internal const string HealthCheck = "";
    }

    [CoreLibrary]
    internal static class SystemController
    {
        private const string ControllerIdentity = "systems";

        internal const string PathController = $"{ControllerIdentity}";

        internal const string GetErrorCodes = "error-codes";

        internal const string GetVersion = "version";
    }
}