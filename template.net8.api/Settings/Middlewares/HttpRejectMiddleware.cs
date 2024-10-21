using Microsoft.Extensions.Localization;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Factory;
using template.net8.api.Localize.Resources;

namespace template.net8.api.Settings.Middlewares;

/// <summary>
///     This middleware is used to reject HTTP requests.
/// </summary>
/// <param name="next"></param>
/// <param name="problemDetailsService"></param>
/// <param name="localizer"></param>
[CoreLibrary]
public sealed class HttpRejectMiddleware(
    RequestDelegate next,
    IProblemDetailsService problemDetailsService,
    IStringLocalizer<Resource> localizer)
{
    private readonly IStringLocalizer<Resource> _localizer =
        localizer ?? throw new ArgumentNullException(nameof(localizer));

    private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    private readonly IProblemDetailsService _problemDetailsService =
        problemDetailsService ?? throw new ArgumentNullException(nameof(problemDetailsService));

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
            var problemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsBadRequestHttpNotSupported(_localizer);
            await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails = problemDetails
            }).ConfigureAwait(false);
        }
    }
}