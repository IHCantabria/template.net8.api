using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using template.net8.api.Business.Factory;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Business.Exceptions;

[CoreLibrary]
internal enum ExceptionType
{
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    RequestTimeout = 408,
    Conflict = 409,
    Gone = 410,
    Validation,
    UnprocessableEntity = 422,
    Business = 500,

    NoImplemented
    // Add more exception types as needed
}

[CoreLibrary]
internal static class BusinessExceptionMapper
{
    private static readonly Dictionary<ExceptionType, Func<Exception, IFeatureCollection, IActionResult>>
        ActionResultHandlers = new()
        {
            {
                ExceptionType.BadRequest,
                (ex, features) => HttpResultFactory.CreateBadRequestResult((BadRequestException)ex, features)
            },
            {
                ExceptionType.Unauthorized,
                (ex, features) => HttpResultFactory.CreateUnauthorizedResult((UnauthorizedException)ex, features)
            },
            {
                ExceptionType.Forbidden,
                (ex, features) => HttpResultFactory.CreateForbiddenResult((ForbiddenException)ex, features)
            },
            {
                ExceptionType.NotFound,
                (ex, features) => HttpResultFactory.CreateNotFoundResult((NotFoundException)ex, features)
            },
            {
                ExceptionType.Conflict,
                (ex, features) => HttpResultFactory.CreateConflictResult((ConflictException)ex, features)
            },
            {
                ExceptionType.RequestTimeout,
                (ex, features) => HttpResultFactory.CreateRequestTimeoutResult((RequestTimeoutException)ex, features)
            },
            { ExceptionType.Gone, (ex, features) => HttpResultFactory.CreateGoneResult((GoneException)ex, features) },
            {
                ExceptionType.Validation,
                (ex, features) => HttpResultFactory.CreateDynamicResult((ValidationException)ex, features)
            },
            {
                ExceptionType.UnprocessableEntity,
                (ex, features) =>
                    HttpResultFactory.CreateUnprocessableEntityResult((UnprocessableEntityException)ex, features)
            },
            {
                ExceptionType.Business,
                (ex, features) =>
                    HttpResultFactory.CreateBusinessResult((BusinessException)ex, features)
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
            { ExceptionType.Unauthorized, HttpStatusCode.Unauthorized },
            { ExceptionType.Forbidden, HttpStatusCode.Forbidden },
            { ExceptionType.NotFound, HttpStatusCode.NotFound },
            { ExceptionType.Conflict, HttpStatusCode.Conflict },
            { ExceptionType.RequestTimeout, HttpStatusCode.RequestTimeout },
            { ExceptionType.Gone, HttpStatusCode.Gone },
            { ExceptionType.Validation, HttpStatusCode.BadRequest },
            { ExceptionType.UnprocessableEntity, HttpStatusCode.UnprocessableEntity },
            { ExceptionType.Business, HttpStatusCode.InternalServerError }
            // Add more exception type mappings as needed
        };

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>key</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
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

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>key</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
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
            BadRequestException => ExceptionType.BadRequest,
            UnauthorizedException => ExceptionType.Unauthorized,
            ForbiddenException => ExceptionType.Forbidden,
            NotFoundException => ExceptionType.NotFound,
            ConflictException => ExceptionType.Conflict,
            RequestTimeoutException => ExceptionType.RequestTimeout,
            GoneException => ExceptionType.Gone,
            ValidationException => ExceptionType.Validation,
            UnprocessableEntityException => ExceptionType.UnprocessableEntity,
            BusinessException => ExceptionType.Business,
            _ => ExceptionType.NoImplemented
        };
    }
}