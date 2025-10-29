using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using Serilog.Context;
using template.net8.api.Core.Attributes;
using template.net8.api.Logger;

namespace template.net8.api.Core.Logger;

[CoreLibrary]
internal static class RequestLogger
{
    private const int MaxBodySizeBytes = 1024 * 1024 * 10; // Máximo 10MB

    /// <summary>
    ///     Log Action Request
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentContextLogPropertyNaming",
        Justification =
            "Open Telemetry fields should have this format: father.child")]
    internal static async Task LogActionRequestAsync(HttpContext context, ILogger logger)
    {
        var methodName = context.Request.Method;
        var requestPath = context.Request.Path;

        var queryParams = ExtractQueryParams(context);
        var routeParams = ExtractRouteParams(context);
        var (bodyContent, formFields) = await ExtractRequestBodyAsync(context).ConfigureAwait(false);

        using (LogContext.PushProperty("request.params.query", queryParams, true))
        using (LogContext.PushProperty("request.params.route", routeParams, true))
        using (LogContext.PushProperty("request.params.body", bodyContent ?? string.Empty, true))
        using (LogContext.PushProperty("request.params.form_field", formFields ?? new Dictionary<string, string>(),
                   true))
        {
            logger.LogActionRequestReceived(methodName, requestPath);
        }
    }

    private static Dictionary<string, string> ExtractQueryParams(HttpContext context)
    {
        return context.Request.Query.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.ToString());
    }

    private static Dictionary<string, string> ExtractRouteParams(HttpContext context)
    {
        return context.GetRouteData()?.Values
                   .ToDictionary(
                       kvp => kvp.Key,
                       kvp => kvp.Value?.ToString() ?? string.Empty)
               ?? new Dictionary<string, string>();
    }

    private static async Task<(string? BodyContent, Dictionary<string, string>? FormFields)>
        ExtractRequestBodyAsync(HttpContext context)
    {
        if (context.Request.Method is not ("POST" or "PUT" or "PATCH" or "DELETE"))
            return (null, null);

        var bodyContent = await TryReadJsonBodyAsync(context).ConfigureAwait(false);
        var formFields = await TryReadFormFieldsAsync(context).ConfigureAwait(false);
        return (bodyContent, formFields);
    }

    private static async Task<string?> TryReadJsonBodyAsync(HttpContext context)
    {
        var contentType = context.Request.ContentType ?? string.Empty;
        if (!contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase))
            return null;

        context.Request.EnableBuffering();

        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await ReadLimitedAsync(reader).ConfigureAwait(false);
        context.Request.Body.Position = 0;

        return body;
    }

    private static async Task<Dictionary<string, string>?> TryReadFormFieldsAsync(HttpContext context)
    {
        var contentType = context.Request.ContentType ?? string.Empty;
        if (!contentType.Contains("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase) &&
            !contentType.Contains("multipart/form-data", StringComparison.OrdinalIgnoreCase))
            return null;

        context.Request.EnableBuffering();
        var form = await context.Request.ReadFormAsync().ConfigureAwait(false);
        var formFields = form.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());

        context.Request.Body.Position = 0;
        return formFields;
    }

    private static async Task<string> ReadLimitedAsync(StreamReader reader)
    {
        var buffer = new char[MaxBodySizeBytes / sizeof(char)];
        var read = await reader.ReadBlockAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
        return new string(buffer, 0, read);
    }

    /// <summary>
    ///     Log Action Response
    /// </summary>
    internal static void LogActionResponseSuccess(HttpContext context,
        ILogger logger)
    {
        var routeData = context.GetRouteData();
        var controller = routeData?.Values["controller"]?.ToString();
        var action = routeData?.Values["action"]?.ToString();
        var methodName = string.IsNullOrEmpty(controller) || string.IsNullOrEmpty(action)
            ? context.Request.Method
            : $"{controller}.{action}";
        var requestPath = context.Request.Path;
        logger.LogActionRequestResponsedSuccess(methodName, requestPath);
    }

    /// <summary>
    ///     Log Action Response
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentContextLogPropertyNaming",
        Justification =
            "Open Telemetry fields should have this format: father.child")]
    internal static void LogActionResponseError(HttpContext context, string responseText,
        ILogger logger)
    {
        var routeData = context.GetRouteData();
        var controller = routeData?.Values["controller"]?.ToString();
        var action = routeData?.Values["action"]?.ToString();
        var methodName = string.IsNullOrEmpty(controller) || string.IsNullOrEmpty(action)
            ? context.Request.Method
            : $"{controller}.{action}";
        var requestPath = context.Request.Path;
        using (LogContext.PushProperty("request.response.error", responseText))
        {
            logger.LogActionRequestResponsedError(methodName, requestPath,
                context.Response.StatusCode.ToString(CultureInfo.InvariantCulture));
        }
    }
}