namespace template.net8.api.Controllers;

internal static class ApiRoutes
{
    private const string Root = "";

    private const string Version = "v1";

    private const string PathVersion = Root + Version;

    private const string Access = "public";

    private const string PathAccess = $"{PathVersion}/{Access}";

    internal static class Dummy
    {
        private const string ControllerDummy = "dummy";

        private const string PathControllerDummy = $"{PathAccess}/{ControllerDummy}";

        internal const string GetDummies = PathControllerDummy + "/";

        internal const string CreateDummy = PathControllerDummy + "/";
    }
}