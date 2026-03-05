using System.Diagnostics.CodeAnalysis;
using Serilog.Core;
using Serilog.Events;

namespace template.net8.api.Core.Logger.Enrichers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class ThreadNameEnricher : ILogEventEnricher
{
    /// <summary>
    ///     ADD DOCUMENTATION
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

        var threadName = Thread.CurrentThread.Name;
        if (threadName is null) return;

        var last = _lastValue;
        if (last?.Value is not ScalarValue { Value: string currentName } ||
            !string.Equals(currentName, threadName, StringComparison.Ordinal))
            _lastValue = last =
                new LogEventProperty("request.thread.name", new ScalarValue(threadName));

        logEvent.AddPropertyIfAbsent(last);
    }
}