using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Logger.Enrichers;

[CoreLibrary]
internal sealed class ActivityEnricher : ILogEventEnricher
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
        var activity = Activity.Current;
        if (activity is null) return;

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("trace_id", activity.TraceId.ToString()));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("span_id", activity.SpanId.ToString()));

        if (activity.ParentSpanId != default)
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("parent_span_id",
                activity.ParentSpanId.ToString()));

        foreach (var tag in activity.TagObjects)
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(tag.Key, tag.Value));
    }
}