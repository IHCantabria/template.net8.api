using template.net8.api.Core.Logger;

namespace template.net8.api.Settings.Middlewares;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class RequestIdLoggingMiddleware(RequestDelegate next)
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    public Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        // Store the HttpContext.TraceIdentifier in the static provider
        RequestIdProvider.SetCurrentTraceIdentifier(context.TraceIdentifier);

        // Continue processing the request
        return _next(context);
    }
}