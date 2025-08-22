using template.net8.api.Core.Attributes;
using template.net8.api.Domain.Persistence.Context;
using template.net8.api.GraphQL;
using template.net8.api.GraphQL.Types;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Graph QL Service Installer
/// </summary>
[CoreLibrary]
public sealed class GraphQLInstaller : IServiceInstaller
{
    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 25;

    /// <summary>
    ///     Install Mediator Service
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     -
    ///     <typeparamref>
    ///         <name>TQuery</name>
    ///     </typeparamref>
    ///     is either not a class or is not inheriting from
    ///     <see>
    ///         <cref>ObjectType`1</cref>
    ///     </see>
    ///     .
    ///     - A query type was already added.
    /// </exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var graphQLServer = builder.Services.AddGraphQLServer();
        if (builder.Environment.EnvironmentName is Envs.Development or Envs.Local)
            graphQLServer.ModifyRequestOptions(opt => { opt.IncludeExceptionDetails = true; });

        graphQLServer.AddFiltering().AddProjections().AddSorting().AddSpatialTypes().AddSpatialProjections()
            .AddSpatialFiltering()
            .AddQueryType<QueryProvider>()
            .AddType<PointSortInputType>()
            .AddType<PolygonSortInputType>()
            .RegisterDbContextFactory<ProjectDbContext>();
        return Task.CompletedTask;
    }
}