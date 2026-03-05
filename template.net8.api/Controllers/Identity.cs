using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using template.net8.api.Contracts;
using template.net8.api.Controllers.Extensions;
using template.net8.api.Core.Contracts;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Timeout;
using template.net8.api.Domain.DTOs;
using template.net8.api.Domain.Extensions;
using template.net8.api.Features.Querys;
using template.net8.api.Localize.Resources;

namespace template.net8.api.Controllers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Controllers must remain public to allow OpenAPI discovery and correct API exposure.")]
[SwaggerTag(SwaggerDocumentation.Identity.ControllerDescription)]
[Route(ApiRoutes.IdentityController.PathController)]
[ApiController]
public sealed class Identity(
    IMediator mediator,
    IStringLocalizer<ResourceMain> localizer,
    ILogger<Identity> logger)
    : MyControllerBase(mediator, localizer, logger)
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [HttpPost]
    [RequestTimeout(RequestConstants.RequestCommandGenericPolicy)]
    [Route(ApiRoutes.IdentityController.Login)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = SwaggerDocumentation.Identity.Login.Summary,
        Description =
            SwaggerDocumentation.Identity.Login.Description,
        OperationId = SwaggerDocumentation.Identity.Login.Id
    )]
    [SwaggerResponse(StatusCodes.Status200OK, SwaggerDocumentation.Identity.Login.Ok,
        typeof(IdTokenResource), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        SwaggerDocumentation.Identity.Login.BadRequest,
        typeof(BadRequestProblemDetailsResource), MediaTypeNames.Application.ProblemJson)]
    [SwaggerResponse(StatusCodes.Status403Forbidden,
        SwaggerDocumentation.Identity.Login.Forbidden,
        typeof(ForbiddenProblemDetailsResource), MediaTypeNames.Application.ProblemJson)]
    [SwaggerResponse(StatusCodes.Status404NotFound,
        SwaggerDocumentation.Identity.Login.NotFound,
        typeof(NotFoundProblemDetailsResource), MediaTypeNames.Application.ProblemJson)]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity,
        SwaggerDocumentation.Identity.Login.UnprocessableEntity,
        typeof(UnprocessableEntityProblemDetailsResource), MediaTypeNames.Application.ProblemJson)]
    public async Task<IActionResult> LoginUserAsync(QueryLoginUserParamsResource queryParams,
        CancellationToken cancellationToken)
    {
        var query = new QueryLoginUser(queryParams);
        var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
        var action = ActionResultPayload<IdTokenDto, IdTokenResource>.Ok(static obj => obj);
        return result.ToActionResult(this, action, Localizer);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ResultSuccessInvalidOperationException">
    ///     Result is not a success! Use ExtractException method instead
    ///     and Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractException method
    /// </exception>
    [HttpGet]
    [Authorize]
    [RequestTimeout(RequestConstants.RequestCommandGenericPolicy)]
    [Route(ApiRoutes.IdentityController.Access)]
    [SwaggerOperation(
        Summary = SwaggerDocumentation.Identity.Access.Summary,
        Description =
            SwaggerDocumentation.Identity.Access.Description,
        OperationId = SwaggerDocumentation.Identity.Access.Id
    )]
    [SwaggerResponse(StatusCodes.Status200OK, SwaggerDocumentation.Identity.Access.Ok,
        typeof(AccessTokenResource), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        SwaggerDocumentation.Identity.Access.BadRequest,
        typeof(BadRequestProblemDetailsResource), MediaTypeNames.Application.ProblemJson)]
    [SwaggerResponse(StatusCodes.Status403Forbidden,
        SwaggerDocumentation.Identity.Access.Forbidden,
        typeof(ForbiddenProblemDetailsResource), MediaTypeNames.Application.ProblemJson)]
    [SwaggerResponse(StatusCodes.Status404NotFound,
        SwaggerDocumentation.Identity.Access.NotFound,
        typeof(NotFoundProblemDetailsResource), MediaTypeNames.Application.ProblemJson)]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity,
        SwaggerDocumentation.Identity.Access.UnprocessableEntity,
        typeof(UnprocessableEntityProblemDetailsResource), MediaTypeNames.Application.ProblemJson)]
    public async Task<IActionResult> AccessUserAsync(CancellationToken cancellationToken)
    {
        var paramsDto = new QueryAccessUserParamsDto
        {
            Identity = new IdentityDto()
        };
        var query = new QueryAccessUser(paramsDto.AddIdentifier(User));
        var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
        var action = ActionResultPayload<AccessTokenDto, AccessTokenResource>.Ok(static obj => obj);
        return result.ToActionResult(this, action, Localizer);
    }
}