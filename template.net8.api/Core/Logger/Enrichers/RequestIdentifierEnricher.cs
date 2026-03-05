using Serilog.Core;
using Serilog.Events;

namespace template.net8.api.Core.Logger.Enrichers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class RequestIdentifierEnricher : ILogEventEnricher
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="logEvent" /> is <see langword="null" />.
    ///     <paramref name="propertyFactory" /> is <see langword="null" />.
    /// </exception>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        ArgumentNullException.ThrowIfNull(logEvent);
        ArgumentNullException.ThrowIfNull(propertyFactory);

        var requestId = RequestIdProvider.GetCurrentRequestId();

        if (string.IsNullOrEmpty(requestId)) return;

        logEvent.RemovePropertyIfPresent("RequestId");
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("request.id", requestId));
    }
}