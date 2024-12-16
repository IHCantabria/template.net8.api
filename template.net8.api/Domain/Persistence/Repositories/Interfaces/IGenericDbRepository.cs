using JetBrains.Annotations;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Interfaces;
using template.net8.api.Domain.Persistence.Models.Interfaces;
using template.net8.api.Domain.Specifications.Interfaces;

namespace template.net8.api.Domain.Persistence.Repositories.Interfaces;

/// <summary>
///     Interfaces for generic database repositories with specific entity DbSet and scoped DbContexts.
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
/// <typeparam name="TEntity"></typeparam>
[CoreLibrary]
public interface IGenericDbRepositoryScopedDbContext<out TDbContext, TEntity>
    where TDbContext : DbContext where TEntity : class, IEntity
{
    /// <summary>
    ///     Get the current DbContext.
    /// </summary>
    /// <returns></returns>
    [MustDisposeResource]
    TDbContext DbContext();

    /// <summary>
    ///     Synchronously verifies the entity that satisfies the verification.
    /// </summary>
    /// <param name="verification"></param>
    /// <returns></returns>
    Try<bool> Verificate(IVerification<TEntity>? verification);

    /// <summary>
    ///     Asynchronously verifies the entity that satisfies the verification.
    /// </summary>
    /// <param name="verification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<bool>> VerificateAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Asynchronously verifies the unique entity that satisfies the verification.
    /// </summary>
    /// <param name="verification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<bool>> VerificateSingleAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Asynchronously gets the entities that satisfy the specification.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<ICollection<TEntity>>> GetAsync(ISpecification<TEntity>? specification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Asynchronously gets the entities that satisfy the specification and maps them to DTOs.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    Task<Result<IEnumerable<TDto>>> GetAsync<TDto>(ISpecification<TEntity, TDto>? specification,
        CancellationToken cancellationToken)
        where TDto : class, IDto;

    /// <summary>
    ///     Asynchronously gets the unique entity that satisfies the specification.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<TEntity>> GetSingleAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Asynchronously gets the unique entity that satisfies the specification and maps it to a DTO.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    Task<Result<TDto>> GetSingleAsync<TDto>(ISpecification<TEntity, TDto> specification,
        CancellationToken cancellationToken)
        where TDto : class, IDto;

    /// <summary>
    ///     Asynchronously gets the first entity that satisfies the specification.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<TEntity>> GetFirstAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Asynchronously gets the first entity that satisfies the specification and maps it to a DTO.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    Task<Result<TDto>> GetFirstAsync<TDto>(ISpecification<TEntity, TDto> specification,
        CancellationToken cancellationToken)
        where TDto : class, IDto;

    /// <summary>
    ///     Synchronously gets the first entity that satisfies the specification and maps it to a DTO.
    /// </summary>
    /// <param name="specification"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    Try<TDto> GetFirst<TDto>(ISpecification<TEntity, TDto> specification)
        where TDto : class, IDto;

    /// <summary>
    ///     Asynchronously inserts the entity.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<TEntity>> InsertAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    ///     Asynchronously inserts the colelction entities.
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<bool>> InsertBulkAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    /// <summary>
    ///     Synchronously deletes the entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Try<TEntity> Delete(TEntity entity);

    /// <summary>
    ///     Asynchronously deletes the entity by its id
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<TEntity>> DeleteAsync(short entityId, CancellationToken cancellationToken);

    /// <summary>
    ///     Synchronously updates the entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Try<TEntity> Update(TEntity entity);

    /// <summary>
    ///     Asynchronously executes the query procedure.
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<Result<bool>> ExecuteQueryProcedureAsync(string procedureName,
        CancellationToken cancellationToken,
        params object[] parameters);

    /// <summary>
    ///     Asynchronously executes the query procedure and returns the entities.
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<Result<ICollection<TEntity>>> ExecuteQueryProcedureAsync(string procedureName,
        ISpecification<TEntity>? specification, CancellationToken cancellationToken,
        params object[] parameters);


    /// <summary>
    ///     Asynchronously executes the query procedure and maps the entities to DTOs.
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parameters"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    Task<Result<IEnumerable<TDto>>> ExecuteQueryProcedureAsync<TDto>(string procedureName,
        ISpecification<TEntity, TDto>? specification, CancellationToken cancellationToken,
        params object[] parameters) where TDto : class, IDto;
}

/// <summary>
///     Interfaces for generic database repositories with scoped DbContexts.
/// </summary>
[CoreLibrary]
public interface IGenericDbRepositoryScopedDbContext<out TDbContext> where TDbContext : DbContext
{
    /// <summary>
    ///     Get the current DbContext.
    /// </summary>
    /// <returns></returns>
    [MustDisposeResource]
    TDbContext DbContext();

    /// <summary>
    ///     Asynchronously gets the composed DTOs.
    /// </summary>
    /// <param name="entitiesSpecifications"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDtoComposed"></typeparam>
    /// <returns></returns>
    Task<Result<TDtoComposed>> GetComposedAsync<TDtoComposed>(
        IEnumerable<Tuple<string, object>> entitiesSpecifications, CancellationToken cancellationToken)
        where TDtoComposed : class, IDto, new();

