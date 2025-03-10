using Serilog;
using Serilog.Exceptions;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Logger;

[CoreLibrary]
internal static class MainLoggerFactory
{
    /// <summary>
    ///     Main Log Factory
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     When
    ///     <paramref>
    ///         <name>value</name>
    ///     </paramref>
    ///     is <code>null</code>
    /// </exception>
    internal static void MainLogFactory()
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .CreateBootstrapLogger();
    }
}