using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Interfaces;
using template.net8.api.Persistence.Models.Interfaces;

namespace template.net8.api.Persistence.Repositories.Interfaces;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification =
        "Repository interface methods are consumed indirectly through dependency injection and implementations.")]
internal interface IGenericDbRepositoryWriteContext<out TDbContext, TEntity>
    where TDbContext : DbContext where TEntity : class, IEntity
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [MustDisposeResource]
    TDbContext DbContext();

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Try<bool> Verificate(IVerification<TEntity>? verification);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<bool>> VerificateAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<bool>> VerificateSingleAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<ICollection<TEntity>>> GetAsync(ISpecification<TEntity>? specification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<IEnumerable<TDto>>> GetAsync<TDto>(ISpecification<TEntity>? specification,
        IProjection<TEntity, TDto> projection,
        CancellationToken cancellationToken)
        where TDto : class, IDto;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<TEntity>> GetSingleAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<TDto>> GetSingleAsync<TDto>(ISpecification<TEntity> specification,
        IProjection<TEntity, TDto> projection,
        CancellationToken cancellationToken)
        where TDto : class, IDto;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<TEntity>> GetFirstAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<TDto>> GetFirstAsync<TDto>(ISpecification<TEntity> specification,
        IProjection<TEntity, TDto> projection,
        CancellationToken cancellationToken)
        where TDto : class, IDto;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Try<TDto> GetFirst<TDto>(ISpecification<TEntity> specification, IProjection<TEntity, TDto> projection)
        where TDto : class, IDto;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<TEntity>> InsertAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<bool>> InsertBulkAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Try<TEntity> Delete(TEntity entity);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<TEntity>> DeleteAsync<TKey>(TKey? entityKey, CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Try<TEntity> Update(TEntity entity);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<bool>> ExecuteProcedureAsync(IProcedureCall procedureCall,
        CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<ICollection<TEntity>>> ExecuteQueryProcedureAsync(IProcedureCall procedureCall,
        ISpecification<TEntity>? specification, CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<IEnumerable<TDto>>> ExecuteQueryProcedureAsync<TDto>(IProcedureCall procedureCall,
        ISpecification<TEntity>? specification, IProjection<TEntity, TDto> projection,
        CancellationToken cancellationToken) where TDto : class, IDto;
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification =
        "Repository interface methods are consumed indirectly through dependency injection and implementations.")]
internal interface IGenericDbRepositoryReadContext<TDbContext, TEntity>
    where TDbContext : DbContext where TEntity : class, IEntity
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [MustDisposeResource]
    Task<TDbContext> CreateDbContextAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Try<bool> Verificate(IVerification<TEntity>? verification);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<bool>> VerificateAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<bool>> VerificateSingleAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<ICollection<TEntity>>> GetAsync(ISpecification<TEntity>? specification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<IEnumerable<TDto>>> GetAsync<TDto>(ISpecification<TEntity>? specification,
        IProjection<TEntity, TDto> projection,
        CancellationToken cancellationToken)
        where TDto : class, IDto;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<TEntity>> GetSingleAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<TDto>> GetSingleAsync<TDto>(ISpecification<TEntity> specification,
        IProjection<TEntity, TDto> projection,
        CancellationToken cancellationToken)
        where TDto : class, IDto;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<TEntity>> GetFirstAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<TDto>> GetFirstAsync<TDto>(ISpecification<TEntity> specification,
        IProjection<TEntity, TDto> projection,
        CancellationToken cancellationToken)
        where TDto : class, IDto;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Try<TDto> GetFirst<TDto>(ISpecification<TEntity> specification, IProjection<TEntity, TDto> projection)
        where TDto : class, IDto;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<bool>> ExecuteProcedureAsync(IProcedureCall procedureCall,
        CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<ICollection<TEntity>>> ExecuteQueryProcedureAsync(IProcedureCall procedureCall,
        ISpecification<TEntity>? specification, CancellationToken cancellationToken);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task<LanguageExt.Common.Result<IEnumerable<TDto>>> ExecuteQueryProcedureAsync<TDto>(IProcedureCall procedureCall,
        ISpecification<TEntity>? specification, IProjection<TEntity, TDto> projection,
        CancellationToken cancellationToken) where TDto : class, IDto;
}