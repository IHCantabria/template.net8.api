using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Attributes;
using template.net8.api.Domain.Base;
using template.net8.api.Domain.Persistence.Repositories.Interfaces;

namespace template.net8.api.Domain.Persistence.Repositories;

/// <summary>
///     Unit of Work pattern implementation.
/// </summary>
[CoreLibrary]
public sealed class UnitOfWork<TDbContext>(TDbContext context, ILogger<UnitOfWork<TDbContext>> logger)
    : DbRepositoryScopedDbContextBase(context, logger), IUnitOfWork<TDbContext> where TDbContext : DbContext
{
    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
    /// <exception cref="DbUpdateConcurrencyException">
    ///     A concurrency violation is encountered while saving to the database.
    ///     A concurrency violation occurs when an unexpected number of rows are affected during save.
    ///     This is usually because the data in the database has been modified since it was loaded into memory.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<Result<bool>> CompleteAsync(CancellationToken cancellationToken)
    {
        await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <summary>
    ///     Get the current DbContext.
    /// </summary>
    /// <returns></returns>
    public TDbContext DbContext()
    {
        return (TDbContext)Context;
    }
}