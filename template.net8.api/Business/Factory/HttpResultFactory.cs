using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using template.net8.api.Core.Attributes;
using template.net8.api.Localize.Interfaces;

namespace template.net8.api.Business.Factory;

[CoreLibrary]
internal static class HttpResultFactory
{
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref>
    ///         <name>index</name>
    ///     </paramref>
    ///     is less than 0.
    ///     -or-
    ///     <paramref>
    ///         <name>index</name>
    ///     </paramref>
    ///     is equal to or greater than
    ///     <see>
    ///         <cref>P:System.Collections.Generic.List`1.Count</cref>
    ///     </see>
    ///     .
    /// </exception>
    internal static BadRequestResult CreateDynamicResult(
        ValidationException exception, IStringLocalizer<IResource> localizer, IFeatureCollection features)
    {
        var httpStatusCode = HttpResultUtils.GetStatusCode(exception);
        return CreateDynamicResult(httpStatusCode, exception, localizer, features);
    }

    private static BadRequestResult CreateDynamicResult(HttpStatusCode httpStatusCode, Exception ex,
        IStringLocalizer<IResource> localizer, IFeatureCollection features)
    {
        return httpStatusCode switch
        {
            HttpStatusCode.BadRequest when ex is ValidationException vex => CreateBadRequestResult(vex, localizer,
                features),
            HttpStatusCode.BadRequest => CreateBadRequestResult(ex, localizer, features),
            HttpStatusCode.Unauthorized when ex is ValidationException vex => CreateUnauthorizedResult(vex, localizer,
                features),
            HttpStatusCode.Unauthorized => CreateUnauthorizedResult(ex, localizer, features),
            HttpStatusCode.Forbidden when ex is ValidationException vex => CreateForbiddenResult(vex, localizer,
                features),
            HttpStatusCode.Forbidden => CreateForbiddenResult(ex, localizer, features),
            HttpStatusCode.NotFound when ex is ValidationException vex =>
                CreateNotFoundResult(vex, localizer, features),
            HttpStatusCode.NotFound => CreateNotFoundResult(ex, localizer, features),
            HttpStatusCode.Conflict when ex is ValidationException vex =>
                CreateConflictResult(vex, localizer, features),
            HttpStatusCode.Conflict => CreateConflictResult(ex, localizer, features),
            HttpStatusCode.Gone when ex is ValidationException vex => CreateGoneResult(vex, localizer, features),
            HttpStatusCode.Gone => CreateGoneResult(ex, localizer, features),
            HttpStatusCode.RequestTimeout when ex is ValidationException vex => CreateRequestTimeoutResult(vex,
                localizer,
                features),
            HttpStatusCode.RequestTimeout => CreateRequestTimeoutResult(ex, localizer, features),
            HttpStatusCode.UnprocessableEntity when ex is ValidationException vex => CreateValidationErrorResult(vex,
                localizer,
                features),
            HttpStatusCode.UnprocessableEntity => CreateUnprocessableEntityResult(ex, localizer, features),
            HttpStatusCode.InternalServerError when ex is ValidationException vex => CreateInternalServerErrorResult(
                vex, localizer,
                features),
            HttpStatusCode.InternalServerError => CreateInternalServerErrorResult(ex, localizer, features),
            _ => throw new NotSupportedException(localizer["MapperExceptionStatusCodeNotSupported"])
        };
    }

