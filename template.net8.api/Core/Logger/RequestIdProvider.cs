using System.Diagnostics;

namespace template.net8.api.Core.Logger;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class RequestIdProvider
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static readonly AsyncLocal<string> CurrentTraceIdentifier = new();

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static void SetCurrentTraceIdentifier(string traceIdentifier)
    {
        CurrentTraceIdentifier.Value = traceIdentifier;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static string? GetCurrentRequestId()
    {
        return Activity.Current?.Id ?? CurrentTraceIdentifier.Value;
    }
}