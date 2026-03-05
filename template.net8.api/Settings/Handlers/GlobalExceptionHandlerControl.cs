using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Localization;
using template.net8.api.Core.Factory;
using template.net8.api.Localize.Resources;
using template.net8.api.Logger;

namespace template.net8.api.Settings.Handlers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class GlobalExceptionHandlerControl(
    IProblemDetailsService problemDetailsService,
    IStringLocalizer<ResourceMain> localizer,
    ILogger<GlobalExceptionHandlerControl> logger)
    : IExceptionHandler
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IStringLocalizer<ResourceMain> _localizer =
        localizer ?? throw new ArgumentNullException(nameof(localizer));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IProblemDetailsService _problemDetailsService =
        problemDetailsService ?? throw new ArgumentNullException(nameof(problemDetailsService));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogExceptionServer(exception);
        var problemDetails =
            ProblemDetailsFactoryCore.CreateProblemDetailsByHttpStatusCode(
                (HttpStatusCode)httpContext.Response.StatusCode, exception,
                _localizer);
        return _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problemDetails
        });
    }
}