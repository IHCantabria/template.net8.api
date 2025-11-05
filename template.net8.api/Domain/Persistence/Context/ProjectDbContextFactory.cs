using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace template.net8.api.Domain.Persistence.Context;

/// <summary>
///     ProjectDbContextFactory class to get a new Context from the Pool
/// </summary>
public sealed class ProjectDbContextFactory(IDbContextFactory<ProjectDbContext> pooledFactory)
    : IDbContextFactory<ProjectDbContext>
{
    private readonly IDbContextFactory<ProjectDbContext> _pooledFactory =
        pooledFactory ?? throw new ArgumentNullException(nameof(pooledFactory));

    /// <summary>
    ///     Create a DbContext Instance
    /// </summary>
    /// <returns></returns>
    [MustDisposeResource]
    public ProjectDbContext CreateDbContext()
    {
        return _pooledFactory.CreateDbContext();
    }

    /// <summary>
    ///     Create a DbContext Instance Asynchronously
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [MustDisposeResource]
    public Task<ProjectDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
    {
        return _pooledFactory.CreateDbContextAsync(cancellationToken);
    }
}