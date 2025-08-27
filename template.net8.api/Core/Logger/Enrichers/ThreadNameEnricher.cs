using Serilog.Core;
using Serilog.Events;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Logger.Enrichers;

[CoreLibrary]
internal sealed class ThreadNameEnricher : ILogEventEnricher
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
    /// <exception cref="InvalidOperationException">
    ///     A set operation was requested, but the <see langword="Name" /> property has
    ///     already been set.
    /// </exception>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var threadName = Thread.CurrentThread.Name;
        if (threadName is null) return;

        var last = _lastValue;
        if (last is null || (string)((ScalarValue)last.Value).Value! != threadName)
            _lastValue = last = new LogEventProperty("request.thread.name", new ScalarValue(threadName));

        logEvent.AddPropertyIfAbsent(last);
    }
}