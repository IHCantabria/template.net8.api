using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace template.net8.api.Persistence.Repositories.Interfaces;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification =
        "Repository interface methods are consumed indirectly through dependency injection and implementations.")]
internal interface IUnitOfWork<out TDbContext> where TDbContext : DbContext
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<bool>> SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<bool>> BeginTransactionAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<bool>> CommitTransactionAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<bool>> RollbackTransactionAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<bool>> ReleaseTransactionAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<bool>> AddSavepointAsync(string name, CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<bool>> RollbackToSavepointAsync(string name, CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<bool>> ReleaseSavepointAsync(string name, CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [MustDisposeResource]
    TDbContext DbContext();
}