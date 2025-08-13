using Microsoft.Extensions.Primitives;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Middlewares;

/// <summary>
///     This middleware is used to add security headers to the response.
/// </summary>
/// <param name="next"></param>
[CoreLibrary]
public sealed class SecurityHeadersMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    /// <summary>
    ///     Invoke Async method to invoke the middleware. This method adds the security headers to the response.
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

        context.Response.OnStarting(() =>
        {
            SetSecurityHeaders(context);

            return Task.CompletedTask;
        });

        return _next(context);
    }

    private static void SetSecurityHeaders(HttpContext context)
    {
        // Content-Security-Policy Header
        context.Response.Headers.Append("Content-Security-Policy",
            new StringValues(
                "default-src 'self'; style-src 'self' 'unsafe-inline'; img-src 'self' avatars3.githubusercontent.com cdn.redoc.ly data:; style-src-elem 'self' 'unsafe-inline' fonts.googleapis.com; script-src-elem 'self' 'unsafe-inline'; worker-src blob:; font-src fonts.gstatic.com"));

        // X-Content-Type-Options Header
        context.Response.Headers.Append("X-Content-Type-Options", new StringValues("nosniff"));

        // X-Frame-Options Header
        context.Response.Headers.Append("X-Frame-Options", new StringValues("SAMEORIGIN"));

        // X-XSS-Protection Header
        context.Response.Headers.Append("X-XSS-Protection", new StringValues("1; mode=block"));

        // Referrer-Policy Header
        context.Response.Headers.Append("Referrer-Policy", new StringValues("no-referrer"));

        // X-Permitted-Cross-Domain-Policies Header
        context.Response.Headers.Append("X-Permitted-Cross-Domain-Policies", new StringValues("none"));

        // Permissions-Policy Header
        context.Response.Headers.Append("Permissions-Policy",
            new StringValues(
                "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()"));
    }
}