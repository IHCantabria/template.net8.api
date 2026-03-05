using Serilog.Core;
using Serilog.Events;

namespace template.net8.api.Core.Logger.Enrichers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class CorrelationIdEnricher : ILogEventEnricher
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private const string CorrelationIdItemKey = "Serilog_CorrelationId";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private const string PropertyName = "correlation_id";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private const string CorrelationIdValueKey = "Serilog_CorrelationId_Value";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly bool _addValueIfHeaderAbsence;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IHttpContextAccessor _contextAccessor;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly string _headerKey;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public CorrelationIdEnricher(string headerKey, bool addValueIfHeaderAbsence)
        : this(headerKey, addValueIfHeaderAbsence, new HttpContextAccessor())
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private CorrelationIdEnricher(string headerKey, bool addValueIfHeaderAbsence, IHttpContextAccessor contextAccessor)
    {
        _headerKey = headerKey;
        _addValueIfHeaderAbsence = addValueIfHeaderAbsence;
        _contextAccessor = contextAccessor;
    }

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

        var httpContext = _contextAccessor.HttpContext;
        if (httpContext is null)
            return;

        if (TryEnrichFromCache(httpContext, logEvent))
            return;

        var correlationId = ResolveCorrelationId(httpContext);
        var correlationProperty = propertyFactory.CreateProperty(PropertyName, correlationId);

        logEvent.AddOrUpdateProperty(correlationProperty);
        CacheCorrelation(httpContext, correlationProperty, correlationId);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static bool TryEnrichFromCache(HttpContext httpContext, LogEvent logEvent)
    {
        if (!httpContext.Items.TryGetValue(CorrelationIdItemKey, out var existingValue) ||
            existingValue is not LogEventProperty existingProperty)
            return false;

        logEvent.AddPropertyIfAbsent(existingProperty);

        // Ensure the raw string value is stored for quick access later
        httpContext.Items.TryAdd(
            CorrelationIdValueKey,
            (existingProperty.Value as ScalarValue)?.Value as string);

        return true;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private string? ResolveCorrelationId(HttpContext httpContext)
    {
        var headerValue = httpContext.Request.Headers[_headerKey].FirstOrDefault()
                          ?? httpContext.Response.Headers[_headerKey].FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(headerValue))
            return headerValue;

        return _addValueIfHeaderAbsence ? Guid.NewGuid().ToString() : null;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void CacheCorrelation(
        HttpContext httpContext,
        LogEventProperty correlationProperty,
        string? correlationId)
    {
        httpContext.Items[CorrelationIdItemKey] = correlationProperty;
        httpContext.Items[CorrelationIdValueKey] = correlationId;
    }
}