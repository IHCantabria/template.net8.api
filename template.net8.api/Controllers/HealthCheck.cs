using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using template.net8.api.Controllers.Extensions;
using template.net8.api.Core.Contracts;
using template.net8.api.Core.DTOs;
using template.net8.api.Features.Querys;
using template.net8.api.Localize.Resources;
using template.net8.api.Settings.Attributes;

namespace template.net8.api.Controllers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Controllers must remain public to allow OpenAPI discovery and correct API exposure.")]
[Route(ApiRoutes.HealthController.PathController)]
[ApiController]
[DevSwagger]
public sealed class Health(
    IMediator mediator,
    IStringLocalizer<ResourceMain> localizer,
    ILogger<Health> logger)
    : MyControllerBase(mediator, localizer, logger)
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [HttpGet]
    [Route(ApiRoutes.HealthController.HealthCheck)]
    public async Task<IActionResult> HealthCheckAsync(CancellationToken cancellationToken)
    {
        var query = new QueryCheckStatus();
        var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
        var action = ActionResultPayload<InfoDto, InfoResource>.Ok(static obj => obj);
        return result.ToActionResult(this, action, Localizer);
    }
}