    /// <summary>
    ///     Asynchronously verifies the composed entities.
    /// </summary>
    /// <param name="entitiesVerifications"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<bool>> VerificateComposedAsync(
        IEnumerable<VerificationModel> entitiesVerifications, CancellationToken cancellationToken);
}

/// <summary>
///     Interfaces for generic database repositories with specific entity DbSet and transient DbContexts.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TDbContext"></typeparam>
[CoreLibrary]
public interface IGenericDbRepositoryTransientDbContext<TDbContext, TEntity>
    where TDbContext : DbContext where TEntity : class, IEntity
{
    /// <summary>
    ///     Create a new DbContext.
    /// </summary>
    /// <returns></returns>
    [MustDisposeResource]
    TDbContext CreateDbContext();

    /// <summary>
    ///     Create a new DbContext Async.
    /// </summary>
    /// <returns></returns>
    /// T
    [MustDisposeResource]
    Task<TDbContext> CreateDbContextAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Synchronously verifies the entity that satisfies the verification.
    /// </summary>
    /// <param name="verification"></param>
    /// <returns></returns>
    Try<bool> Verificate(IVerification<TEntity>? verification);

    /// <summary>
    ///     Asynchronously verifies the entity that satisfies the verification.
    /// </summary>
    /// <param name="verification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<bool>> VerificateAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Asynchronously verifies the unique entity that satisfies the verification.
    /// </summary>
    /// <param name="verification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<bool>> VerificateSingleAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Asynchronously gets the entities that satisfy the specification.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<ICollection<TEntity>>> GetAsync(ISpecification<TEntity>? specification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Asynchronously gets the entities that satisfy the specification and maps them to DTOs.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    Task<Result<IEnumerable<TDto>>> GetAsync<TDto>(ISpecification<TEntity, TDto>? specification,
        CancellationToken cancellationToken)
        where TDto : class, IDto;

    /// <summary>
    ///     Asynchronously gets the unique entity that satisfies the specification.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<TEntity>> GetSingleAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Asynchronously gets the unique entity that satisfies the specification and maps it to a DTO.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    Task<Result<TDto>> GetSingleAsync<TDto>(ISpecification<TEntity, TDto> specification,
        CancellationToken cancellationToken)
        where TDto : class, IDto;

    /// <summary>
    ///     Asynchronously gets the first entity that satisfies the specification.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<TEntity>> GetFirstAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Asynchronously gets the first entity that satisfies the specification and maps it to a DTO.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    Task<Result<TDto>> GetFirstAsync<TDto>(ISpecification<TEntity, TDto> specification,
        CancellationToken cancellationToken)
        where TDto : class, IDto;

    /// <summary>
    ///     Synchronously gets the first entity that satisfies the specification and maps it to a DTO.
    /// </summary>
    /// <param name="specification"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    Try<TDto> GetFirst<TDto>(ISpecification<TEntity, TDto> specification)
        where TDto : class, IDto;

    /// <summary>
    ///     Asynchronously executes the query procedure.
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<Result<bool>> ExecuteQueryProcedureAsync(string procedureName,
        CancellationToken cancellationToken,
        params object[] parameters);


    /// <summary>
    ///     Asynchronously executes the query procedure and returns the entities.
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<Result<ICollection<TEntity>>> ExecuteQueryProcedureAsync(string procedureName,
        ISpecification<TEntity>? specification, CancellationToken cancellationToken,
        params object[] parameters);


    /// <summary>
    ///     Asynchronously executes the query procedure and maps the entities to DTOs.
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parameters"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    Task<Result<IEnumerable<TDto>>> ExecuteQueryProcedureAsync<TDto>(string procedureName,
        ISpecification<TEntity, TDto>? specification, CancellationToken cancellationToken,
        params object[] parameters) where TDto : class, IDto;
}

/// <summary>
///     Interfaces for generic database repositories with transient DbContexts.
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
[CoreLibrary]
public interface IGenericDbRepositoryTransientDbContext<TDbContext>
    where TDbContext : DbContext
{
    /// <summary>
    ///     Create a new DbContext.
    /// </summary>
    /// <returns></returns>
    [MustDisposeResource]
    TDbContext CreateDbContext();

    /// <summary>
    ///     Create a new DbContext Async.
    /// </summary>
    /// <returns></returns>
    /// T
    [MustDisposeResource]
    Task<TDbContext> CreateDbContextAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Asynchronously gets the composed DTOs.
    /// </summary>
    /// <param name="entitiesSpecifications"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDtoComposed"></typeparam>
    /// <returns></returns>
    Task<Result<TDtoComposed>> GetComposedAsync<TDtoComposed>(
        IEnumerable<Tuple<string, object>> entitiesSpecifications, CancellationToken cancellationToken)
        where TDtoComposed : class, IDto, new();

    /// <summary>
    ///     Asynchronously verifies the composed entities.
    /// </summary>
    /// <param name="entitiesVerifications"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<bool>> VerificateComposedAsync(
        IEnumerable<VerificationModel> entitiesVerifications, CancellationToken cancellationToken);
}