using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using template.net8.api.Business.Extensions;
using template.net8.api.Core.Factory;
using template.net8.api.Localize.Resources;

namespace template.net8.api.Business.Factory;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class HttpResultFactory
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static BadRequestResult CreateValidationResult(
        ValidationException vex, IStringLocalizer<ResourceMain> localizer, IFeatureCollection features)
    {
        var httpStatusCode = HttpResultUtils.GetStatusCode(vex);
        return ManageValidationResultCreation(httpStatusCode, vex, localizer, features);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static BadRequestResult ManageValidationResultCreation(HttpStatusCode httpStatusCode,
        ValidationException vex,
        IStringLocalizer<ResourceMain> localizer, IFeatureCollection features)
    {
        return httpStatusCode switch
        {
            HttpStatusCode.BadRequest or HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden
                or HttpStatusCode.NotFound or HttpStatusCode.RequestTimeout or HttpStatusCode.Conflict
                or HttpStatusCode.Gone or HttpStatusCode.UnprocessableEntity or HttpStatusCode.InternalServerError
                or HttpStatusCode.NotImplemented => CreateValidationResult(httpStatusCode, vex, localizer, features),
            _ => throw new NotSupportedException(localizer["MapperExceptionStatusCodeNotSupported"])
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static BadRequestResult CreateBadRequestResult(Exception exception,
        IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsBadRequest(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static BadRequestResult CreateValidationResult(HttpStatusCode statusCode, ValidationException exception,
        IStringLocalizer<ResourceMain> localizer, IFeatureCollection features)
    {
        var clientProblemDetails =
            ProblemDetailsFactoryCore.CreateProblemDetailsByHttpStatusCode(statusCode, exception, localizer);
        clientProblemDetails.AddErrors(localizer, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static BadRequestResult CreateUnauthorizedResult(Exception exception,
        IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsUnauthorized(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static BadRequestResult CreateForbiddenResult(Exception exception,
        IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsForbidden(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static BadRequestResult CreateNotFoundResult(Exception exception, IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsNotFound(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static BadRequestResult CreateRequestTimeoutResult(Exception exception,
        IStringLocalizer<ResourceMain> localizer, IFeatureCollection features)
    {
        var clientProblemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsRequestTimeout(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static BadRequestResult CreateConflictResult(Exception exception, IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsConflict(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static BadRequestResult CreateGoneResult(Exception exception, IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsGone(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static BadRequestResult CreateUnprocessableEntityResult(Exception exception,
        IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails =
            ProblemDetailsFactoryCore.CreateProblemDetailsUnprocessableEntity(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static BadRequestResult CreateInternalServerErrorResult(Exception exception,
        IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails =
            ProblemDetailsFactoryCore.CreateProblemDetailsInternalServerError(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static BadRequestResult CreateNotImplementedResult(Exception exception,
        IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails =
            ProblemDetailsFactoryCore.CreateProblemDetailsNotImplemented(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }
}