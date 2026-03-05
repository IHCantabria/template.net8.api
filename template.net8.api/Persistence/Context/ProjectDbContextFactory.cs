using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace template.net8.api.Persistence.Context;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class AppDbContextFactory(IDbContextFactory<AppDbContext> pooledFactory)
    : IDbContextFactory<AppDbContext>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IDbContextFactory<AppDbContext> _pooledFactory =
        pooledFactory ?? throw new ArgumentNullException(nameof(pooledFactory));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [MustDisposeResource]
    public AppDbContext CreateDbContext()
    {
        return _pooledFactory.CreateDbContext();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [MustDisposeResource]
    public Task<AppDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
    {
        return _pooledFactory.CreateDbContextAsync(cancellationToken);
    }
}