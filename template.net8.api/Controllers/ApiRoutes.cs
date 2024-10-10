using template.net8.api.Core.Attributes;

namespace template.net8.api.Controllers;

[CoreLibrary]
internal static class ApiRoutes
{
    private const string Root = "";

    private const string Version = "v1";

    private const string PathVersion = Root + Version;

    private const string Access = "public";

    private const string PathAccess = $"{PathVersion}/{Access}";

    internal static class Dummies
    {
        private const string ControllerIdentity = "dummies";

        internal const string PathController = $"{PathAccess}/{ControllerIdentity}";

        internal const string GetDummies = "";

        internal const string GetDummy = "{dummy-key}";

        internal const string CreateDummy = "";
    }

    [CoreLibrary]
    internal static class Health
    {
        internal const string PathController = "";

        internal const string HealthCheck = "";
    }
}