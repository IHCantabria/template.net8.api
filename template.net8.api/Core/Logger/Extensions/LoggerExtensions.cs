using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Serilog.Core;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Logger.Sinks;
using ILogger = Serilog.ILogger;

namespace template.net8.api.Core.Logger.Extensions;

[CoreLibrary]
internal static class LoggerExtensions
{
    /// <summary>
    ///     Check if the current logger has sinks
    /// </summary>
    /// <param name="logger"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    /// <exception cref="ArgumentException">
    ///     The method is neither declared nor inherited by the class of
    ///     <paramref>
    ///         <name>obj</name>
    ///     </paramref>
    ///     .
    /// </exception>
    /// <exception cref="FieldAccessException">
    ///     The caller does not have permission to access this field.
    ///     Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception,
    ///     <see cref="MemberAccessException" />, instead.
    /// </exception>
    /// <exception cref="TargetException">
    ///     The field is non-static and
    ///     <paramref>
    ///         <name>obj</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    ///     Note: In .NET for Windows Store apps or the Portable Class Library, catch <see cref="Exception" /> instead.
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///     A field is marked literal, but the field does not have one of the accepted
    ///     literal types.
    /// </exception>
    /// <exception cref="InvalidCastException">
    ///     An element in the sequence cannot be cast to type
    ///     <paramref>
    ///         <name>TResult</name>
    ///     </paramref>
    ///     .
    /// </exception>
    [SuppressMessage("Security",
        "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields",
        Justification =
            "Access to the non-public field is necessary because Serilog dont provide with a form to know if the current log has Sinks configured")]
    internal static bool CurrentLoggerHasSinks(this ILogger? logger)
    {
        var innerLoggerField = logger?.GetType().GetField("_logger", BindingFlags.NonPublic | BindingFlags.Instance);
        if (innerLoggerField != null) logger = innerLoggerField.GetValue(logger) as Serilog.Core.Logger;

        // Usa Reflection para obtener los Sinks privados dentro del Logger
        var pipelineField =
            typeof(Serilog.Core.Logger).GetField("_sink", BindingFlags.NonPublic | BindingFlags.Instance);
        if (pipelineField == null) return false;

        var pipeline = pipelineField.GetValue(logger);
        if (pipeline == null) return false;

        var sinksField = pipeline.GetType().GetField("_sinks", BindingFlags.NonPublic | BindingFlags.Instance);
        if (sinksField == null) return false;

        var sinks = sinksField.GetValue(pipeline) as IEnumerable;
        return sinks != null && sinks.Cast<ILogEventSink>().Any(les => les is not MemorySink);
    }
}