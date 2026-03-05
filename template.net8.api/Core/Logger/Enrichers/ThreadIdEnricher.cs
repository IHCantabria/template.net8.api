using System.Diagnostics.CodeAnalysis;
using Serilog.Core;
using Serilog.Events;

namespace template.net8.api.Core.Logger.Enrichers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class ThreadIdEnricher : ILogEventEnricher
{
    /// <summary>
    ///     The cached last created "ThreadName" property with some thread name. It is likely to be reused frequently so
    ///     avoiding heap allocations.
    /// </summary>
    private LogEventProperty? _lastValue;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="logEvent" /> is <see langword="null" />.
    ///     <paramref name="propertyFactory" /> is <see langword="null" />.
    /// </exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        ArgumentNullException.ThrowIfNull(logEvent);
        ArgumentNullException.ThrowIfNull(propertyFactory);

        var threadId = Environment.CurrentManagedThreadId;
        var last = _lastValue;
        if (last?.Value is not ScalarValue { Value: int currentThreadId } ||
            currentThreadId != threadId)
            // no need to synchronize threads on write - just some of them will win
            _lastValue = last =
                new LogEventProperty("request.thread.id", new ScalarValue(threadId));

        logEvent.AddPropertyIfAbsent(last);
    }
}