using System.Net;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Business.Exceptions;

internal enum ExceptionType
{
    BadRequest,
    NotFound,
    Conflict,
    Gone,
    Validation,
    UnprocessableEntity,

    NoImplemented
    // Add more exception types as needed
}

[CoreLibrary]
internal static class BusinessExceptionMapper
{
    private static readonly Dictionary<ExceptionType, Func<Exception, IFeatureCollection, IActionResult>>
        ActionResultHandlers = new()
        {
            { ExceptionType.BadRequest, (ex, features) => CreateBadRequestResult((BadRequestException)ex, features) },
            { ExceptionType.NotFound, (ex, features) => CreateNotFoundResult((NotFoundException)ex, features) },
            { ExceptionType.Conflict, (ex, features) => CreateConflictResult((ConflictException)ex, features) },
            { ExceptionType.Gone, (ex, features) => CreateGoneResult((GoneException)ex, features) },
            {
                ExceptionType.Validation,
                (ex, features) => CreateDynamicResult((ValidationException)ex, features)
            },
            {
                ExceptionType.UnprocessableEntity,
                (ex, features) => CreateUnprocessableEntityResult((UnprocessableEntityException)ex, features)
            },
            {
                ExceptionType.NoImplemented,
                (_, _) => throw new NotSupportedException("Exception type is not supported.")
            }
            // Add more exception type mappings as needed
        };

    private static readonly Dictionary<ExceptionType, HttpStatusCode>
        HttpStatusCodetHandlers = new()
        {
            { ExceptionType.BadRequest, HttpStatusCode.BadRequest },
            { ExceptionType.NotFound, HttpStatusCode.NotFound },
            { ExceptionType.Conflict, HttpStatusCode.Conflict },
            { ExceptionType.Gone, HttpStatusCode.Gone },
            { ExceptionType.Validation, HttpStatusCode.BadRequest },
            { ExceptionType.UnprocessableEntity, HttpStatusCode.UnprocessableEntity }
            // Add more exception type mappings as needed
        };

    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="NotSupportedException">Condition.</exception>
    internal static IActionResult MapExceptionToResult(Exception ex,
        IFeatureCollection features)
    {
        var exceptionType = GetExceptionType(ex);
        if (ActionResultHandlers.TryGetValue(exceptionType, out var handler))
            return handler(ex, features);
        throw new NotSupportedException($"Mapper for exception type {exceptionType} is not supported.");
    }

    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    /// <exception cref="NotSupportedException">Condition.</exception>
    internal static HttpStatusCode MapExceptionToHttpStatusCode(Exception ex)
    {
        var exceptionType = GetExceptionType(ex);
        if (HttpStatusCodetHandlers.TryGetValue(exceptionType, out var statusCode))
            return statusCode;
        throw new NotSupportedException($"Status Code for exception type {exceptionType} is not supported.");
    }

    /// <summary>
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    private static ExceptionType GetExceptionType(Exception exception)
    {
        return exception switch
        {
            NotFoundException => ExceptionType.NotFound,
            GoneException => ExceptionType.Gone,
            ValidationException => ExceptionType.Validation,
            UnprocessableEntityException => ExceptionType.UnprocessableEntity,
            _ => ExceptionType.NoImplemented
        };
    }

    private static BadRequestResult CreateDynamicResult(
        ValidationException exception, IFeatureCollection features)
    {
        var httpStatusCode = GetStatusCode(exception);
        return CreateDynamicResult(httpStatusCode, exception, features);
    }

    /// <returns></returns>
    private static BadRequestResult CreateDynamicResult(HttpStatusCode httpStatusCode, Exception ex,
        IFeatureCollection features)
    {
        return httpStatusCode switch
        {
            HttpStatusCode.NotFound when ex is ValidationException vex => CreateNotFoundResult(vex, features),
            HttpStatusCode.NotFound => CreateNotFoundResult(ex, features),
            HttpStatusCode.Gone when ex is ValidationException vex => CreateGoneResult(vex, features),
            HttpStatusCode.Gone => CreateGoneResult(ex, features),
            HttpStatusCode.UnprocessableEntity when ex is ValidationException vex => CreateValidationErrorResult(vex,
                features),
            HttpStatusCode.UnprocessableEntity => CreateUnprocessableEntityResult(ex, features),
            _ => throw new NotSupportedException($"Mapper for http status code {httpStatusCode} is not supported.")
        };
    }

