using Delta;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using template.net8.api.Business;
using template.net8.api.Core.Attributes;
using template.net8.api.Domain.Persistence.Context;
using template.net8.api.Settings.Interfaces;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.PipelineConfigurators;

/// <summary>
///     Etag Db Tracking Configurator
/// </summary>
[CoreLibrary]
public sealed class EtagConfigurator : IPipelineConfigurator
{
    /// <summary>
    ///     Load order of the pipeline configurator
    /// </summary>
    public short LoadOrder => 4;

    /// <summary>
    ///     Configure Pipeline for the Etag Db Tracking
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">
    ///     There is no service of type
    ///     <typeparamref>
    ///         <name>T</name>
    ///     </typeparamref>
    ///     .
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <paramref>
    ///         <name>comparisonType</name>
    ///     </paramref>
    ///     is not a <see cref="StringComparison" /> value.
    /// </exception>
    public Task ConfigurePipelineAsync(WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
        DeltaExtensions.UseResponseDiagnostics = true;

        //Get swagger configuration from service with strongly typed options object.
        var configuration = app.Services.GetRequiredService<IOptions<ProjectDbOptions>>().Value;
        if (configuration.ConnectionString.IsNullOrEmpty()) return Task.CompletedTask;

        app.UseDelta<ProjectDbContext>(_ => BusinessConstants.ApiName,
            httpContext => httpContext.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase));

        //Beware of caching on permission-segregated data, use logic similar to this:
        //app.UseDelta<ProjectDbContext>(httpContext =>
        //        httpContext.User.Identity is not null && httpContext.User.Identity.IsAuthenticated
        //            ? $"{BusinessConstants.ApiName}:{BusinessConstants.AuthenticatedUser}"
        //            : $"{BusinessConstants.ApiName}:{BusinessConstants.AnonymousUser}",
        //    httpContext => httpContext.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase));

        return Task.CompletedTask;
    }
}