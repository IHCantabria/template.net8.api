﻿using System.Net;
using System.Numerics;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Business.Factory;

[CoreLibrary]
internal static class HttpResultUtils
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
    internal static ProblemDetails AddErrors(ProblemDetails problemDetails, ValidationException vex)
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
    internal static HttpStatusCode GetStatusCode(ValidationException validationException)
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
        //Must be Serial
        foreach (var group in groupedErrors)
        {
            var errors = group.Select(error => new ProblemDetailsValidationError
            {
                Detail = error.ErrorMessage, Code = $"{BusinessConstants.ApiCode}-BE-{error.ErrorCode}",
                Value = error.AttemptedValue?.ToString(), Pointer = group.Key
            });
            errorsList.AddRange(errors);
        }

        return errorsList;
    }
}

[CoreLibrary]
internal sealed record
    ProblemDetailsValidationError : IEqualityOperators<ProblemDetailsValidationError, ProblemDetailsValidationError,
    bool>
{
    [JsonRequired] public required string Detail { get; init; }

    [JsonRequired] public required string Pointer { get; init; }

    [JsonRequired] public required string? Value { get; init; }

    [JsonRequired] public required string Code { get; init; }
}