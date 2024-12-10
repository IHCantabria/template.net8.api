using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using template.net8.api.Core.Attributes;
using template.net8.api.Logger;

namespace template.net8.api.Settings.Filters;

/// <summary>
///     Request Log Filter
/// </summary>
[CoreLibrary]
public sealed class RequestLogFilter : IAsyncActionFilter, IOrderedFilter
{
    private readonly ILogger _logger;

    /// <summary>
    ///     Default Constructor
    /// </summary>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    public RequestLogFilter(ILogger<RequestLogFilter> logger)
    {
        Order = 2;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///     OnActionExecutionAsync method
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="NotSupportedException">
    ///     There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for
    ///     <typeparamref>
    ///         <name>TValue</name>
    ///     </typeparamref>
    ///     or its serializable members.
    /// </exception>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);
        var methodName = context.ActionDescriptor.DisplayName;
        var requestPath = context.HttpContext?.Request.Path;
        _logger.LogActionRequestReceived(methodName, requestPath);
        var jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
            WriteIndented = true
        };
        LogRequestParameters(context, jsonOptions);

        // Await the action result
        var result = await next().ConfigureAwait(false);

        // Cast the result to an Mvc ObjectResult and add the value to the log if the result is a error response and not a FileContentResult

        if (result.Result is ObjectResult { Value: not FileContentResult, StatusCode: >= 400 } response)
        {
            _logger.LogActionRequestResponseError(JsonSerializer.Serialize(response.Value, jsonOptions));
        }

        _logger.LogActionRequestResponsed(methodName, requestPath);
    }

    /// <summary>
    ///     Order of the filter
    /// </summary>
    public int Order { get; }

    private void LogRequestParameters(ActionExecutingContext context, JsonSerializerOptions jsonOptions)
    {
        // Add the request parameters 
        var requestParameters = context.ActionArguments;
        foreach (var param in requestParameters)
        {
            // Skip CancellationToken properties
            if (param.Value is CancellationToken)
            {
                continue;
            }

            _logger.LogActionRequestParameter(param.Key, JsonSerializer.Serialize(param.Value, jsonOptions));
        }
    }
}