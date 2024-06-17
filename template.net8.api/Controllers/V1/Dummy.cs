using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using template.net8.api.Contracts;
using template.net8.api.Controllers.Extensions;
using template.net8.api.Core.Contracts;
using template.net8.api.Domain.DTOs;
using template.net8.api.Features.Commands;
using template.net8.api.Features.Querys;

namespace template.net8.api.Controllers.V1;

/// <summary>
///     Dummy Controller
/// </summary>
[SwaggerTag(SwaggerDocumentation.Dummy.Tag)]
[ApiController]
public sealed class Dummy(
    IMediator mediator,
    ILogger<Dummy> logger)
    : MyControllerBase(mediator, logger)
{
    /// <summary>
    ///     Get the Dummies.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    /// <exception cref="NotSupportedException">Condition.</exception>
    [HttpGet]
    [Route(ApiRoutes.Dummy.GetDummies)]
    [SwaggerOperation(
        Summary = SwaggerDocumentation.Dummy.GetDummies.Summary,
        Description =
            SwaggerDocumentation.Dummy.GetDummies.Description,
        OperationId = SwaggerDocumentation.Dummy.GetDummies.Id
    )]
    [SwaggerResponse(StatusCodes.Status200OK, SwaggerDocumentation.Dummy.GetDummies.Ok,
        typeof(IEnumerable<DummyResource>), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetDummies(CancellationToken cancellationToken)
    {
        var query = new GetDummiesQuery();
        var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
        return result.ToActionResult(obj =>
            DummyDto.ToCollection(obj.ToList()), HttpContext.Features);
    }

    /// <summary>
    ///     Create Dummy.
    /// </summary>
    /// <param name="commandParams"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    /// <exception cref="NotSupportedException">Condition.</exception>
    [HttpPost]
    [Route(ApiRoutes.Dummy.CreateDummy)]
    [SwaggerOperation(
        Summary = SwaggerDocumentation.Dummy.CreateDummy.Summary,
        Description =
            SwaggerDocumentation.Dummy.CreateDummy.Description,
        OperationId = SwaggerDocumentation.Dummy.CreateDummy.Id
    )]
    [SwaggerResponse(StatusCodes.Status200OK, SwaggerDocumentation.Dummy.CreateDummy.Ok,
        typeof(DummyResource), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity,
        SwaggerDocumentation.Dummy.CreateDummy.UnprocessableEntity,
        typeof(RequestTimeoutProblemDetailsResource), MediaTypeNames.Application.ProblemJson)]
    public async Task<IActionResult> CreateDummy(CommandDummyCreateParamsResource commandParams,
        CancellationToken cancellationToken)
    {
        var query = new CreateDummyCommand(commandParams);
        var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
        return result.ToActionResult(obj =>
            (DummyResource)obj, HttpContext.Features);
    }
}