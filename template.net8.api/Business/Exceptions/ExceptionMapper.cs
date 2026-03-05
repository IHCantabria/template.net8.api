using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using template.net8.api.Business.Factory;
using template.net8.api.Core.Exceptions;
using template.net8.api.Localize.Resources;
using NotImplementedException = template.net8.api.Core.Exceptions.NotImplementedException;

namespace template.net8.api.Business.Exceptions;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal enum ExceptionType
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    BadRequest = 400,

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Unauthorized = 401,

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Forbidden = 403,

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    NotFound = 404,

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    RequestTimeout = 408,

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Conflict = 409,

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Gone = 410,

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Validation = 601,

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    UnprocessableEntity = 422,

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    InternalServerError = 500,

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    NotImplemented = 501,

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    NotSupported = 602
    // Add more exception types as needed
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class ExceptionMapper
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
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
                ExceptionType.Validation, static (ex, localizer, features) =>
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

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="NotSupportedException">Condition.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static IActionResult MapExceptionToResult(Exception ex, IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var exceptionType = GetExceptionType(ex);
        if (exceptionType is ExceptionType.NotSupported)
            throw new NotSupportedException(localizer["MapperExceptionResultNotSupported", exceptionType]);

        return ActionResultHandlers.TryGetValue(exceptionType, out var handler)
            ? handler(ex, localizer, features)
            : throw new NotSupportedException(localizer["MapperExceptionResultNotSupported", exceptionType]);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
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