    private static BadRequestResult CreateBadRequestResult(Exception exception, IFeatureCollection features)
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
            Detail = "Check the described errors and relaunch the request with the correct values.",
            Type = "https://tools.ietf.org/html/rfc9110#name-400-bad-request",
            Status = StatusCodes.Status400BadRequest
        };
        clientProblemDetails = AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateNotFoundResult(Exception exception, IFeatureCollection features)
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
            Detail = "Check the described errors and relaunch the request with the correct values.",
            Type = "https://tools.ietf.org/html/rfc9110#name-404-not-found",
            Status = StatusCodes.Status404NotFound
        };
        clientProblemDetails = AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateConflictResult(Exception exception, IFeatureCollection features)
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
            Detail = "Check the described errors and relaunch the request with the correct values.",
            Type = "https://tools.ietf.org/html/rfc9110#name-409-conflict",
            Status = StatusCodes.Status409Conflict
        };
        clientProblemDetails = AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateGoneResult(Exception exception, IFeatureCollection features)
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
            Detail = "Check the described errors and relaunch the request with the correct values.",
            Type = "https://tools.ietf.org/html/rfc9110#name-410-gone",
            Status = StatusCodes.Status410Gone
        };
        clientProblemDetails = AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateValidationErrorResult(
        ValidationException exception, IFeatureCollection features)
    {
        var clientProblemDetails = new ProblemDetails
        {
            Title = "One or more validation errors occurred.",
            Detail = "Check the described errors and relaunch the request with the correct values.",
            Type = "https://tools.ietf.org/html/rfc9110#name-422-unprocessable-content",
            Status = StatusCodes.Status422UnprocessableEntity
        };
        clientProblemDetails = AddErrors(clientProblemDetails, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateUnprocessableEntityResult(Exception exception,
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

    private static ProblemDetails AddErrors(ProblemDetails problemDetails, ValidationException vex)
    {
        var errorsCollection = CreateErrorsCollection(vex);
        if (errorsCollection.Count is 1)
        {
            var singleError = errorsCollection[0];
            problemDetails.Detail = singleError.Detail;
            problemDetails.Extensions["value"] = singleError.Value;
            problemDetails.Extensions["pointer"] = singleError.Pointer;
            problemDetails.Extensions["code"] = singleError.Code;
        }
        else
        {
            problemDetails.Extensions["errors"] = errorsCollection;
        }

        return problemDetails;
    }

    private static List<ProblemDetailsValidationError> CreateErrorsCollection(ValidationException validationException)
    {
        var groupedErrors = GroupErrors(validationException.Errors);
        return ConvertToDictionary(groupedErrors);
    }

    private static HttpStatusCode GetStatusCode(ValidationException validationException)
    {
        var errorGroups = GroupErrors(validationException.Errors);
        var errorCodes = GetHttpStatusCodes(errorGroups).ToList();

        return errorCodes.Count == 1 ? errorCodes[0] : HttpStatusCode.UnprocessableEntity;
    }

    private static IEnumerable<IGrouping<string, ValidationFailure>> GroupErrors(IEnumerable<ValidationFailure> errors)
    {
        return errors.GroupBy(error => error.PropertyName);
    }

    private static IEnumerable<HttpStatusCode> GetHttpStatusCodes(
        IEnumerable<IGrouping<string, ValidationFailure>> groups)
    {
        return groups.SelectMany(g => g.Select(vf => vf.CustomState).OfType<HttpStatusCode>()).Distinct();
    }

    private static List<ProblemDetailsValidationError> ConvertToDictionary(
        IEnumerable<IGrouping<string, ValidationFailure>> groupedErrors)
    {
        var errorsList = new List<ProblemDetailsValidationError>();
        //Should be Serial
        foreach (var group in groupedErrors)
        {
            var errors = group.Select(error => new ProblemDetailsValidationError
            {
                Detail = error.ErrorMessage, Code = $"BE-{error.ErrorCode}",
                Value = error.AttemptedValue.ToString(), Pointer = group.Key
            });
            errorsList.AddRange(errors);
        }

        return errorsList;
    }

    private sealed record ProblemDetailsValidationError
    {
        internal required string Detail { get; init; }

        internal required string Pointer { get; init; }

        internal required string? Value { get; init; }

        internal required string Code { get; init; }
    }
}