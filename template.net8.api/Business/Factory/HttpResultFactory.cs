using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using template.net8.api.Business.Messages;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Business.Factory;

[CoreLibrary]
internal static class HttpResultFactory
{
    private const string ValidationDetailErrorMsg =
        "Check the described errors and relaunch the request with the correct values.";

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
        ValidationException exception, IFeatureCollection features)
    {
        var httpStatusCode = HttpResultUtils.GetStatusCode(exception);
        return CreateDynamicResult(httpStatusCode, exception, features);
    }

    private static BadRequestResult CreateDynamicResult(HttpStatusCode httpStatusCode, Exception ex,
        IFeatureCollection features)
    {
        return httpStatusCode switch
        {
            HttpStatusCode.BadRequest when ex is ValidationException vex => CreateBadRequestResult(vex, features),
            HttpStatusCode.BadRequest => CreateBadRequestResult(ex, features),
            HttpStatusCode.Unauthorized when ex is ValidationException vex => CreateUnauthorizedResult(vex, features),
            HttpStatusCode.Unauthorized => CreateUnauthorizedResult(ex, features),
            HttpStatusCode.Forbidden when ex is ValidationException vex => CreateForbiddenResult(vex, features),
            HttpStatusCode.Forbidden => CreateForbiddenResult(ex, features),
            HttpStatusCode.NotFound when ex is ValidationException vex => CreateNotFoundResult(vex, features),
            HttpStatusCode.NotFound => CreateNotFoundResult(ex, features),
            HttpStatusCode.Conflict when ex is ValidationException vex => CreateConflictResult(vex, features),
            HttpStatusCode.Conflict => CreateConflictResult(ex, features),
            HttpStatusCode.Gone when ex is ValidationException vex => CreateGoneResult(vex, features),
            HttpStatusCode.Gone => CreateGoneResult(ex, features),
            HttpStatusCode.RequestTimeout when ex is ValidationException vex => CreateRequestTimeoutResult(vex,
                features),
            HttpStatusCode.RequestTimeout => CreateRequestTimeoutResult(ex, features),
            HttpStatusCode.UnprocessableEntity when ex is ValidationException vex => CreateValidationErrorResult(vex,
                features),
            HttpStatusCode.UnprocessableEntity => CreateUnprocessableEntityResult(ex, features),
            HttpStatusCode.InternalServerError when ex is ValidationException vex => CreateBusinessResult(vex,
                features),
            HttpStatusCode.InternalServerError => CreateBusinessResult(ex, features),
            _ => throw new NotSupportedException($"Mapper for http status code {httpStatusCode} is not supported.")
        };
    }

    internal static BadRequestResult CreateBadRequestResult(Exception exception, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = "Incorrect Inputs.",
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-400-bad-request",
            Status = StatusCodes.Status400BadRequest
        };
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateBadRequestResult(ValidationException exception, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = "Incorrect Inputs.",
            Detail = ValidationDetailErrorMsg,
            Type = "https://tools.ietf.org/html/rfc9110#name-400-bad-request",
            Status = StatusCodes.Status400BadRequest
        };
        clientProblemDetails = HttpResultUtils.AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    internal static BadRequestResult CreateUnauthorizedResult(Exception exception, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = "Unauthorized Inputs.",
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-401-unauthorized",
            Status = StatusCodes.Status401Unauthorized
        };
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateUnauthorizedResult(ValidationException exception, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = "Unauthorized Access.",
            Detail = ValidationDetailErrorMsg,
            Type = "https://tools.ietf.org/html/rfc9110#name-401-unauthorized",
            Status = StatusCodes.Status401Unauthorized
        };
        clientProblemDetails = HttpResultUtils.AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    internal static BadRequestResult CreateForbiddenResult(Exception exception, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = "Forbidden Access.",
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-403-forbidden",
            Status = StatusCodes.Status403Forbidden
        };
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateForbiddenResult(ValidationException exception, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = "Forbidden Access.",
            Detail = ValidationDetailErrorMsg,
            Type = "https://tools.ietf.org/html/rfc9110#name-403-forbidden",
            Status = StatusCodes.Status403Forbidden
        };
        clientProblemDetails = HttpResultUtils.AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    internal static BadRequestResult CreateNotFoundResult(Exception exception, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = "Data not Found.",
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-404-not-found",
            Status = StatusCodes.Status404NotFound
        };
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateNotFoundResult(ValidationException exception, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = "Data not Found.",
            Detail = ValidationDetailErrorMsg,
            Type = "https://tools.ietf.org/html/rfc9110#name-404-not-found",
            Status = StatusCodes.Status404NotFound
        };
        clientProblemDetails = HttpResultUtils.AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    internal static BadRequestResult CreateConflictResult(Exception exception, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = "Operation Not Allowed",
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-409-conflict",
            Status = StatusCodes.Status409Conflict
        };
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateConflictResult(ValidationException exception, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = "Operation Not Allowed",
            Detail = ValidationDetailErrorMsg,
            Type = "https://tools.ietf.org/html/rfc9110#name-409-conflict",
            Status = StatusCodes.Status409Conflict
        };
        clientProblemDetails = HttpResultUtils.AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    internal static BadRequestResult CreateRequestTimeoutResult(Exception exception, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = "Request Timeout. Try Again",
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-408-request-timeout",
            Status = StatusCodes.Status408RequestTimeout
        };
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateRequestTimeoutResult(ValidationException exception,
        IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = "Request Timeout. Try Again",
            Detail = ValidationDetailErrorMsg,
            Type = "https://tools.ietf.org/html/rfc9110#name-408-request-timeout",
            Status = StatusCodes.Status408RequestTimeout
        };
        clientProblemDetails = HttpResultUtils.AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    internal static BadRequestResult CreateGoneResult(Exception exception, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = "Expired Data",
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-410-gone",
            Status = StatusCodes.Status410Gone
        };
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateGoneResult(ValidationException exception, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = "Expired Data",
            Detail = ValidationDetailErrorMsg,
            Type = "https://tools.ietf.org/html/rfc9110#name-410-gone",
            Status = StatusCodes.Status410Gone
        };
        clientProblemDetails = HttpResultUtils.AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateValidationErrorResult(
        ValidationException exception, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = "One or more validation errors occurred.",
            Detail = ValidationDetailErrorMsg,
            Type = "https://tools.ietf.org/html/rfc9110#name-422-unprocessable-content",
            Status = StatusCodes.Status422UnprocessableEntity
        };
        clientProblemDetails = HttpResultUtils.AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    internal static BadRequestResult CreateUnprocessableEntityResult(Exception exception,
        IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = "Data inconsistency",
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-422-unprocessable-content",
            Status = StatusCodes.Status422UnprocessableEntity
        };
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    internal static BadRequestResult CreateBusinessResult(Exception exception,
        IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = MessageDefinitions.GenericServerError,
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-500-internal-server-error",
            Status = StatusCodes.Status500InternalServerError
        };
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateBusinessResult(ValidationException exception, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = MessageDefinitions.GenericServerError,
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-500-internal-server-error",
            Status = StatusCodes.Status500InternalServerError
        };
        clientProblemDetails = HttpResultUtils.AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }
}