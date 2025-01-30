using Delta;
using template.net8.api.Business;
using template.net8.api.Core.Attributes;
using template.net8.api.Domain.Persistence.Context;
using template.net8.api.Settings.Interfaces;

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
    public Task ConfigurePipelineAsync(WebApplication app)
    {
        DeltaExtensions.UseResponseDiagnostics = true;
        app.UseDelta<ProjectDbContext>(_ => BusinessConstants.ApiName);

        //Beware of caching on permission-segregated data, use logic similar to this:
        //app.UseDelta<ProjectDbContext>(httpContext =>
        //    httpContext.User.Identity is not null && httpContext.User.Identity.IsAuthenticated
        //        ? $"{BusinessConstants.ApiName}:{BusinessConstants.AuthenticatedUser}"
        //        : $"{BusinessConstants.ApiName}:{BusinessConstants.AnonymousUser}");

        return Task.CompletedTask;
    }
}