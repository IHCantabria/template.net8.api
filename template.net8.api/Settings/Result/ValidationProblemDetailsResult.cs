using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Factory;
using template.net8.api.Localize.Resources;

namespace template.net8.api.Settings.Result;

/// <summary>
///     Validation Problem Details Result Action
/// </summary>
[CoreLibrary]
public sealed class ValidationProblemDetailsResult : IActionResult
{
    /// <summary>
    ///     Execute Result Async
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="InvalidOperationException">
    ///     There is no service of type
    ///     <typeparamref>
    ///         <name>T</name>
    ///     </typeparamref>
    ///     .
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <paramref>
    ///         <name>keySelector</name>
    ///     </paramref>
    ///     produces duplicate keys for two elements.
    /// </exception>
    public Task ExecuteResultAsync(ActionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var localizer = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<Resource>>();
        var problemDetails =
            ProblemDetailsFactoryCore.CreateProblemDetailsBadRequestValidationPayload(context.ModelState, localizer);
        var objectResult = new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
        return objectResult.ExecuteResultAsync(context);
    }
}