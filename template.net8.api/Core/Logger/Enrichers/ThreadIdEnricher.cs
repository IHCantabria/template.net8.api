using Serilog.Core;
using Serilog.Events;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Logger.Enrichers;

[CoreLibrary]
internal sealed class ThreadIdEnricher : ILogEventEnricher
{
    /// <summary>
    ///     The cached last created "ThreadName" property with some thread name. It is likely to be reused frequently so
    ///     avoiding heap allocations.
    /// </summary>
    private LogEventProperty? _lastValue;

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
    /// <exception cref="ArgumentException">
    ///     When
    ///     <paramref>
    ///         <name>name</name>
    ///     </paramref>
    ///     is empty or only contains whitespace
    /// </exception>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var threadId = Environment.CurrentManagedThreadId;
        var last = _lastValue;
        if (last is null || (int)((ScalarValue)last.Value).Value! != threadId)
            // no need to synchronize threads on write - just some of them will win
            _lastValue = last = new LogEventProperty("request.thread.id", new ScalarValue(threadId));

        logEvent.AddPropertyIfAbsent(last);
    }
}