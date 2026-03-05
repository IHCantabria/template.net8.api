using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Serilog.Core;
using template.net8.api.Core.Logger.Sinks;
using ILogger = Serilog.ILogger;

namespace template.net8.api.Core.Logger.Extensions;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class LoggerExtensions
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="NotSupportedException">
    ///     A field is marked literal, but the field does not have one of the accepted
    ///     literal types.
    /// </exception>
    /// <exception cref="FieldAccessException">
    ///     The caller does not have permission to access this field.
    ///     Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception,
    ///     <see cref="MemberAccessException" />, instead.
    /// </exception>
    [SuppressMessage("Security",
        "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields",
        Justification =
            "Reflection is required to access internal Serilog state because no public API is available to determine whether sinks are configured.")]
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumented",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static bool CurrentLoggerHasSinks(this ILogger? logger)
    {
        var innerLoggerField = logger?.GetType().GetField("_logger", BindingFlags.NonPublic | BindingFlags.Instance);
        if (innerLoggerField != null) logger = innerLoggerField.GetValue(logger) as Serilog.Core.Logger;

        var pipelineField =
            typeof(Serilog.Core.Logger).GetField("_sink", BindingFlags.NonPublic | BindingFlags.Instance);
        if (pipelineField == null) return false;

        var pipeline = pipelineField.GetValue(logger);
        if (pipeline == null) return false;

        var sinksField = pipeline.GetType().GetField("_sinks", BindingFlags.NonPublic | BindingFlags.Instance);
        if (sinksField == null) return false;

        var sinks = sinksField.GetValue(pipeline) as IEnumerable;
        return sinks != null && sinks.Cast<ILogEventSink>().Any(static s => !IsOrWrapsMemorySink(s));
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage("Security",
        "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields",
        Justification =
            "Reflection is required to access internal Serilog state because no public API is available to determine whether sinks are configured.")]
    private static bool IsOrWrapsMemorySink(ILogEventSink sink)
    {
        switch (sink)
        {
            case null:
                return false;
            case MemorySink:
                return true;
        }

        var wrappedSinkField = sink.GetType().GetField("_wrappedSink", BindingFlags.NonPublic | BindingFlags.Instance);
        if (wrappedSinkField == null) return false;

        var inner = wrappedSinkField.GetValue(sink) as ILogEventSink;
        return inner != null && IsOrWrapsMemorySink(inner);
    }
}