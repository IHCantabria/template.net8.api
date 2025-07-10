using Serilog.Core;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace template.net8.api.Core.Logger.Sinks;

internal sealed class MemorySink : ILogEventSink
{
    private readonly List<LogEvent> _events = new();

    /// <summary>
    ///     Emit the log event
    /// </summary>
    /// <param name="logEvent"></param>
    public void Emit(LogEvent logEvent)
    {
        _events.Add(logEvent);
    }

    /// <summary>
    ///     Flush the events to the real logger
    /// </summary>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public void FlushToLogger(ILogger logger)
    {
        var eventsToFlush = _events.ToList();
        _events.Clear();

        foreach (var logEvent in eventsToFlush) logger.Write(logEvent);
    }
}