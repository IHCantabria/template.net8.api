using System.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Extensions;

[CoreLibrary]
internal static class ProblemDetailsContextExtensions
{
    internal static void HiddenDetails(this ProblemDetailsContext ctx, IHostEnvironment env)
    {
        if (ShouldHiddenDetails(env,
                ctx.ProblemDetails.Status))
            ctx.ProblemDetails.Detail = null;
    }

    /// <summary>
    ///     This method will add a method field to the ProblemDetails.Extensions dictionary if it does not already exist.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static void AddMethodField(this ProblemDetailsContext ctx)
    {
        if (ContainsMethod(ctx.ProblemDetails.Extensions)) return;

        var httpMethod = ctx.HttpContext.Request.Method;
        ctx.ProblemDetails.Extensions.TryAdd("method", httpMethod);
    }

    internal static void AddInstanceField(this ProblemDetailsContext ctx)
    {
        if (NotContainsInstance(ctx))
            ctx.ProblemDetails.Instance = GetInstance(ctx.HttpContext);
    }

    /// <summary>
    ///     This method will add a requestId field to the ProblemDetails.Extensions dictionary if it does not already exist.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static void AddRequestIdField(this ProblemDetailsContext ctx)
    {
        if (ContainsRequestId(ctx.ProblemDetails.Extensions)) return;

        var requestId = Activity.Current?.Id ?? ctx.HttpContext.TraceIdentifier;
        ctx.ProblemDetails.Extensions.TryAdd("requestId", requestId);
    }

    /// <summary>
    ///     This method will add a traceId field to the ProblemDetails.Extensions dictionary if it does not already exist.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static void AddTraceIdField(this ProblemDetailsContext ctx)
    {
        if (ContainsTraceId(ctx.ProblemDetails.Extensions)) return;

        var activityFeature = ctx.HttpContext.Features.Get<IHttpActivityFeature>();
        var activity = activityFeature?.Activity;
        if (activity is not null)
            ctx.ProblemDetails.Extensions.TryAdd("traceId", activity);
    }

    /// <summary>
    ///     This method will add a code field to the ProblemDetails.Extensions dictionary if it does not already exist.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static void AddCodeField(this ProblemDetailsContext ctx)
    {
        if (ContainsCode(ctx.ProblemDetails.Extensions)) return;

        const string code = "BE-BROKEN-ARROW";
        ctx.ProblemDetails.Extensions.TryAdd("code", code);
    }

    internal static void UseHttpContextProblemDetails(this ProblemDetailsContext ctx,
        ProblemDetails clientProblemDetails)
    {
        ctx.ProblemDetails.Status = clientProblemDetails.Status ?? ctx.ProblemDetails.Status;
        ctx.HttpContext.Response.StatusCode = clientProblemDetails.Status ?? StatusCodes.Status400BadRequest;
        ctx.ProblemDetails.Title = clientProblemDetails.Title ?? ctx.ProblemDetails.Title;
        ctx.ProblemDetails.Detail = clientProblemDetails.Detail ?? ctx.ProblemDetails.Detail;
        ctx.ProblemDetails.Type = clientProblemDetails.Type ?? ctx.ProblemDetails.Type;
        ctx.ProblemDetails.Instance = clientProblemDetails.Instance ?? ctx.ProblemDetails.Instance;
        ctx.ProblemDetails.Extensions = clientProblemDetails.Extensions.Count > 0
            ? MergeExtensions(ctx.ProblemDetails.Extensions, clientProblemDetails.Extensions)
            : ctx.ProblemDetails.Extensions;
    }

    private static Dictionary<string, object?> MergeExtensions(IDictionary<string, object?> serverExtensions,
        IDictionary<string, object?> clientExtensions)
    {
        // Create a new dictionary to hold the merged extensions
        Dictionary<string, object?> mergedExtensions = new(serverExtensions);

        // Merge or override serverExtensions with clientExtensions
        //Should be Serial
        foreach (var entry in clientExtensions) mergedExtensions[entry.Key] = entry.Value;

        return mergedExtensions;
    }

    private static bool ShouldHiddenDetails(IHostEnvironment env, int? status)
    {
        return env.IsProduction() && status >= StatusCodes.Status500InternalServerError;
    }

    private static bool ContainsRequestId(IDictionary<string, object?> extensions)
    {
        return extensions.ContainsKey("requestId");
    }

    private static bool ContainsTraceId(IDictionary<string, object?> extensions)
    {
        return extensions.ContainsKey("traceId");
    }

    private static bool ContainsCode(IDictionary<string, object?> extensions)
    {
        return extensions.ContainsKey("code");
    }

    private static bool NotContainsInstance(ProblemDetailsContext ctx)
    {
        return ctx.ProblemDetails.Instance is null;
    }

    private static bool ContainsMethod(IDictionary<string, object?> extensions)
    {
        return extensions.ContainsKey("method");
    }

    private static string GetInstance(HttpContext ctx)
    {
        return $"{ctx.Request.Scheme}://{ctx.Request.Host}{ctx.Request.Path}";
    }
}