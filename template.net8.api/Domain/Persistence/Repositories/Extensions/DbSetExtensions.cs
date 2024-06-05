using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Attributes;
using template.net8.api.Domain.Persistence.Models.Interfaces;

namespace template.net8.api.Domain.Persistence.Repositories.Extensions;

[CoreLibrary]
internal static class DbSetExtensions
{
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    internal static async ValueTask<TEntity?> FindItemAsync<TEntity>(this DbSet<TEntity> set, params object[] keyValues)
        where TEntity : class, IEntity
    {
        return keyValues[^1] is CancellationToken ct
            ? await set.FindAsync(keyValues[..^1], ct).ConfigureAwait(false)
            : await set.FindAsync(keyValues).ConfigureAwait(false);
    }
}