using System.Diagnostics;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Middlewares;

/// <summary>
///     Middleware
/// </summary>
/// <param name="next"></param>
[CoreLibrary]
public sealed class TelemetryMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    /// <summary>
    ///     Invoke Async method to check if the request is HTTPS.
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="InvalidOperationException">
    ///     A set operation was requested, but the <see langword="Name" /> property has
    ///     already been set.
    /// </exception>
    public Task InvokeAsync(HttpContext context)
    {
        var activity = Activity.Current;
        if (activity == null) return _next(context);

        var threadId = Environment.CurrentManagedThreadId;
        var threadName = Thread.CurrentThread.Name ?? string.Empty;

        activity.SetTag("request.thread.id", threadId);
        activity.SetTag("request.thread.name", threadName);

        return _next(context);
    }
}