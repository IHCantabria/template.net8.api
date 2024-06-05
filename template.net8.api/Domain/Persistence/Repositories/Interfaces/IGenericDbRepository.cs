using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Attributes;
using template.net8.api.Domain.Interfaces;
using template.net8.api.Domain.Persistence.Models.Interfaces;
using template.net8.api.Domain.Specifications.Interfaces;

namespace template.net8.api.Domain.Persistence.Repositories.Interfaces;

/// <summary>
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
/// <typeparam name="TEntity"></typeparam>
[CoreLibrary]
public interface IGenericDbRepositoryScopedDbContext<TDbContext, TEntity>
    where TDbContext : DbContext where TEntity : class, IEntity
{
    /// <summary>
    /// </summary>
    /// <param name="verification"></param>
    /// <returns></returns>
    Result<bool> Verificate(IVerification<TEntity>? verification);

    /// <summary>
    /// </summary>
    /// <param name="verification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<bool>> VerificateAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken);

    /// <summary>
    /// </summary>
    /// <param name="verification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<bool>> VerificateSingleAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken);

    /// <summary>
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<ICollection<TEntity>>> GetAsync(ISpecification<TEntity>? specification,
        CancellationToken cancellationToken);

    /// <summary>
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    Task<Result<IEnumerable<TDto>>> GetAsync<TDto>(ISpecification<TEntity, TDto>? specification,
        CancellationToken cancellationToken)
        where TDto : class, IDto;

    /// <summary>
    ///     Get single entity by specification
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<TEntity>> GetSingleAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken);

    /// <summary>
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    public Task<Result<TDto>> GetSingleAsync<TDto>(ISpecification<TEntity, TDto> specification,
        CancellationToken cancellationToken)
        where TDto : class, IDto;

    /// <summary>
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result<TEntity>> InsertAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public Result<TEntity> Delete(TEntity entity);

    /// <summary>
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    public Result<TDto> Delete<TDto>(TEntity entity) where TDto : class, IDto;

    /// <summary>
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result<TEntity>> DeleteAsync(short entityId, CancellationToken cancellationToken);

    /// <summary>
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    public Task<Result<TDto>> DeleteAsync<TDto>(short entityId, CancellationToken cancellationToken)
        where TDto : class, IDto;

    /// <summary>
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public Result<TEntity> Update(TEntity entity);

    /// <summary>
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public Result<TDto> Update<TDto>(TEntity entity);

    /// <summary>
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task<Result<bool>> ExecuteQueryProcedureAsync(string procedureName,
        CancellationToken cancellationToken,
        params object[] parameters);


    /// <summary>
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task<Result<ICollection<TEntity>>> ExecuteQueryProcedureAsync(string procedureName,
        ISpecification<TEntity>? specification, CancellationToken cancellationToken,
        params object[] parameters);


    /// <summary>
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parameters"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    public Task<Result<IEnumerable<TDto>>> ExecuteQueryProcedureAsync<TDto>(string procedureName,
        ISpecification<TEntity, TDto>? specification, CancellationToken cancellationToken,
        params object[] parameters) where TDto : class, IDto;
}

/// <summary>
/// </summary>
[CoreLibrary]
public interface IGenericDbRepositoryScopedDbContext<TDbContext> where TDbContext : DbContext
{
    /// <summary>
    /// </summary>
    /// <param name="entitiesSpecifications"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDtoComposed"></typeparam>
    /// <returns></returns>
    public Task<Result<TDtoComposed>> GetComposedAsync<TDtoComposed>(
        IEnumerable<Tuple<string, object>> entitiesSpecifications, CancellationToken cancellationToken)
        where TDtoComposed : class, IDto, new();

    /// <summary>
    /// </summary>
    /// <param name="entitiesVerifications"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result<bool>> VerificateComposedAsync(
        IEnumerable<VerificationModel> entitiesVerifications, CancellationToken cancellationToken);
}

/// <summary>
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TDbContext"></typeparam>
[CoreLibrary]
public interface IGenericDbRepositoryTransientDbContext<TDbContext, TEntity>
    where TDbContext : DbContext where TEntity : class, IEntity
{
    /// <summary>
    /// </summary>
    /// <param name="verification"></param>
    /// <returns></returns>
    Result<bool> Verificate(IVerification<TEntity>? verification);

    /// <summary>
    /// </summary>
    /// <param name="verification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<bool>> VerificateAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken);

    /// <summary>
    /// </summary>
    /// <param name="verification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<bool>> VerificateSingleAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken);

    /// <summary>
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<ICollection<TEntity>>> GetAsync(ISpecification<TEntity>? specification,
        CancellationToken cancellationToken);

    /// <summary>
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    Task<Result<IEnumerable<TDto>>> GetAsync<TDto>(ISpecification<TEntity, TDto>? specification,
        CancellationToken cancellationToken)
        where TDto : class, IDto;

    /// <summary>
    ///     Get single entity by specification
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<TEntity>> GetSingleAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken);

    /// <summary>
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    public Task<Result<TDto>> GetSingleAsync<TDto>(ISpecification<TEntity, TDto> specification,
        CancellationToken cancellationToken)
        where TDto : class, IDto;

    /// <summary>
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task<Result<bool>> ExecuteQueryProcedureAsync(string procedureName,
        CancellationToken cancellationToken,
        params object[] parameters);


    /// <summary>
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task<Result<ICollection<TEntity>>> ExecuteQueryProcedureAsync(string procedureName,
        ISpecification<TEntity>? specification, CancellationToken cancellationToken,
        params object[] parameters);


    /// <summary>
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parameters"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    public Task<Result<IEnumerable<TDto>>> ExecuteQueryProcedureAsync<TDto>(string procedureName,
        ISpecification<TEntity, TDto>? specification, CancellationToken cancellationToken,
        params object[] parameters) where TDto : class, IDto;
}

/// <summary>
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
[CoreLibrary]
public interface IGenericDbRepositoryTransientDbContext<TDbContext>
    where TDbContext : DbContext
{
    /// <summary>
    /// </summary>
    /// <param name="entitiesSpecifications"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDtoComposed"></typeparam>
    /// <returns></returns>
    public Task<Result<TDtoComposed>> GetComposedAsync<TDtoComposed>(
        IEnumerable<Tuple<string, object>> entitiesSpecifications, CancellationToken cancellationToken)
        where TDtoComposed : class, IDto, new();

    /// <summary>
    /// </summary>
    /// <param name="entitiesVerifications"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result<bool>> VerificateComposedAsync(
        IEnumerable<VerificationModel> entitiesVerifications, CancellationToken cancellationToken);
}