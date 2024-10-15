using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using template.net8.api.Core.Attributes;
using template.net8.api.Localize.Resources;
using template.net8.api.Logger;

namespace template.net8.api.Settings.Handlers;

[CoreLibrary]
internal sealed class GlobalExceptionHandlerControl(
    IProblemDetailsService problemDetailsService,
    IStringLocalizer<Resource> localizer,
    ILogger<ControllerBase> logger)
    : IExceptionHandler
{
    private readonly IStringLocalizer<Resource> _localizer =
        localizer ?? throw new ArgumentNullException(nameof(localizer));

    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IProblemDetailsService _problemDetailsService =
        problemDetailsService ?? throw new ArgumentNullException(nameof(problemDetailsService));

    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        //TODO: Add exception handling specific, read https://adnanrafiq.com/blog/a-complete-guide-to-all-asp-dot-net-builtin-middlewares-part3/
        _logger.LogExceptionServer(exception.ToString());
        return _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails =
            {
                Title = _localizer["GenericServerError"],
                Detail = exception.Message,
                Type = "https://tools.ietf.org/html/rfc9110#name-500-internal-server-error"
            },
            Exception = exception
        });
    }
}