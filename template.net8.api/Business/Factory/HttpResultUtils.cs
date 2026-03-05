using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Numerics;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.Results;

namespace template.net8.api.Business.Factory;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class HttpResultUtils
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static List<ProblemDetailsValidationError> CreateErrorsCollection(ValidationException validationException)
    {
        var groupedErrors = GroupErrors(validationException.Errors);
        return ConvertToDictionary(groupedErrors);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static HttpStatusCode GetStatusCode(ValidationException validationException)
    {
        var errorGroups = GroupErrors(validationException.Errors);
        var errorCodes = GetHttpStatusCodes(errorGroups).ToList();

        return errorCodes.Count == 1 ? errorCodes[0] : HttpStatusCode.UnprocessableEntity;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static IEnumerable<IGrouping<string, ValidationFailure>> GroupErrors(IEnumerable<ValidationFailure> errors)
    {
        return errors.GroupBy(static error => error.PropertyName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static IEnumerable<HttpStatusCode> GetHttpStatusCodes(
        IEnumerable<IGrouping<string, ValidationFailure>> groups)
    {
        return groups.SelectMany(static g => g.Select(static vf => vf.CustomState).OfType<HttpStatusCode>()).Distinct();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static List<ProblemDetailsValidationError> ConvertToDictionary(
        IEnumerable<IGrouping<string, ValidationFailure>> groupedErrors)
    {
        var errorsList = new List<ProblemDetailsValidationError>();
        //Must be Serial
        foreach (var errors in groupedErrors.Select(static group => group.Select(error =>
                     new ProblemDetailsValidationError
                     {
                         Detail = error.ErrorMessage, Code = error.ErrorCode,
                         Value = error.AttemptedValue?.ToString(), Pointer = group.Key
                     })))
            errorsList.AddRange(errors);

        return errorsList;
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed record
    ProblemDetailsValidationError : IEqualityOperators<ProblemDetailsValidationError, ProblemDetailsValidationError,
    bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string Detail { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string Pointer { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string? Value { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [JsonRequired]
    public required string Code { get; init; }
}