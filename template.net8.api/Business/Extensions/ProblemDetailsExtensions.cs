using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using template.net8.api.Business.Factory;
using template.net8.api.Core.Attributes;
using template.net8.api.Localize.Resources;

namespace template.net8.api.Business.Extensions;

[CoreLibrary]
internal static class ProblemDetailsExtensions
{
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
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>key</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///     The property is set and the
    ///     <see>
    ///         <cref>IDictionary`2</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    ///     The property is retrieved and
    ///     <paramref>
    ///         <name>key</name>
    ///     </paramref>
    ///     is not found.
    /// </exception>
    internal static ProblemDetails AddErrors(this ProblemDetails problemDetails,
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

        return problemDetails;
    }
}