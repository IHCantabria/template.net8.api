using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using template.net8.api.Controllers.Extensions;
using template.net8.api.Core;
using template.net8.api.Core.Contracts;
using template.net8.api.Core.DTOs;
using template.net8.api.Core.Timeout;
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
[SwaggerTag(SwaggerDocumentation.System.ControllerDescription)]
[Route(ApiRoutes.SystemController.PathController)]
[ApiController]
public sealed class ApplicationSystem(
    IMediator mediator,
    IStringLocalizer<ResourceMain> localizer,
    ILogger<ApplicationSystem> logger)
    : MyControllerBase(mediator, localizer, logger)
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    [HttpGet]
    [DevSwagger]
    [RequestTimeout(RequestConstants.RequestQueryGenericPolicy)]
    [Route(ApiRoutes.SystemController.GetErrorCodes)]
    [SwaggerOperation(
        Summary = SwaggerDocumentation.System.GetErrorCodes.Summary,
        Description =
            SwaggerDocumentation.System.GetErrorCodes.Description,
        OperationId = SwaggerDocumentation.System.GetErrorCodes.Id
    )]
    [SwaggerResponse(StatusCodes.Status200OK, SwaggerDocumentation.System.GetErrorCodes.Ok,
        typeof(IEnumerable<ErrorCodeResource>), MediaTypeNames.Application.Json)]
    public Task<IActionResult> GetErrorCodesAsync(IStringLocalizer<ResourceDictionaryErrorCode> localizer)
    {
        var resources = localizer.GetAllStrings().Filter(static s =>
                s.Name.StartsWith(CoreConstants.ApiErrorCodesPrefix, StringComparison.Ordinal))
            .ToList();
        var result = new LanguageExt.Common.Result<IEnumerable<LocalizedString>>(resources);
        var action =
            ActionResultPayload<IEnumerable<LocalizedString>, IEnumerable<ErrorCodeResource>>.Ok(static obj =>
                ErrorCodeResource.ToCollection(obj.ToList()));
        return Task.FromResult(result.ToActionResult(this, action, Localizer));
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [HttpGet]
    [RequestTimeout(RequestConstants.RequestQueryGenericPolicy)]
    [Route(ApiRoutes.SystemController.GetVersion)]
    [SwaggerOperation(
        Summary = SwaggerDocumentation.System.GetVersion.Summary,
        Description =
            SwaggerDocumentation.System.GetVersion.Description,
        OperationId = SwaggerDocumentation.System.GetVersion.Id
    )]
    [SwaggerResponse(StatusCodes.Status200OK, SwaggerDocumentation.System.GetVersion.Ok,
        typeof(VersionResource), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetVersionAsync(CancellationToken cancellationToken)
    {
        var query = new QueryGetVersion();
        var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
        var action = ActionResultPayload<VersionDto, VersionResource>.Ok(static obj => obj);
        return result.ToActionResult(this, action, Localizer);
    }
}