using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using template.net8.api.Controllers.Extensions;
using template.net8.api.Core;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Contracts;
using template.net8.api.Core.DTOs;
using template.net8.api.Core.Timeout;
using template.net8.api.Features.Querys;
using template.net8.api.Localize.Resources;
using template.net8.api.Settings.Attributes;

namespace template.net8.api.Controllers.V1;

/// <summary>
///     Application System Controller
/// </summary>
[SwaggerTag(SwaggerDocumentation.System.ControllerDescription)]
[Route(ApiRoutes.SystemController.PathController)]
[ApiController]
[CoreLibrary]
public sealed class ApplicationSystem(
    IMediator mediator,
    IStringLocalizer<ResourceMain> localizer,
    ILogger<ApplicationSystem> logger)
    : MyControllerBase(mediator, localizer, logger)
{
    /// <summary>
    ///     Get the Application Code Errors.
    /// </summary>
    /// <param name="localizer"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <paramref>
    ///         <name>comparisonType</name>
    ///     </paramref>
    ///     is not a <see cref="StringComparison" /> value.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>value</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
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
        var resources = localizer.GetAllStrings().Filter(s =>
                s.Name.StartsWith(CoreConstants.ApiErrorCodesPrefix, StringComparison.Ordinal))
            .ToList();
        var result = new LanguageExt.Common.Result<IEnumerable<LocalizedString>>(resources);
        var action =
            ActionResultPayload<IEnumerable<LocalizedString>, IEnumerable<ErrorCodeResource>>.Ok(obj =>
                ErrorCodeResource.ToCollection(obj.ToList()));
        return Task.FromResult(result.ToActionResult(this, action, Localizer));
    }

    /// <summary>
    ///     Get Version.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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
        var action = ActionResultPayload<VersionDto, VersionResource>.Ok(obj => obj);
        return result.ToActionResult(this, action, Localizer);
    }
}