using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using template.net8.api.Contracts;
using template.net8.api.Controllers.Extensions;
using template.net8.api.Core.Contracts;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Timeout;
using template.net8.api.Domain.DTOs;
using template.net8.api.Domain.Persistence.Models;
using template.net8.api.Features.Commands;
using template.net8.api.Features.Querys;
using template.net8.api.Hubs;
using template.net8.api.Localize.Resources;
using template.net8.api.Settings.Options;

namespace template.net8.api.Controllers.V1;

/// <summary>
///     Dummies Controller
/// </summary>
[SwaggerTag(SwaggerDocumentation.Dummies.ControllerDescription)]
[Route(ApiRoutes.DummiesController.PathController)]
[ApiController]
public sealed class Dummies(
    IMediator mediator,
    IStringLocalizer<ResourceMain> localizer,
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
    [RequestTimeout(RequestConstants.RequestQueryGenericPolicy)]
    [Route(ApiRoutes.DummiesController.GetDummies)]
    [SwaggerOperation(
        Summary = SwaggerDocumentation.Dummies.GetDummies.Summary,
        Description =
            SwaggerDocumentation.Dummies.GetDummies.Description,
        OperationId = SwaggerDocumentation.Dummies.GetDummies.Id
    )]
    [SwaggerResponse(StatusCodes.Status200OK, SwaggerDocumentation.Dummies.GetDummies.Ok,
        typeof(IEnumerable<DummyResource>), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetDummiesAsync(CancellationToken cancellationToken)
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
    [RequestTimeout(RequestConstants.RequestQueryGenericPolicy)]
    [Route(ApiRoutes.DummiesController.GetDummy)]
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
    public async Task<IActionResult> GetDummyAsync([FromRoute] QueryGetDummyParamsResource queryParams,
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
    [RequestTimeout(RequestConstants.RequestCommandGenericPolicy)]
    [Route(ApiRoutes.DummiesController.CreateDummy)]
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
    public async Task<IActionResult> CreateDummyAsync(CommandCreateDummyParamsResource commandParams,
        CancellationToken cancellationToken)
    {
        var query = new CommandCreateDummy(commandParams);
        var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
        var action = new ActionResultPayload<Dummy, DummyCreatedResource>(obj => obj,
            (nameof(Dummies), nameof(GetDummyAsync)), ("Key", "dummy-key"));
        return result.ToActionResult(action, Localizer, HttpContext.Features);
    }

    /// <summary>
    ///     Get Earthquakes Events Async
    /// </summary>
    /// <param name="config"></param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns></returns>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="CoreException">
    ///     Error Creating the Http Action Result. Error mapping action endpoint response to
    ///     resource
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    [HttpGet]
    [RequestTimeout(RequestConstants.RequestQueryGenericPolicy)]
    [Route(ApiRoutes.DummiesController.Hubs)]
    [SwaggerOperation(
        Summary = SwaggerDocumentation.Dummies.GetDummy.Summary,
        Description =
            SwaggerDocumentation.Dummies.GetDummy.Description,
        OperationId = SwaggerDocumentation.Dummies.GetDummy.Id
    )]
    [SwaggerResponse(StatusCodes.Status200OK, SwaggerDocumentation.Dummies.GetDummy.Ok,
        typeof(IEnumerable<HubEventResource>), MediaTypeNames.Application.Json)]
    public Task<IActionResult> GetEarthquakesEventsAsync(IOptions<SwaggerOptions> config,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(config);
        var fullUrl = config.Value.ServerUrl.OriginalString + ApiRoutes.DummiesHub.PathHub;

        var eventList = new List<HubEventResource>
        {
            new()
            {
                Name = HubsDocumentation.Dummy.CheckConnectionStatus.Name, Path = fullUrl,
                Type = HubsDocumentation.Dummy.CheckConnectionStatus.Type,
                Description = Localizer["CreateDummyValidatorTextInvalidMsg"], // TODO:FIX
                Fields = []
            },
            new()
            {
                Name = HubsDocumentation.Dummy.ConnectionStatus.Name, Path = fullUrl,
                Type = HubsDocumentation.Dummy.ConnectionStatus.Type,
                Description = Localizer["CreateDummyValidatorTextInvalidMsg"], // TODO:FIX
                Fields = HubsDocumentation.Dummy.ConnectionStatus.Fields
            },
            new()
            {
                Name = HubsDocumentation.Dummy.ConnectionOnline.Name, Path = fullUrl,
                Type = HubsDocumentation.Dummy.ConnectionOnline.Type,
                Description = Localizer["CreateDummyValidatorTextInvalidMsg"], // TODO:FIX
                Fields = HubsDocumentation.Dummy.ConnectionOnline.Fields
            },
            new()
            {
                Name = HubsDocumentation.Dummy.NewDummy.Name, Path = fullUrl,
                Type = HubsDocumentation.Dummy.NewDummy.Type,
                Description = Localizer["CreateDummyValidatorTextInvalidMsg"], // TODO:FIX
                Fields = HubsDocumentation.Dummy.NewDummy.Fields
            }
        };
        var result = new LanguageExt.Common.Result<IEnumerable<HubEventResource>>(eventList);
        var action =
            new ActionResultPayload<IEnumerable<HubEventResource>, IEnumerable<HubEventResource>>(obj =>
                obj);
        return Task.FromResult(result.ToActionResult(action, Localizer, HttpContext.Features));
    }
}