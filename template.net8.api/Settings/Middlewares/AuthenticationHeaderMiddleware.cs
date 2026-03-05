namespace template.net8.api.Settings.Middlewares;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class AuthenticationHeaderMiddleware(RequestDelegate next)
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
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

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
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

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
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