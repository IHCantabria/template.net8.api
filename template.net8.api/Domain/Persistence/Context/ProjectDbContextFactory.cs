using Microsoft.EntityFrameworkCore;

namespace template.net8.Api.Domain.Persistence.Context;

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
    public ProjectDbContext CreateDbContext()
    {
        return _pooledFactory.CreateDbContext();
    }

    /// <summary>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public Task<ProjectDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
    {
        return _pooledFactory.CreateDbContextAsync(cancellationToken);
    }
}