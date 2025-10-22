using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Contracts;
using template.net8.api.Localize.Resources;
using template.net8.api.Settings.Attributes;
using template.net8.api.Settings.Options;

namespace template.net8.api.Controllers.V1;

/// <summary>
///     Health Check Controller
/// </summary>
[Route(ApiRoutes.HealthController.PathController)]
[ApiController]
[DevSwagger]
[CoreLibrary]
public sealed class Health(
    IMediator mediator,
    IStringLocalizer<ResourceMain> localizer,
    ILogger<Health> logger)
    : MyControllerBase(mediator, localizer, logger)
{
    /// <summary>
    ///     Health Check.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    [HttpGet]
    [Route(ApiRoutes.HealthController.HealthCheck)]
    public Task<IActionResult> HealthCheckAsync(IOptions<ProjectOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return Task.FromResult<IActionResult>(Ok(new InfoResource
        {
            Status = StatusCodes.Status200OK,
            Version = options.Value.Version
        }));
    }
}