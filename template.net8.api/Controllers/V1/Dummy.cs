using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using template.net8.Api.Contracts;
using template.net8.Api.Controllers.Extensions;
using template.net8.Api.Domain.DTOs;
using template.net8.Api.Features.Commands;
using template.net8.Api.Features.Querys;

namespace template.net8.Api.Controllers.V1;

/// <summary>
///     Dummy Controller
/// </summary>
[SwaggerTag("Dummy Controller")]
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
        Summary = "Get the Dummies.",
        Description =
            "Get the dummies in the system.",
        OperationId = "GetDummies"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Return the dummies in the system.",
        typeof(IEnumerable<DummyResource>), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status408RequestTimeout,
        "Unable to get the dummies due to a request timeout issue, retry the request.",
        typeof(RequestTimeoutProblemDetailsResource), MediaTypeNames.Application.ProblemJson)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError,
        "Unable to get the dummies due to a server error.",
        typeof(InternalServerProblemDetailsResource), MediaTypeNames.Application.ProblemJson)]
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
        Summary = "Create Dummy.",
        Description =
            "Create a new Dummy in the system.",
        OperationId = "CreateDummy"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Return the dummy created in the system.",
        typeof(DummyResource), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status408RequestTimeout,
        "Unable to create the dummy due to a request timeout issue, retry the request.",
        typeof(RequestTimeoutProblemDetailsResource), MediaTypeNames.Application.ProblemJson)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError,
        "Unable to create the dummy due to a server error.",
        typeof(InternalServerProblemDetailsResource), MediaTypeNames.Application.ProblemJson)]
    public async Task<IActionResult> CreateDummy(CommandDummyCreateParamsResource commandParams,
        CancellationToken cancellationToken)
    {
        var query = new CreateDummyCommand(commandParams);
        var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
        return result.ToActionResult(obj =>
            (DummyResource)obj, HttpContext.Features);
    }
}