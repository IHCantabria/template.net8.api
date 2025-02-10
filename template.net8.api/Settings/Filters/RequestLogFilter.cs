using Microsoft.AspNetCore.Mvc.Filters;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Logger;

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

        //Log the action request
        RequestLogger.LogActionRequest(context, _logger);

        // Await the action result
        var result = await next().ConfigureAwait(false);

        //Log the action response
        RequestLogger.LogActionResponse(result, _logger);
    }

    /// <summary>
    ///     Order of the filter
    /// </summary>
    public int Order { get; }
}