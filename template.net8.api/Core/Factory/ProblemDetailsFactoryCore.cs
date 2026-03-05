using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using template.net8.api.Localize.Resources;

namespace template.net8.api.Core.Factory;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "ReSharper",
    "ClassTooBig",
    Justification =
        "Centralized ProblemDetails factory. The class intentionally groups all HTTP status mappings in one place.")]
internal static class ProblemDetailsFactoryCore
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static ProblemDetails CreateProblemDetailsByHttpStatusCode(HttpStatusCode httpStatusCode, Exception ex,
        IStringLocalizer<ResourceMain> localizer)
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

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static ProblemDetails CreateProblemDetailsBadRequestValidationPayload(ModelStateDictionary modelState,
        IStringLocalizer<ResourceMain> localizer)
    {
        var errors = modelState.ToDictionary(static kvp => kvp.Key,
            static kvp => kvp.Value?.Errors.Select(static e => e.ErrorMessage)
        ).Where(static k => k.Value is not null && k.Value.Any()).ToDictionary();
        return ProblemDetailsFactoryCoreUtils.IsRootFileFail(modelState.Keys)
            ? CreateProblemDetailsBadRequestValidationJsonMalformed(errors, localizer)
            : CreateProblemDetailsBadRequestValidationJsonInvalid(errors, localizer);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    private static ProblemDetails CreateProblemDetailsBadRequestValidationJsonMalformed(
        Dictionary<string, IEnumerable<string>?> errors,
        IStringLocalizer localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsValidationTitle"],
            Detail = localizer["ProblemDetailsBadRequestValidationJsonMalformedDetail"],
            Type = "https://tools.ietf.org/html/rfc9110#name-400-bad-request",
            Status = StatusCodes.Status400BadRequest
        };
        problemDetails.Extensions.TryAdd("errors", errors);
        problemDetails.Extensions.TryAdd("code",
            localizer["ProblemDetailsBadRequestValidationJsonMalformedCode"].Value);
        return problemDetails;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    private static ProblemDetails CreateProblemDetailsBadRequestValidationJsonInvalid(
        Dictionary<string, IEnumerable<string>?> errors,
        IStringLocalizer localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsValidationTitle"],
            Detail = localizer["ProblemDetailsBadRequestValidationJsonInvalidDetail"],
            Type = "https://tools.ietf.org/html/rfc9110#name-400-bad-request",
            Status = StatusCodes.Status400BadRequest
        };
        problemDetails.Extensions.TryAdd("errors", errors);
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsBadRequestValidationJsonInvalidCode"].Value);
        return problemDetails;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static ProblemDetails CreateProblemDetailsBadRequest(Exception exception,
        IStringLocalizer<ResourceMain> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsBadRequestTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-400-bad-request",
            Status = StatusCodes.Status400BadRequest
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsBadRequestCode"].Value);
        return problemDetails;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static ProblemDetails CreateProblemDetailsBadRequestHttpNotSupported(
        IStringLocalizer<ResourceMain> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsBadRequestHttpNotSupportedTitle"],
            Detail = localizer["ProblemDetailsBadRequestHttpNotSupportedDetail"],
            Type = "https://tools.ietf.org/html/rfc9110#name-400-bad-request",
            Status = StatusCodes.Status500InternalServerError
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsBadRequestHttpNotSupportedCode"].Value);
        return problemDetails;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static ProblemDetails CreateProblemDetailsUnauthorized(Exception exception,
        IStringLocalizer<ResourceMain> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsUnauthorizedTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-401-unauthorized",
            Status = StatusCodes.Status401Unauthorized
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsUnauthorizedCode"].Value);
        return problemDetails;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static ProblemDetails CreateProblemDetailsUnauthorizedMissingToken(
        IStringLocalizer<ResourceMain> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsUnauthorizedMissingTokenTitle"],
            Detail = localizer["ProblemDetailsUnauthorizedMissingTokenDetail"],
            Type = "https://tools.ietf.org/html/rfc9110#name-401-unauthorized",
            Status = StatusCodes.Status401Unauthorized
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsUnauthorizedMissingTokenCode"].Value);
        return problemDetails;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static ProblemDetails CreateProblemDetailsUnauthorizedProcessFail(Exception exception,
        IStringLocalizer<ResourceMain> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsUnauthorizedProcessFailTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-401-unauthorized",
            Status = StatusCodes.Status401Unauthorized
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsUnauthorizedProcessFailCode"].Value);
        return problemDetails;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static ProblemDetails CreateProblemDetailsForbiddenAccess(IStringLocalizer<ResourceMain> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsForbiddenAccessTitle"],
            Detail = localizer["ProblemDetailsForbiddenAccessDetail"],
            Type = "https://tools.ietf.org/html/rfc9110#name-403-forbidden",
            Status = StatusCodes.Status403Forbidden
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsForbiddenAccessCode"].Value);
        return problemDetails;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static ProblemDetails CreateProblemDetailsForbidden(Exception exception,
        IStringLocalizer<ResourceMain> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsForbiddenTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-403-forbidden",
            Status = StatusCodes.Status403Forbidden
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsForbiddenCode"].Value);
        return problemDetails;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static ProblemDetails CreateProblemDetailsNotFound(Exception exception,
        IStringLocalizer<ResourceMain> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsNotFoundTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-404-not-found",
            Status = StatusCodes.Status404NotFound
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsNotFoundCode"].Value);
        return problemDetails;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    private static ProblemDetails CreateProblemDetailsMethodNotAllowed(Exception exception,
        IStringLocalizer localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsMethodNotAllowedTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-405-method-not-allowed",
            Status = StatusCodes.Status405MethodNotAllowed
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsMethodNotAllowedCode"].Value);
        return problemDetails;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static ProblemDetails CreateProblemDetailsRequestTimeout(Exception exception,
        IStringLocalizer<ResourceMain> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsRequestTimeoutTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-408-request-timeout",
            Status = StatusCodes.Status408RequestTimeout
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsRequestTimeoutCode"].Value);
        return problemDetails;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static ProblemDetails CreateProblemDetailsConflict(Exception exception,
        IStringLocalizer<ResourceMain> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsConflictTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-409-conflict",
            Status = StatusCodes.Status409Conflict
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsConflictCode"].Value);
        return problemDetails;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static ProblemDetails CreateProblemDetailsGone(Exception exception,
        IStringLocalizer<ResourceMain> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsGoneTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-410-gone",
            Status = StatusCodes.Status410Gone
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsGoneCode"].Value);
        return problemDetails;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static ProblemDetails CreateProblemDetailsUnsupportedMediaType(Exception exception,
        IStringLocalizer localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsUnsupportedMediaTypeTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-415-unsupported-media-type",
            Status = StatusCodes.Status415UnsupportedMediaType
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsUnsupportedMediaTypeCode"].Value);
        return problemDetails;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static ProblemDetails CreateProblemDetailsUnprocessableEntity(Exception exception,
        IStringLocalizer<ResourceMain> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsUnprocessableEntityTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-422-unprocessable-content",
            Status = StatusCodes.Status422UnprocessableEntity
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsUnprocessableEntityCode"].Value);
        return problemDetails;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    private static ProblemDetails CreateProblemDetailsTooManyRequest(Exception exception,
        IStringLocalizer localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsTooManyRequestTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc6585#section-4",
            Status = StatusCodes.Status429TooManyRequests
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsTooManyRequestCode"].Value);
        return problemDetails;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static ProblemDetails CreateProblemDetailsInternalServerError(Exception exception,
        IStringLocalizer<ResourceMain> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsInternalServerErrorTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-500-internal-server-error",
            Status = StatusCodes.Status500InternalServerError
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsInternalServerErrorCode"].Value);
        return problemDetails;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static ProblemDetails CreateProblemDetailsNotImplemented(Exception exception,
        IStringLocalizer<ResourceMain> localizer)
    {
        var problemDetails = new ProblemDetails
        {
            Title = localizer["ProblemDetailsNotImplementedTitle"],
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#name-501-not-implemented",
            Status = StatusCodes.Status501NotImplemented
        };
        problemDetails.Extensions.TryAdd("code", localizer["ProblemDetailsNotImplementedCode"].Value);
        return problemDetails;
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
file static class ProblemDetailsFactoryCoreUtils
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static bool IsRootFileFail(ModelStateDictionary.KeyEnumerable keys)
    {
        return keys.Contains("$");
    }
}