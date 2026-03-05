namespace template.net8.api.Core.Timeout;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class DbContextConstants
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const short MaxRetryCount = 3;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static readonly TimeSpan MaxRetryDelay = TimeSpan.FromSeconds(2);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static readonly int CommandTimeout = TimeSpan.FromSeconds(30).Seconds;
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class RequestConstants
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string RequestQueryGenericPolicy = nameof(RequestQueryGenericPolicy);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string RequestCommandGenericPolicy = nameof(RequestCommandGenericPolicy);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static readonly TimeSpan RequestCommandGenericTimeout = TimeSpan.FromSeconds(10);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static readonly TimeSpan RequestQueryGenericTimeout = TimeSpan.FromSeconds(3);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static readonly TimeSpan RequestDefaultTimeout = TimeSpan.FromSeconds(5);
}