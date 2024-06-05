using template.net8.api.Core.Attributes;
using template.net8.api.Logger;

namespace template.net8.api.Communications;

[CoreLibrary]
internal class WebClientBase
{
    internal readonly IHttpClientFactory HttpClientFactory;

    internal readonly ILogger Logger;

    /// <exception cref="ArgumentNullException">Condition.</exception>
    protected WebClientBase(IHttpClientFactory httpClientFactory, ILogger<WebClientBase> logger)
    {
        HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        Logger.LogWebClientBaseInjected(logger.GetType().ToString());
    }

    internal static HttpRequestMessage CreateRequest(HttpMethod method, Uri path)
    {
        return new HttpRequestMessage(method, path);
    }
}