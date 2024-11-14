using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Timeout;

[CoreLibrary]
internal static class DbContextConstants
{
    internal const short MaxRetryCount = 3;
    internal static readonly TimeSpan MaxRetryDelay = TimeSpan.FromSeconds(2);
    internal static readonly int CommandTimeout = TimeSpan.FromSeconds(30).Seconds;
}

[CoreLibrary]
internal static class RequestConstants
{
    internal const string RequestQueryGenericPolicy = nameof(RequestQueryGenericPolicy);
    internal const string RequestCommandGenericPolicy = nameof(RequestCommandGenericPolicy);

    internal static readonly TimeSpan RequestCommandGenericTimeout = TimeSpan.FromSeconds(10);
    internal static readonly TimeSpan RequestQueryGenericTimeout = TimeSpan.FromSeconds(3);

    internal static readonly TimeSpan RequestDefaultTimeout = TimeSpan.FromSeconds(5);
}