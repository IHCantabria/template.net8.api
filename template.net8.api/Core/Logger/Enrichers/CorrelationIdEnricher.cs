using Serilog.Core;
using Serilog.Events;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Logger.Enrichers;

[CoreLibrary]
internal sealed class CorrelationIdEnricher : ILogEventEnricher
{
    private const string CorrelationIdItemKey = "Serilog_CorrelationId";
    private const string PropertyName = "correlation_id";
    private const string CorrelationIdValueKey = "Serilog_CorrelationId_Value";

    private readonly bool _addValueIfHeaderAbsence;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly string _headerKey;

    public CorrelationIdEnricher(string headerKey, bool addValueIfHeaderAbsence)
        : this(headerKey, addValueIfHeaderAbsence, new HttpContextAccessor())
    {
    }

    internal CorrelationIdEnricher(string headerKey, bool addValueIfHeaderAbsence, IHttpContextAccessor contextAccessor)
    {
        _headerKey = headerKey;
        _addValueIfHeaderAbsence = addValueIfHeaderAbsence;
        _contextAccessor = contextAccessor;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="System.Collections.Generic.KeyNotFoundException">
    ///     The property is retrieved and
    ///     <paramref>
    ///         <name>key</name>
    ///     </paramref>
    ///     is not found.
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///     The property is set and the
    ///     <see>
    ///         <cref>IDictionary`2</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = _contextAccessor.HttpContext;
        if (httpContext == null)
            return;

        // Check if CorrelationId is already in HttpContext.Items
        if (httpContext.Items.TryGetValue(CorrelationIdItemKey, out var existingValue) &&
            existingValue is LogEventProperty existingProperty)
        {
            logEvent.AddPropertyIfAbsent(existingProperty);

            // Ensure the raw string value is stored for quick access later
            httpContext.Items.TryAdd(CorrelationIdValueKey,
                (existingProperty.Value as ScalarValue)?.Value as string
            );

            return;
        }

        // Try to get CorrelationId from request, response or generate a new one if allowed
        var correlationId = httpContext.Request.Headers[_headerKey].FirstOrDefault()
                            ?? httpContext.Response.Headers[_headerKey].FirstOrDefault()
                            ?? (_addValueIfHeaderAbsence ? Guid.NewGuid().ToString() : null);

        var correlationProperty = propertyFactory.CreateProperty(PropertyName, correlationId);
        logEvent.AddOrUpdateProperty(correlationProperty);

        // Cache both property and raw value in HttpContext.Items
        httpContext.Items[CorrelationIdItemKey] = correlationProperty;
        httpContext.Items[CorrelationIdValueKey] = correlationId;
    }
}