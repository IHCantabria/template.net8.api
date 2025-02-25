﻿using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using template.net8.api.Business.Factory;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Exceptions;
using template.net8.api.Localize.Resources;
using NotImplementedException = template.net8.api.Core.Exceptions.NotImplementedException;

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
    Validation = 601,
    UnprocessableEntity = 422,
    InternalServerError = 500,
    NotImplemented = 501,

    NotSupported = 602
    // Add more exception types as needed
}

[CoreLibrary]
internal static class ExceptionMapper
{
    private static readonly Dictionary<ExceptionType,
            Func<Exception, IStringLocalizer<ResourceMain>, IFeatureCollection, IActionResult>>
        ActionResultHandlers = new()
        {
            {
                ExceptionType.BadRequest,
                HttpResultFactory.CreateBadRequestResult
            },
            {
                ExceptionType.Unauthorized,
                HttpResultFactory.CreateUnauthorizedResult
            },
            {
                ExceptionType.Forbidden,
                HttpResultFactory.CreateForbiddenResult
            },
            {
                ExceptionType.NotFound,
                HttpResultFactory.CreateNotFoundResult
            },
            {
                ExceptionType.Conflict,
                HttpResultFactory.CreateConflictResult
            },
            {
                ExceptionType.RequestTimeout,
                HttpResultFactory.CreateRequestTimeoutResult
            },
            { ExceptionType.Gone, HttpResultFactory.CreateGoneResult },
            {
                ExceptionType.Validation,
                (ex, localizer, features) =>
                    HttpResultFactory.CreateValidationResult((ValidationException)ex, localizer, features)
            },
            {
                ExceptionType.UnprocessableEntity,
                HttpResultFactory.CreateUnprocessableEntityResult
            },
            {
                ExceptionType.InternalServerError,
                HttpResultFactory.CreateInternalServerErrorResult
            },
            {
                ExceptionType.NotImplemented,
                HttpResultFactory.CreateNotImplementedResult
            }
            // Add more exception type mappings as needed
        };

    private static readonly Dictionary<ExceptionType, HttpStatusCode>
        HttpStatusCodeHandlers = new()
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
            { ExceptionType.InternalServerError, HttpStatusCode.InternalServerError },
            { ExceptionType.NotImplemented, HttpStatusCode.NotImplemented }
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
    internal static IActionResult MapExceptionToResult(Exception ex, IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var exceptionType = GetExceptionType(ex);
        if (exceptionType is ExceptionType.NotSupported)
            throw new NotSupportedException(localizer["MapperExceptionResultNotSupported", exceptionType]);

        if (ActionResultHandlers.TryGetValue(exceptionType, out var handler))
            return handler(ex, localizer, features);

        throw new NotSupportedException(localizer["MapperExceptionResultNotSupported", exceptionType]);
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>key</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="NotSupportedException">Condition.</exception>
    internal static HttpStatusCode ExceptionToHttpStatusCode(Exception ex, IStringLocalizer<ResourceMain> localizer)
    {
        var exceptionType = GetExceptionType(ex);
        if (exceptionType is ExceptionType.NotSupported)
            throw new NotSupportedException(localizer["ExceptionStatusCodeNotSupported", exceptionType]);

        if (HttpStatusCodeHandlers.TryGetValue(exceptionType, out var statusCode))
            return statusCode;

        throw new NotSupportedException(localizer["ExceptionStatusCodeNotSupported", exceptionType]);
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
            InternalServerErrorException => ExceptionType.InternalServerError,
            NotImplementedException => ExceptionType.NotImplemented,
            _ => ExceptionType.NotSupported
        };
    }
}