using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using template.net8.api.Logger;

namespace template.net8.api.Communications;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "ReSharper",
    "UnusedType.Global",
    Justification = "Type is instantiated via dependency injection and/or reflection.")]
[SuppressMessage(
    "ReSharper",
    "NotAccessedField.Global",
    Justification = "Field is accessed by derived classes.")]
[SuppressMessage(
    "ReSharper",
    "MemberCanBePrivate.Global",
    Justification = "Members are intended to be accessed by derived classes.")]
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification = "Method is intended for reuse by derived web clients.")]
internal abstract class WebClientBase
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    protected readonly IHttpClientFactory HttpClientFactory;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    protected readonly ILogger Logger;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    protected WebClientBase(IHttpClientFactory httpClientFactory, ILogger<WebClientBase> logger)
    {
        HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (Logger.IsEnabled(LogLevel.Debug))
            Logger.LogWebClientBaseInjected(logger.GetType().ToString());
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [MustDisposeResource]
    internal static HttpRequestMessage CreateRequest(HttpMethod method, Uri path)
    {
        return new HttpRequestMessage(method, path);
    }
}