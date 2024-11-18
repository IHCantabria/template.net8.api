using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using template.net8.api.Business.Extensions;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Factory;
using template.net8.api.Localize.Resources;

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
    internal static BadRequestResult CreateValidationResult(
        ValidationException vex, IStringLocalizer<ResourceMain> localizer, IFeatureCollection features)
    {
        var httpStatusCode = HttpResultUtils.GetStatusCode(vex);
        return ManageValidationResultCreation(httpStatusCode, vex, localizer, features);
    }

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

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static BadRequestResult CreateBadRequestResult(Exception exception,
        IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsBadRequest(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    private static BadRequestResult CreateValidationResult(HttpStatusCode statusCode, ValidationException exception,
        IStringLocalizer<ResourceMain> localizer, IFeatureCollection features)
    {
        var clientProblemDetails =
            ProblemDetailsFactoryCore.CreateProblemDetailsByHttpStatusCode(statusCode, exception, localizer);
        clientProblemDetails.AddErrors(localizer, exception);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static BadRequestResult CreateUnauthorizedResult(Exception exception,
        IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsUnauthorized(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static BadRequestResult CreateForbiddenResult(Exception exception,
        IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsForbidden(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static BadRequestResult CreateNotFoundResult(Exception exception, IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsNotFound(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static BadRequestResult CreateRequestTimeoutResult(Exception exception,
        IStringLocalizer<ResourceMain> localizer, IFeatureCollection features)
    {
        var clientProblemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsRequestTimeout(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static BadRequestResult CreateConflictResult(Exception exception, IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsConflict(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static BadRequestResult CreateGoneResult(Exception exception, IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsGone(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static BadRequestResult CreateUnprocessableEntityResult(Exception exception,
        IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails =
            ProblemDetailsFactoryCore.CreateProblemDetailsUnprocessableEntity(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static BadRequestResult CreateInternalServerErrorResult(Exception exception,
        IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        var clientProblemDetails =
            ProblemDetailsFactoryCore.CreateProblemDetailsInternalServerError(exception, localizer);
        features.Set(clientProblemDetails);
        return new BadRequestResult();
    }

    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
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