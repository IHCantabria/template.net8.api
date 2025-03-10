using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Extensions;
using template.net8.api.Logger;

namespace template.net8.api.Core.Logger;

[CoreLibrary]
internal static class RequestLogger
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions().AddCoreOptions();

    /// <summary>
    ///     Log Action Request
    /// </summary>
    internal static void LogActionRequest(ActionExecutingContext context, ILogger logger)
    {
        var methodName = context.ActionDescriptor.DisplayName;
        var requestPath = context.HttpContext?.Request.Path;
        logger.LogActionRequestReceived(methodName, requestPath);
        LogRequestParameters(context, logger);
    }

    /// <summary>
    ///     Log Action Request
    /// </summary>
    internal static void LogActionRequest(ActionContext context, ILogger logger)
    {
        var methodName = context.ActionDescriptor.DisplayName;
        var requestPath = context.HttpContext?.Request.Path;
        logger.LogActionRequestReceived(methodName, requestPath);
        if (context is ActionExecutingContext executingContext)
            LogRequestParameters(executingContext, logger);
    }

    /// <summary>
    ///     Log Action Response
    /// </summary>
    /// <exception cref="NotSupportedException">
    ///     There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for
    ///     <typeparamref>
    ///         <name>TValue</name>
    ///     </typeparamref>
    ///     or its serializable members.
    /// </exception>
    internal static void LogActionResponse(ActionExecutedContext context,
        ILogger logger)
    {
        var methodName = context.ActionDescriptor.DisplayName;
        var requestPath = context.HttpContext?.Request.Path;

        // Cast the result to an Mvc ObjectResult and add the value to the log if the result is a error response and not a FileContentResult

        if (context.Result is ObjectResult { Value: not FileContentResult, StatusCode: >= 400 } response)
            logger.LogActionRequestResponseError(JsonSerializer.Serialize(response.Value, Options));

        logger.LogActionRequestResponsed(methodName, requestPath);
    }

    /// <summary>
    ///     Log Action Response
    /// </summary>
    /// <exception cref="NotSupportedException">
    ///     There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for
    ///     <typeparamref>
    ///         <name>TValue</name>
    ///     </typeparamref>
    ///     or its serializable members.
    /// </exception>
    internal static void LogActionResponse(ActionContext context,
        ProblemDetails problemDetails,
        ILogger<Program> logger)
    {
        var methodName = context.ActionDescriptor.DisplayName;
        var requestPath = context.HttpContext?.Request.Path;
        logger.LogActionRequestResponseError(JsonSerializer.Serialize(problemDetails, Options));

        logger.LogActionRequestResponsed(methodName, requestPath);
    }

    private static void LogRequestParameters(ActionExecutingContext context, ILogger logger)
    {
        // Add the request parameters 
        foreach (var param in context.ActionArguments)
        {
            // Skip CancellationToken properties
            if (param.Value is CancellationToken) continue;

            logger.LogActionRequestParameter(param.Key, JsonSerializer.Serialize(param.Value, Options));
        }
    }
}