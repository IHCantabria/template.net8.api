using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using template.net8.api.Business.Factory;
using template.net8.api.Localize.Resources;

namespace template.net8.api.Business.Extensions;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class ProblemDetailsExtensions
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static void AddErrors(this ProblemDetails problemDetails,
        IStringLocalizer<ResourceMain> localizer,
        ValidationException vex)
    {
        var errorsCollection = HttpResultUtils.CreateErrorsCollection(vex);
        problemDetails.Detail = localizer["ProblemDetailsValidationDetail"];
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
            problemDetails.Title = localizer["ProblemDetailsValidationTitle"];
            problemDetails.Extensions["errors"] = errorsCollection;
            problemDetails.Extensions["code"] = localizer["ProblemDetailsValidationCode"];
        }
    }
}