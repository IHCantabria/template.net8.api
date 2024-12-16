using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Exceptions;
using template.net8.api.Domain.Base;
using template.net8.api.Domain.Persistence.Repositories.Interfaces;

namespace template.net8.api.Domain.Persistence.Repositories;

/// <summary>
///     Unit of Work pattern implementation.
/// </summary>
[CoreLibrary]
public sealed class UnitOfWork<TDbContext>(TDbContext context, ILogger<UnitOfWork<TDbContext>> logger)
    : DbRepositoryScopedDbContextBase(context, logger), IUnitOfWork<TDbContext>, IAsyncDisposable
    where TDbContext : DbContext
{
    private IDbContextTransaction? _transaction;

    /// <summary>
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync().ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     Get the current DbContext.
    /// </summary>
    /// <returns></returns>
    public TDbContext DbContext()
    {
        return (TDbContext)Context;
    }

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
    public async Task<Result<bool>> SaveChangesAsync(CancellationToken cancellationToken)
    {
        await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <summary>
    ///     Begin a new transaction.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<Result<bool>> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction is not null)
        {
            return new Result<bool>(new CoreException("There is a transaction already open!"));
        }

        _transaction = await context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <summary>
    ///     Commit the transaction.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<Result<bool>> CommitTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null)
        {
            return new Result<bool>(new CoreException("There is not a transaction active"));
        }

        await _transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <summary>
    ///     Rollback the transaction.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<Result<bool>> RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null)
        {
            return new Result<bool>(new CoreException("There is not a transaction active"));
        }

        await _transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <summary>
    ///     Release the transaction.
    /// </summary>
    /// <returns></returns>
    public async Task<Result<bool>> ReleaseTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null)
        {
            return new Result<bool>(new CoreException("There is not a transaction active"));
        }

        await _transaction.DisposeAsync().ConfigureAwait(false);
        _transaction = null;
        return true;
    }

    /// <summary>
    ///     Add a savepoint to the transaction.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<Result<bool>> AddSavepointAsync(string name, CancellationToken cancellationToken)
    {
        if (_transaction is null)
        {
            return new Result<bool>(new CoreException("There is not a transaction active"));
        }

        await _transaction.CreateSavepointAsync(name, cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <summary>
    ///     Rollback to a savepoint in the transaction.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<Result<bool>> RollbackToSavepointAsync(string name, CancellationToken cancellationToken)
    {
        if (_transaction is null)
        {
            return new Result<bool>(new CoreException("There is not a transaction active"));
        }

        await _transaction.RollbackToSavepointAsync(name, cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <summary>
    ///     Release a savepoint in the transaction.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<Result<bool>> ReleaseSavepointAsync(string name, CancellationToken cancellationToken)
    {
        if (_transaction is null)
        {
            return new Result<bool>(new CoreException("There is not a transaction active"));
        }

        await _transaction.ReleaseSavepointAsync(name, cancellationToken).ConfigureAwait(false);
        return true;
    }
}