using System.Net.Mime;
using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using template.net8.api.Business;
using template.net8.api.Controllers.Extensions;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Contracts;
using template.net8.api.Core.DTOs;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Timeout;
using template.net8.api.Features.Querys;
using template.net8.api.Localize.Resources;
using template.net8.api.Settings.Attributes;

namespace template.net8.api.Controllers.V1;

/// <summary>
///     Application System Controller
/// </summary>
[SwaggerTag(SwaggerDocumentation.System.ControllerDescription)]
[Route(ApiRoutes.System.PathController)]
[ApiController]
[CoreLibrary]
public sealed class ApplicationSystem(
    IMediator mediator,
    IStringLocalizer<Resource> localizer,
    ILogger<ApplicationSystem> logger)
    : MyControllerBase(mediator, localizer, logger)
{
    /// <summary>
    ///     Get the Application Code Errors.
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
    /// <exception cref="ArgumentException">
    ///     <paramref>
    ///         <name>comparisonType</name>
    ///     </paramref>
    ///     is not a <see cref="StringComparison" /> value.
    /// </exception>
    [HttpGet]
    [DevSwagger]
    [RequestTimeout(RequestConstants.RequestQueryGenericPolicy)]
    [Route(ApiRoutes.System.GetErrorCodes)]
    [SwaggerOperation(
        Summary = SwaggerDocumentation.System.GetErrorCodes.Summary,
        Description =
            SwaggerDocumentation.System.GetErrorCodes.Description,
        OperationId = SwaggerDocumentation.System.GetErrorCodes.Id
    )]
    [SwaggerResponse(StatusCodes.Status200OK, SwaggerDocumentation.System.GetErrorCodes.Ok,
        typeof(IEnumerable<ErrorCodeResource>), MediaTypeNames.Application.Json)]
    public Task<IActionResult> GetErrorCodes(CancellationToken cancellationToken)
    {
        var resources = Localizer.GetAllStrings().Filter(s =>
                s.Name.StartsWith(BusinessConstants.ApiErrorCodesPrefix, StringComparison.Ordinal))
            .ToList();
        var result = new Result<IEnumerable<LocalizedString>>(resources);
        var action =
            new ActionResultPayload<IEnumerable<LocalizedString>, IEnumerable<ErrorCodeResource>>(obj =>
                ErrorCodeResource.ToCollection(obj.ToList()));
        return Task.FromResult(result.ToActionResult(action, Localizer, HttpContext.Features));
    }

    /// <summary>
    ///     Get Version.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="CoreException">
    ///     Error Creating the Http Action Result. Error mapping action endpoint response to
    ///     resource
    /// </exception>
    [HttpGet]
    [RequestTimeout(RequestConstants.RequestQueryGenericPolicy)]
    [Route(ApiRoutes.System.GetVersion)]
    [SwaggerOperation(
        Summary = SwaggerDocumentation.System.GetVersion.Summary,
        Description =
            SwaggerDocumentation.System.GetVersion.Description,
        OperationId = SwaggerDocumentation.System.GetVersion.Id
    )]
    [SwaggerResponse(StatusCodes.Status200OK, SwaggerDocumentation.System.GetVersion.Ok,
        typeof(VersionResource), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetVersion(CancellationToken cancellationToken)
    {
        var query = new QueryGetVersion();
        var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
        var action =
            new ActionResultPayload<VersionDto, VersionResource>(
                obj => obj);
        return result.ToActionResult(action, Localizer, HttpContext.Features);
    }
}