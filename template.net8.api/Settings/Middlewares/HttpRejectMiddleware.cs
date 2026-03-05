using Microsoft.Extensions.Localization;
using template.net8.api.Core.Factory;
using template.net8.api.Localize.Resources;

namespace template.net8.api.Settings.Middlewares;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class HttpRejectMiddleware(
    RequestDelegate next,
    IProblemDetailsService problemDetailsService,
    IStringLocalizer<ResourceMain> localizer)
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IStringLocalizer<ResourceMain> _localizer =
        localizer ?? throw new ArgumentNullException(nameof(localizer));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IProblemDetailsService _problemDetailsService =
        problemDetailsService ?? throw new ArgumentNullException(nameof(problemDetailsService));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
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