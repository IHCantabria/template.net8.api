using FluentValidation;
using LanguageExt.Common;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using template.net8.api.Business.Exceptions;
using template.net8.api.Business.Messages;

namespace template.net8.api.Controllers.Extensions;

internal static class ControllerExtensions
{
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    /// <exception cref="NotSupportedException">Condition.</exception>
    internal static IActionResult ToActionResult<TResult, TContract>(this Result<TResult> result,
        Func<TResult, TContract> mapper, IFeatureCollection features)
    {
        return result.Match(obj =>
            {
                var response = mapper(obj);
                return new OkObjectResult(response);
            },
            ex =>
            {
                if (ex is BusinessException or ValidationException)
                    return BusinessExceptionMapper.MapExceptionToResult(ex, features);

                //Exception not Controlled
                //Important: Only Write details for Business Exceptions
                var clientProblemDetails = new ProblemDetails
                {
                    Title = MessageDefinitions.GenericServerError,
                    Detail = ex.Message,
                    Type = "https://tools.ietf.org/html/rfc9110#name-500-internal-server-error",
                    Status = StatusCodes.Status500InternalServerError
                };
                features.Set(clientProblemDetails);
                return new BadRequestResult();
            });
    }
}