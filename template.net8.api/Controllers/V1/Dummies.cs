using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using template.net8.api.Contracts;
using template.net8.api.Controllers.Extensions;
using template.net8.api.Core.Contracts;
using template.net8.api.Core.Exceptions;
using template.net8.api.Domain.DTOs;
using template.net8.api.Domain.Persistence.Models;
using template.net8.api.Features.Commands;
using template.net8.api.Features.Querys;
using template.net8.api.Localize.Resources;

namespace template.net8.api.Controllers.V1;

/// <summary>
///     Dummies Controller
/// </summary>
[SwaggerTag(SwaggerDocumentation.Dummies.ControllerDescription)]
[Route(ApiRoutes.Dummies.PathController)]
[ApiController]
public sealed class Dummies(
    IMediator mediator,
    IStringLocalizer<Resource> localizer,
    ILogger<Dummies> logger)
    : MyControllerBase(mediator, localizer, logger)
{
    /// <summary>
    ///     Get the Dummies.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="CoreException">
    ///     Error Creating the Http Action Result. Error mapping action endpoint response to
    ///     resource
    /// </exception>
    [HttpGet]
    [Route(ApiRoutes.Dummies.GetDummies)]
    [SwaggerOperation(
        Summary = SwaggerDocumentation.Dummies.GetDummies.Summary,
        Description =
            SwaggerDocumentation.Dummies.GetDummies.Description,
        OperationId = SwaggerDocumentation.Dummies.GetDummies.Id
    )]
    [SwaggerResponse(StatusCodes.Status200OK, SwaggerDocumentation.Dummies.GetDummies.Ok,
        typeof(IEnumerable<DummyResource>), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetDummies(CancellationToken cancellationToken)
    {
        var query = new QueryGetDummies();
        var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
        var action =
            new ActionResultPayload<IEnumerable<DummyDto>, IEnumerable<DummyResource>>(obj =>
                DummyDto.ToCollection(obj.ToList()));
        return result.ToActionResult(action, Localizer, HttpContext.Features);
    }


    /// <summary>
    ///     Get Dummy.
    /// </summary>
    /// <param name="queryParams"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="CoreException">
    ///     Error Creating the Http Action Result. Error mapping action endpoint response to
    ///     resource
    /// </exception>
    [HttpGet]
    [Route(ApiRoutes.Dummies.GetDummy)]
    [SwaggerOperation(
        Summary = SwaggerDocumentation.Dummies.GetDummy.Summary,
        Description =
            SwaggerDocumentation.Dummies.GetDummy.Description,
        OperationId = SwaggerDocumentation.Dummies.GetDummy.Id
    )]
    [SwaggerResponse(StatusCodes.Status200OK, SwaggerDocumentation.Dummies.GetDummy.Ok,
        typeof(DummyResource), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        SwaggerDocumentation.Dummies.GetDummy.BadRequest,
        typeof(BadRequestProblemDetailsResource), MediaTypeNames.Application.ProblemJson)]
    [SwaggerResponse(StatusCodes.Status404NotFound,
        SwaggerDocumentation.Dummies.GetDummy.NotFound,
        typeof(NotFoundProblemDetailsResource), MediaTypeNames.Application.ProblemJson)]
    public async Task<IActionResult> GetDummy([FromRoute] QueryGetDummyParamsResource queryParams,
        CancellationToken cancellationToken)
    {
        QueryGetDummyParamsDto paramsDto = queryParams;
        var query = new QueryGetDummy(paramsDto);
        var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
        var action = new ActionResultPayload<DummyDto, DummyResource>(obj => obj);
        return result.ToActionResult(action, Localizer, HttpContext.Features);
    }

    /// <summary>
    ///     Create Dummy.
    /// </summary>
    /// <param name="commandParams"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="CoreException">
    ///     Error Creating the Http Action Result. Error mapping action endpoint response to
    ///     resource
    /// </exception>
    [HttpPost]
    [Route(ApiRoutes.Dummies.CreateDummy)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = SwaggerDocumentation.Dummies.CreateDummy.Summary,
        Description =
            SwaggerDocumentation.Dummies.CreateDummy.Description,
        OperationId = SwaggerDocumentation.Dummies.CreateDummy.Id
    )]
    [SwaggerResponse(StatusCodes.Status200OK, SwaggerDocumentation.Dummies.CreateDummy.Ok,
        typeof(DummyCreatedResource), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity,
        SwaggerDocumentation.Dummies.CreateDummy.UnprocessableEntity,
        typeof(RequestTimeoutProblemDetailsResource), MediaTypeNames.Application.ProblemJson)]
    public async Task<IActionResult> CreateDummy(CommandCreateDummyParamsResource commandParams,
        CancellationToken cancellationToken)
    {
        var query = new CommandCreateDummy(commandParams);
        var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
        var action = new ActionResultPayload<Dummy, DummyCreatedResource>(obj => obj,
            (nameof(Dummies), nameof(GetDummy)), ("Key", "dummy-key"));
        return result.ToActionResult(action, Localizer, HttpContext.Features);
    }
}