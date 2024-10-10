using Microsoft.Extensions.Diagnostics.HealthChecks;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.HealthChecks;

[CoreLibrary]
internal sealed class MemoryHealthCheck : IHealthCheck
{
    private const long Threshold = 1024L * 1024L * 1024L;


    /// <summary>
    ///     Check Health Async method to check the health of the application based on the allocated memory.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref>
    ///         <name>generation</name>
    ///     </paramref>
    ///     is less than 0.
    /// </exception>
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        // Include GC information in the reported diagnostics.
        var allocated = GC.GetTotalMemory(false);
        var data = new Dictionary<string, object>
        {
            { "AllocatedBytes", allocated },
            { "Gen0Collections", GC.CollectionCount(0) },
            { "Gen1Collections", GC.CollectionCount(1) },
            { "Gen2Collections", GC.CollectionCount(2) }
        };
        var status = allocated < Threshold ? HealthStatus.Healthy : HealthStatus.Unhealthy;

        return Task.FromResult(new HealthCheckResult(
            status,
            "Reports degraded status if allocated bytes " +
            $">= {Threshold} bytes. Restart Service if status is UnHealthy",
            data: data));
    }
}