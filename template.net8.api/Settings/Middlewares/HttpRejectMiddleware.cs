using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Middlewares;

/// <summary>
///     This middleware is used to reject HTTP requests.
/// </summary>
/// <param name="next"></param>
/// <param name="problemDetailsService"></param>
[CoreLibrary]
public sealed class HttpRejectMiddleware(
    RequestDelegate next,
    IProblemDetailsService problemDetailsService)
{
    private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    private readonly IProblemDetailsService _problemDetailsService =
        problemDetailsService ?? throw new ArgumentNullException(nameof(problemDetailsService));

    /// <summary>
    ///     Invoke Async method to check if the request is HTTPS.
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (context.Request.IsHttps)
        {
            await _next(context).ConfigureAwait(false);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails =
                {
                    Title = "HTTP protocol is not allowed.",
                    Detail = "HTTPS protocol is required.",
                    Type = "https://tools.ietf.org/html/rfc9110#name-400-bad-request"
                }
            }).ConfigureAwait(false);
        }
    }
}