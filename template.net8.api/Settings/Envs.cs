using template.net8.Api.Core.Attributes;

namespace template.net8.Api.Settings;

[CoreLibrary]
internal static class Envs
{
    internal const string Local = "local";

    internal const string Development = "dev";

    internal const string PreProduction = "pre";

    internal const string Production = "prod";
}