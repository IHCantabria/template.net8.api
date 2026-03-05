using System.Diagnostics.CodeAnalysis;
using Delta;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using template.net8.api.Business;
using template.net8.api.Persistence.Context;
using template.net8.api.Settings.Interfaces;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.PipelineConfigurators;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class EtagConfigurator : IPipelineConfigurator
{
    /// <inheritdoc cref="IPipelineConfigurator.LoadOrder" />
    public short LoadOrder => 4;

    /// <inheritdoc cref="IPipelineConfigurator.ConfigurePipelineAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="app" /> is <see langword="null" />.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public Task ConfigurePipelineAsync(WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
        DeltaExtensions.UseResponseDiagnostics = true;

        //Get swagger configuration from service with strongly typed options object.
        var configuration = app.Services.GetRequiredService<IOptions<AppDbOptions>>().Value;
        if (string.IsNullOrEmpty(configuration.ConnectionString)) return Task.CompletedTask;

        app.UseDelta<AppDbContext>(static _ => BusinessConstants.ApiName,
            static httpContext => httpContext.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase));

        //Beware of caching on permission-segregated or user-segregated data, use suffix and Identity access to separate cache entries.
        return Task.CompletedTask;
    }
}