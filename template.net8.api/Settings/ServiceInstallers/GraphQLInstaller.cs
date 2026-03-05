using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using template.net8.api.GraphQL;
using template.net8.api.GraphQL.Types;
using template.net8.api.Persistence.Context;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class GraphQLInstaller : IServiceInstaller
{
    /// <inheritdoc cref="IServiceInstaller.LoadOrder" />
    public short LoadOrder => 25;

    /// <inheritdoc cref="IServiceInstaller.InstallServiceAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var graphQLServer = builder.Services.AddGraphQLServer();
        if (builder.Environment.EnvironmentName is Envs.Development or Envs.Local)
            graphQLServer.ModifyRequestOptions(static opt => opt.IncludeExceptionDetails = true);

        graphQLServer.AddFiltering().AddProjections().AddSorting().AddSpatialTypes().AddSpatialProjections()
            .AddSpatialFiltering()
            .AddQueryType<QueryProvider>()
            .AddType<PointSortInputType>()
            .AddType<PolygonSortInputType>()
            .RegisterDbContextFactory<AppDbContext>();
        return Task.CompletedTask;
    }
}