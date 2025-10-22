using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Text;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Logger;

namespace template.net8.api.Settings.Middlewares;

/// <summary>
///     Middleware
/// </summary>
/// <param name="next"></param>
[CoreLibrary]
//TODO REVIEW: Evaluate if this middleware is necessary or if it can be replaced by Services.AddHttpLogging and IHttpLoggingInterceptor
public sealed class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    /// <summary>
    ///     Invoke Async method to invoke the middleware.
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    [SuppressMessage("ReSharper", "CA2007",
        Justification =
            "ConfigureAwait cant be injected in the Buffer Memory Stream")]
    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        await RequestLogger.LogActionRequestAsync(context, _logger).ConfigureAwait(false);

        var originalBody = context.Response.Body;
        await using var buffer = new MemoryStream();
        context.Response.Body = buffer;

        try
        {
            await _next(context).ConfigureAwait(false);
            await HandleResponseAsync(context, buffer, originalBody).ConfigureAwait(false);
        }
        finally
        {
            context.Response.Body = originalBody;
        }
    }

    private async Task HandleResponseAsync(HttpContext context, MemoryStream buffer, Stream originalBody)
    {
        if (context.Response.StatusCode >= 400)
        {
            var text = await ReadResponseTextAsync(buffer, context.Response.Headers)
                .ConfigureAwait(false);
            RequestLogger.LogActionResponseError(context, text, _logger);
        }
        else
        {
            RequestLogger.LogActionResponseSuccess(context, _logger);
        }

        buffer.Seek(0, SeekOrigin.Begin);
        await buffer.CopyToAsync(originalBody).ConfigureAwait(false);
    }

    private static async Task<string> ReadResponseTextAsync(
        MemoryStream buffer,
        IHeaderDictionary headers)
    {
        buffer.Seek(0, SeekOrigin.Begin);

        Stream payload = buffer;
        if (headers.TryGetValue("Content-Encoding", out var enc))
        {
            var encVal = enc.ToString().ToUpperInvariant();
            if (encVal.Contains("GZIP", StringComparison.InvariantCultureIgnoreCase))
                payload = new GZipStream(buffer, CompressionMode.Decompress, true);
            else if (encVal.Contains("BR", StringComparison.InvariantCultureIgnoreCase))
                payload = new BrotliStream(buffer, CompressionMode.Decompress, true);
        }

        using var reader = new StreamReader(
            payload,
            Encoding.UTF8,
            false,
            1024,
            true);

        var text = await reader.ReadToEndAsync().ConfigureAwait(false);
        if (payload != buffer)
            await payload.DisposeAsync().ConfigureAwait(false);

        return text;
    }
}