﻿using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using template.net8.api.Core.Attributes;
using template.net8.api.Localize.Resources;

namespace template.net8.api.Core.Factory;

[CoreLibrary]
internal static class ProblemDetailsFactoryCore
{
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static ProblemDetails CreateProblemDetailsByHttpStatusCode(HttpStatusCode httpStatusCode, Exception ex,
        IStringLocalizer<Resource> localizer)
    {
        return httpStatusCode switch
        {
            HttpStatusCode.BadRequest => CreateProblemDetailsBadRequest(ex, localizer),
            HttpStatusCode.Unauthorized => CreateProblemDetailsUnauthorized(ex, localizer),
            HttpStatusCode.Forbidden => CreateProblemDetailsForbidden(ex, localizer),
            HttpStatusCode.NotFound =>
                CreateProblemDetailsNotFound(ex, localizer),
            HttpStatusCode.MethodNotAllowed => CreateProblemDetailsMethodNotAllowed(ex, localizer),
            HttpStatusCode.RequestTimeout => CreateProblemDetailsRequestTimeout(ex, localizer),
            HttpStatusCode.Conflict => CreateProblemDetailsConflict(ex, localizer),
            HttpStatusCode.Gone => CreateProblemDetailsGone(ex, localizer),
            HttpStatusCode.UnsupportedMediaType => CreateProblemDetailsUnsupportedMediaType(ex, localizer),
            HttpStatusCode.UnprocessableEntity => CreateProblemDetailsUnprocessableEntity(ex, localizer),
            HttpStatusCode.TooManyRequests => CreateProblemDetailsTooManyRequest(ex, localizer),
            HttpStatusCode.InternalServerError => CreateProblemDetailsInternalServerError(ex, localizer),
            HttpStatusCode.NotImplemented => CreateProblemDetailsNotImplemented(ex, localizer),
            _ => CreateProblemDetailsInternalServerError(ex, localizer)
        };
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     or
    ///     <paramref>
    ///         <name>keySelector</name>
    ///     </paramref>
    ///     or
    ///     <paramref>
    ///         <name>elementSelector</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    ///     -or-
    ///     <paramref>
    ///         <name>keySelector</name>
    ///     </paramref>
    ///     produces a key that is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <paramref>
    ///         <name>keySelector</name>
    ///     </paramref>
    ///     produces duplicate keys for two elements.
    /// </exception>
    internal static ProblemDetails CreateProblemDetailsBadRequestValidationPayload(ModelStateDictionary modelState,
        IStringLocalizer<Resource> localizer)
    {
        var errors = modelState.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage)
        );
        return IsRootFileFail(modelState.Keys)
            ? CreateProblemDetailsBadRequestValidationJsonMalformed(errors, localizer)
            : CreateProblemDetailsBadRequestValidationJsonInvalid(errors, localizer);
    }

    private static ProblemDetails CreateProblemDetailsBadRequestValidationJsonMalformed(
        Dictionary<string, IEnumerable<string>?> errors,
        IStringLocalizer<Resource> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsValidationTitle"],
            Detail = localizer["ProblemDetailsBadRequestValidationJsonMalformedDetail"],
            Type = "https://tools.ietf.org/html/rfc9110#name-400-bad-request",
            Status = StatusCodes.Status400BadRequest
        };
        problemDetails.Extensions.TryAdd("errors", errors);
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsBadRequestValidationJsonMalformedCode"]);
        return problemDetails;
    }

    private static ProblemDetails CreateProblemDetailsBadRequestValidationJsonInvalid(
        Dictionary<string, IEnumerable<string>?> errors,
        IStringLocalizer<Resource> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsValidationTitle"],
            Detail = localizer["ProblemDetailsBadRequestValidationJsonInvalidDetail"],
            Type = "https://tools.ietf.org/html/rfc9110#name-400-bad-request",
            Status = StatusCodes.Status400BadRequest
        };
        problemDetails.Extensions.TryAdd("errors", errors);
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsBadRequestValidationJsonInvalidCode"]);
        return problemDetails;
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static ProblemDetails CreateProblemDetailsBadRequest(Exception exception,
        IStringLocalizer<Resource> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsBadRequestTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-400-bad-request",
            Status = StatusCodes.Status400BadRequest
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsBadRequestCode"]);
        return problemDetails;
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static ProblemDetails CreateProblemDetailsBadRequestHttpNotSupported(IStringLocalizer<Resource> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsBadRequestHttpNotSupportedTitle"],
            Detail = localizer["ProblemDetailsBadRequestHttpNotSupportedDetail"],
            Type = "https://tools.ietf.org/html/rfc9110#name-400-bad-request",
            Status = StatusCodes.Status500InternalServerError
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsBadRequestHttpNotSupportedCode"]);
        return problemDetails;
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static ProblemDetails CreateProblemDetailsUnauthorized(Exception exception,
        IStringLocalizer<Resource> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsUnauthorizedTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-401-unauthorized",
            Status = StatusCodes.Status401Unauthorized
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsUnauthorizedCode"]);
        return problemDetails;
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static ProblemDetails CreateProblemDetailsForbidden(Exception exception,
        IStringLocalizer<Resource> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsForbiddenTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-403-forbidden",
            Status = StatusCodes.Status403Forbidden
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsForbiddenCode"]);
        return problemDetails;
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static ProblemDetails CreateProblemDetailsNotFound(Exception exception,
        IStringLocalizer<Resource> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsNotFoundTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-404-not-found",
            Status = StatusCodes.Status404NotFound
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsNotFoundCode"]);
        return problemDetails;
    }

    private static ProblemDetails CreateProblemDetailsMethodNotAllowed(Exception exception,
        IStringLocalizer<Resource> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsMethodNotAllowedTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-405-method-not-allowed",
            Status = StatusCodes.Status405MethodNotAllowed
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsMethodNotAllowedCode"]);
        return problemDetails;
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static ProblemDetails CreateProblemDetailsRequestTimeout(Exception exception,
        IStringLocalizer<Resource> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsRequestTimeoutTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-408-request-timeout",
            Status = StatusCodes.Status408RequestTimeout
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsRequestTimeoutCode"]);
        return problemDetails;
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static ProblemDetails CreateProblemDetailsConflict(Exception exception,
        IStringLocalizer<Resource> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsConflictTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-409-conflict",
            Status = StatusCodes.Status409Conflict
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsConflictCode"]);
        return problemDetails;
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static ProblemDetails CreateProblemDetailsGone(Exception exception,
        IStringLocalizer<Resource> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsGoneTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-410-gone",
            Status = StatusCodes.Status410Gone
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsGoneCode"]);
        return problemDetails;
    }

    private static ProblemDetails CreateProblemDetailsUnsupportedMediaType(Exception exception,
        IStringLocalizer<Resource> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsUnsupportedMediaTypeTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-415-unsupported-media-type",
            Status = StatusCodes.Status415UnsupportedMediaType
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsUnsupportedMediaTypeCode"]);
        return problemDetails;
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static ProblemDetails CreateProblemDetailsUnprocessableEntity(Exception exception,
        IStringLocalizer<Resource> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsUnprocessableEntityTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-422-unprocessable-content",
            Status = StatusCodes.Status422UnprocessableEntity
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsUnprocessableEntityCode"]);
        return problemDetails;
    }

    private static ProblemDetails CreateProblemDetailsTooManyRequest(Exception exception,
        IStringLocalizer<Resource> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsTooManyRequestTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc6585#section-4",
            Status = StatusCodes.Status429TooManyRequests
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsTooManyRequestCode"]);
        return problemDetails;
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static ProblemDetails CreateProblemDetailsInternalServerError(Exception exception,
        IStringLocalizer<Resource> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsInternalServerErrorTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-500-internal-server-error",
            Status = StatusCodes.Status500InternalServerError
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsInternalServerErrorCode"]);
        return problemDetails;
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static ProblemDetails CreateProblemDetailsNotImplemented(Exception exception,
        IStringLocalizer<Resource> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsNotImplementedTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-501-not-implemented",
            Status = StatusCodes.Status501NotImplemented
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsNotImplementedCode"]);
        return problemDetails;
    }

    private static bool IsRootFileFail(ModelStateDictionary.KeyEnumerable keys)
    {
        return keys.Contains("$");
    }
}