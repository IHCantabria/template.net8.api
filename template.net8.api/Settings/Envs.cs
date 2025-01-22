using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings;

[CoreLibrary]
internal static class Envs
{
    internal const string Local = "local";

    internal const string Test = "test";

    internal const string Development = "dev";

    internal const string PreProduction = "pre";

    internal const string Production = "prod";
}