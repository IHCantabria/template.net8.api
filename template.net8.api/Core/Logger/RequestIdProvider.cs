using System.Diagnostics;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Logger;

[CoreLibrary]
internal static class RequestIdProvider
{
    private static readonly AsyncLocal<string> CurrentTraceIdentifier = new();

    /// <summary>
    ///     Sets the current HttpContext.TraceIdentifier for use in logs and problem details
    /// </summary>
    public static void SetCurrentTraceIdentifier(string traceIdentifier)
    {
        CurrentTraceIdentifier.Value = traceIdentifier;
    }

    /// <summary>
    ///     Gets the current request ID from Activity.Current?.Id or the stored TraceIdentifier
    /// </summary>
    public static string? GetCurrentRequestId()
    {
        return Activity.Current?.Id ?? CurrentTraceIdentifier.Value;
    }
}