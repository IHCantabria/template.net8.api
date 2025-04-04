﻿using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Factory;
using template.net8.api.Localize.Resources;
using template.net8.api.Logger;

namespace template.net8.api.Settings.Handlers;

[CoreLibrary]
internal sealed class GlobalExceptionHandlerControl(
    IProblemDetailsService problemDetailsService,
    IStringLocalizer<ResourceMain> localizer,
    ILogger<ControllerBase> logger)
    : IExceptionHandler
{
    private readonly IStringLocalizer<ResourceMain> _localizer =
        localizer ?? throw new ArgumentNullException(nameof(localizer));

    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IProblemDetailsService _problemDetailsService =
        problemDetailsService ?? throw new ArgumentNullException(nameof(problemDetailsService));

    /// <summary>
    ///     Try Handle Async
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        //TODO: Add exception handling specific, read https://adnanrafiq.com/blog/a-complete-guide-to-all-asp-dot-net-builtin-middlewares-part3/
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