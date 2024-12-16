using JetBrains.Annotations;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Domain.Persistence.Repositories.Interfaces;

/// <summary>
///     Interface for the Unit of Work pattern.
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
[CoreLibrary]
public interface IUnitOfWork<out TDbContext> where TDbContext : DbContext
{
    /// <summary>
    ///     Save the pending changes in the repository.
    /// </summary>
    /// <returns></returns>
    Task<Result<bool>> SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Begin a new transaction.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<bool>> BeginTransactionAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Commit the transaction.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<Result<bool>> CommitTransactionAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Rollback the transaction.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<Result<bool>> RollbackTransactionAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Release the transaction.
    /// </summary>
    /// <returns></returns>
    Task<Result<bool>> ReleaseTransactionAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Add a savepoint to the transaction.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<Result<bool>> AddSavepointAsync(string name, CancellationToken cancellationToken);

    /// <summary>
    ///     Rollback to a savepoint in the transaction.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<Result<bool>> RollbackToSavepointAsync(string name, CancellationToken cancellationToken);

    /// <summary>
    ///     Release a savepoint in the transaction.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<Result<bool>> ReleaseSavepointAsync(string name, CancellationToken cancellationToken);

    /// <summary>
    ///     Get the current DbContext.
    /// </summary>
    /// <returns></returns>
    [MustDisposeResource]
    TDbContext DbContext();
}