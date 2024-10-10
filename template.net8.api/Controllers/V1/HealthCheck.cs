using MediatR;
using Microsoft.AspNetCore.Mvc;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Controllers.V1;

/// <summary>
///     Health Check Controller
/// </summary>
[Route(ApiRoutes.Health.PathController)]
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[CoreLibrary]
public sealed class Health(
    IMediator mediator,
    ILogger<Health> logger)
    : MyControllerBase(mediator, logger)
{
    /// <summary>
    ///     Health Check.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route(ApiRoutes.Health.HealthCheck)]
    public Task<IActionResult> HealthCheck()
    {
        return Task.FromResult<IActionResult>(Ok("Service is running"));
    }
}