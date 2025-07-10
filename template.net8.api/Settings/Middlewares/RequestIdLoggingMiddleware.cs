using template.net8.api.Core.Attributes;
using template.net8.api.Core.Logger;

namespace template.net8.api.Settings.Middlewares;

/// <summary>
///     Middleware to capture HttpContext.TraceIdentifier for RequestIdEnricher
/// </summary>
/// <param name="next"></param>
[CoreLibrary]
public sealed class RequestIdLoggingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    /// <summary>
    ///     Invoke Async method to check if the request is HTTPS.
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
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