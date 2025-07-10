using Serilog.Core;
using Serilog.Events;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Logger.Enrichers;

[CoreLibrary]
internal sealed class RequestIdentifierEnricher : ILogEventEnricher
{
    /// <summary>
    ///     Enriches the log event with Activity information.
    /// </summary>
    /// <param name="logEvent"></param>
    /// <param name="propertyFactory"></param>
    /// <exception cref="ArgumentNullException">
    ///     When
    ///     <paramref>
    ///         <name>property</name>
    ///     </paramref>
    ///     is <code>null</code>
    /// </exception>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var requestId = RequestIdProvider.GetCurrentRequestId();

        if (string.IsNullOrEmpty(requestId)) return;

        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("RequestId", requestId));
    }
}