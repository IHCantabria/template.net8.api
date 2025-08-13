using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Middlewares;

/// <summary>
///     This middleware is used to add security headers to the response.
/// </summary>
/// <param name="next"></param>
[CoreLibrary]
public sealed class AuthenticationHeaderMiddleware(RequestDelegate next)
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
            switch (context.Response.StatusCode)
            {
                case StatusCodes.Status401Unauthorized:
                {
                    SetUnauthorizedHeader(context);
                    break;
                }

                case StatusCodes.Status403Forbidden:
                {
                    SetForbiddenHeader(context);
                    break;
                }
            }

            return Task.CompletedTask;
        });

        return _next(context);
    }

    private static void SetUnauthorizedHeader(HttpContext context)
    {
        var err = context.Items.TryGetValue("BearerError", out var e) ? e as string : "invalid_token";
        var desc = context.Items.TryGetValue("BearerErrorDescription", out var d)
            ? d as string
            : "The access token is missing, invalid, or expired";

        context.Response.Headers.Remove("WWW-Authenticate");
        context.Response.Headers.Append(
            "WWW-Authenticate",
            $"Bearer error=\"{err}\", error_description=\"{desc}\""
        );
    }

    private static void SetForbiddenHeader(HttpContext context)
    {
        var desc = context.Items.TryGetValue("BearerErrorDescription", out var d)
            ? d as string
            : "The token does not have sufficient scope for this resource";

        context.Response.Headers.Remove("WWW-Authenticate");
        context.Response.Headers.Append(
            "WWW-Authenticate",
            $"Bearer error=\"insufficient_scope\", error_description=\"{desc}\""
        );
    }
}