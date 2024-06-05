using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using template.net8.Api.Business.Messages;
using template.net8.Api.Core.Attributes;
using template.net8.Api.Logger;

namespace template.net8.Api.Settings.Handlers;

[CoreLibrary]
internal sealed class GlobalExceptionHandlerControl(
    IProblemDetailsService problemDetailsService,
    ILogger<ControllerBase> logger)
    : IExceptionHandler
{
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IProblemDetailsService _problemDetailsService =
        problemDetailsService ?? throw new ArgumentNullException(nameof(problemDetailsService));

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        //TODO: REVIEW THIS, maybe this logic is do it by the default middleware
        //OperationCanceled by the client, Hungry API eat exception ñam ñam ñam 
        if (exception is OperationCanceledException)
            return true;
        //TODO: Add exception handling specific, read https://adnanrafiq.com/blog/a-complete-guide-to-all-asp-dot-net-builtin-middlewares-part3/
        _logger.LogExceptionGeneral(exception.ToString());
        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails =
            {
                Title = MessageDefinitions.GenericServerError,
                Detail = exception.Message,
                Type = "https://tools.ietf.org/html/rfc9110#name-500-internal-server-error"
            },
            Exception = exception
        }).ConfigureAwait(false);
    }
}