    internal static BadRequestResult CreateBadRequestResult(Exception exception,
        IStringLocalizer<IResource> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsBadRequestTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-400-bad-request",
            Status = StatusCodes.Status400BadRequest
        };
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateBadRequestResult(ValidationException exception,
        IStringLocalizer<IResource> localizer, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsBadRequestTitle"],
            Detail = localizer["ProblemDetailsValidationDetail"],
            Type = "https://tools.ietf.org/html/rfc9110#name-400-bad-request",
            Status = StatusCodes.Status400BadRequest
        };
        clientProblemDetails = HttpResultUtils.AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    internal static BadRequestResult CreateUnauthorizedResult(Exception exception,
        IStringLocalizer<IResource> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsUnauthorizedTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-401-unauthorized",
            Status = StatusCodes.Status401Unauthorized
        };
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateUnauthorizedResult(ValidationException exception,
        IStringLocalizer<IResource> localizer, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsUnauthorizedTitle"],
            Detail = localizer["ProblemDetailsValidationDetail"],
            Type = "https://tools.ietf.org/html/rfc9110#name-401-unauthorized",
            Status = StatusCodes.Status401Unauthorized
        };
        clientProblemDetails = HttpResultUtils.AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    internal static BadRequestResult CreateForbiddenResult(Exception exception,
        IStringLocalizer<IResource> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsUnauthorizedTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-403-forbidden",
            Status = StatusCodes.Status403Forbidden
        };
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateForbiddenResult(ValidationException exception,
        IStringLocalizer<IResource> localizer, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsForbiddenTitle"],
            Detail = localizer["ProblemDetailsValidationDetail"],
            Type = "https://tools.ietf.org/html/rfc9110#name-403-forbidden",
            Status = StatusCodes.Status403Forbidden
        };
        clientProblemDetails = HttpResultUtils.AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    internal static BadRequestResult CreateNotFoundResult(Exception exception, IStringLocalizer<IResource> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsNotFoundTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-404-not-found",
            Status = StatusCodes.Status404NotFound
        };
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateNotFoundResult(ValidationException exception,
        IStringLocalizer<IResource> localizer, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsNotFoundTitle"],
            Detail = localizer["ProblemDetailsValidationDetail"],
            Type = "https://tools.ietf.org/html/rfc9110#name-404-not-found",
            Status = StatusCodes.Status404NotFound
        };
        clientProblemDetails = HttpResultUtils.AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    internal static BadRequestResult CreateRequestTimeoutResult(Exception exception,
        IStringLocalizer<IResource> localizer, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsRequestTimeoutTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-408-request-timeout",
            Status = StatusCodes.Status408RequestTimeout
        };
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateRequestTimeoutResult(ValidationException exception,
        IStringLocalizer<IResource> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsRequestTimeoutTitle"],
            Detail = localizer["ProblemDetailsValidationDetail"],
            Type = "https://tools.ietf.org/html/rfc9110#name-408-request-timeout",
            Status = StatusCodes.Status408RequestTimeout
        };
        clientProblemDetails = HttpResultUtils.AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    internal static BadRequestResult CreateConflictResult(Exception exception, IStringLocalizer<IResource> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsConflictTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-409-conflict",
            Status = StatusCodes.Status409Conflict
        };
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateConflictResult(ValidationException exception,
        IStringLocalizer<IResource> localizer, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsConflictTitle"],
            Detail = localizer["ProblemDetailsValidationDetail"],
            Type = "https://tools.ietf.org/html/rfc9110#name-409-conflict",
            Status = StatusCodes.Status409Conflict
        };
        clientProblemDetails = HttpResultUtils.AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    internal static BadRequestResult CreateGoneResult(Exception exception, IStringLocalizer<IResource> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsGoneTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-410-gone",
            Status = StatusCodes.Status410Gone
        };
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateGoneResult(ValidationException exception,
        IStringLocalizer<IResource> localizer, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsGoneTitle"],
            Detail = localizer["ProblemDetailsValidationDetail"],
            Type = "https://tools.ietf.org/html/rfc9110#name-410-gone",
            Status = StatusCodes.Status410Gone
        };
        clientProblemDetails = HttpResultUtils.AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateValidationErrorResult(
        ValidationException exception, IStringLocalizer<IResource> localizer, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsValidationTitle"],
            Detail = localizer["ProblemDetailsValidationDetail"],
            Type = "https://tools.ietf.org/html/rfc9110#name-422-unprocessable-content",
            Status = StatusCodes.Status422UnprocessableEntity
        };
        clientProblemDetails = HttpResultUtils.AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    internal static BadRequestResult CreateUnprocessableEntityResult(Exception exception,
        IStringLocalizer<IResource> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsUnprocessableEntityTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-422-unprocessable-content",
            Status = StatusCodes.Status422UnprocessableEntity
        };
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    internal static BadRequestResult CreateInternalServerErrorResult(Exception exception,
        IStringLocalizer<IResource> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = localizer["GenericServerError"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-500-internal-server-error",
            Status = StatusCodes.Status500InternalServerError
        };
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateInternalServerErrorResult(ValidationException exception,
        IStringLocalizer<IResource> localizer, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = localizer["GenericServerError"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-500-internal-server-error",
            Status = StatusCodes.Status500InternalServerError
        };
        clientProblemDetails = HttpResultUtils.AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }
}