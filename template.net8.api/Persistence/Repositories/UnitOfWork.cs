using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using template.net8.api.Core.Base;
using template.net8.api.Core.Exceptions;
using template.net8.api.Persistence.Repositories.Interfaces;

namespace template.net8.api.Persistence.Repositories;

/// <inheritdoc cref="IUnitOfWork{TDbContext}" />
[MustDisposeResource]
internal sealed class UnitOfWork<TDbContext>(TDbContext context, ILogger<UnitOfWork<TDbContext>> logger)
    : StatefulRepositoryBase(context, logger), IUnitOfWork<TDbContext>, IAsyncDisposable
    where TDbContext : DbContext
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private IDbContextTransaction? _transaction;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_transaction != null) await _transaction.DisposeAsync().ConfigureAwait(false);
    }

    /// <inheritdoc cref="IUnitOfWork{TDbContext}.DbContext" />
    public TDbContext DbContext()
    {
        return (TDbContext)Context;
    }

    /// <inheritdoc cref="IUnitOfWork{TDbContext}.SaveChangesAsync" />
    /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
    /// <exception cref="DbUpdateConcurrencyException">
    ///     A concurrency violation is encountered while saving to the database.
    ///     A concurrency violation occurs when an unexpected number of rows are affected during save.
    ///     This is usually because the data in the database has been modified since it was loaded into memory.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<bool>> SaveChangesAsync(CancellationToken cancellationToken)
    {
        await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <inheritdoc cref="IUnitOfWork{TDbContext}.BeginTransactionAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<bool>> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction is not null)
            return new LanguageExt.Common.Result<bool>(new CoreException("There is a transaction already open!"));

        _transaction = await Context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <inheritdoc cref="IUnitOfWork{TDbContext}.CommitTransactionAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<bool>> CommitTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null)
            return new LanguageExt.Common.Result<bool>(new CoreException("There is not a transaction active"));

        await _transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <inheritdoc cref="IUnitOfWork{TDbContext}.RollbackTransactionAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<bool>> RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null)
            return new LanguageExt.Common.Result<bool>(new CoreException("There is not a transaction active"));

        await _transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <inheritdoc cref="IUnitOfWork{TDbContext}.ReleaseTransactionAsync" />
    public async Task<LanguageExt.Common.Result<bool>> ReleaseTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null)
            return new LanguageExt.Common.Result<bool>(new CoreException("There is not a transaction active"));

        await _transaction.DisposeAsync().ConfigureAwait(false);
        _transaction = null;
        return true;
    }

    /// <inheritdoc cref="IUnitOfWork{TDbContext}.AddSavepointAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<bool>> AddSavepointAsync(string name,
        CancellationToken cancellationToken)
    {
        if (_transaction is null)
            return new LanguageExt.Common.Result<bool>(new CoreException("There is not a transaction active"));

        await _transaction.CreateSavepointAsync(name, cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <inheritdoc cref="IUnitOfWork{TDbContext}.RollbackToSavepointAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<bool>> RollbackToSavepointAsync(string name,
        CancellationToken cancellationToken)
    {
        if (_transaction is null)
            return new LanguageExt.Common.Result<bool>(new CoreException("There is not a transaction active"));

        await _transaction.RollbackToSavepointAsync(name, cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <inheritdoc cref="IUnitOfWork{TDbContext}.ReleaseSavepointAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<bool>> ReleaseSavepointAsync(string name,
        CancellationToken cancellationToken)
    {
        if (_transaction is null)
            return new LanguageExt.Common.Result<bool>(new CoreException("There is not a transaction active"));

        await _transaction.ReleaseSavepointAsync(name, cancellationToken).ConfigureAwait(false);
        return true;
    }
}