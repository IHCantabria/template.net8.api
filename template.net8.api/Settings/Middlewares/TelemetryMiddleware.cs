using System.Diagnostics;

namespace template.net8.api.Settings.Middlewares;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class TelemetryMiddleware(RequestDelegate next)
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     A set operation was requested, but the <see langword="Name" /> property has
    ///     already been set.
    /// </exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
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