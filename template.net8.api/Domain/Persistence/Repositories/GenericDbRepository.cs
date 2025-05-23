﻿using System.Collections;
using System.Reflection;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JetBrains.Annotations;
using LanguageExt;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Extensions;
using template.net8.api.Core.Interfaces;
using template.net8.api.Domain.Base;
using template.net8.api.Domain.Persistence.Models.Interfaces;
using template.net8.api.Domain.Persistence.Repositories.Extensions;
using template.net8.api.Domain.Persistence.Repositories.Interfaces;

namespace template.net8.api.Domain.Persistence.Repositories;

/// <summary>
///     Generic Repository Scoped DbContext with specific DbSet
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TDbContext"></typeparam>
[CoreLibrary]
public sealed class GenericDbRepositoryScopedDbContext<TDbContext, TEntity>(
    TDbContext context,
    IMapper mapper,
    ILogger<GenericDbRepositoryScopedDbContext<TDbContext, TEntity>> logger)
    : DbRepositoryScopedDbContextBase(context, logger),
        IGenericDbRepositoryScopedDbContext<TDbContext, TEntity>
    where TDbContext : DbContext where TEntity : class, IEntity
{
    private const string EmptyQuery = "Query return empty result";
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    /// <summary>
    ///     Get the current DbContext.
    /// </summary>
    public TDbContext DbContext()
    {
        return (TDbContext)Context;
    }

    /// <summary>
    ///     Synchronously verifies the entity that satisfies the verification.
    /// </summary>
    /// <param name="verification"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public Try<bool> Verificate(IVerification<TEntity>? verification)
    {
        return () =>
        {
            var queryable = _dbSet.AsExpandable();
            var query = queryable.ApplyVerification(verification);
            return query.Any();
        };
    }

    /// <summary>
    ///     Asynchronously verifies the entity that satisfies the verification.
    /// </summary>
    /// <param name="verification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<bool>> VerificateAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken)
    {
        var queryable = _dbSet.AsExpandable();
        var query = queryable.ApplyVerification(verification);
        return await query.AnyAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Asynchronously verifies the unique entity that satisfies the verification.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="verification"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<bool>> VerificateSingleAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken)
    {
        var queryable = _dbSet.AsExpandable();
        var query = queryable.ApplyVerification(verification);
        return await query.Take(2).CountAsync(cancellationToken).ConfigureAwait(false) == 1;
    }

    /// <summary>
    ///     Asynchronously gets the entities that satisfy the specification.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="InvalidOperationException">
    ///     The
    ///     <see>
    ///         <cref>P:System.Nullable`1.HasValue</cref>
    ///     </see>
    ///     property is <see langword="false" />.
    /// </exception>
    public async Task<LanguageExt.Common.Result<ICollection<TEntity>>> GetAsync(
        ISpecification<TEntity>? specification, CancellationToken cancellationToken)
    {
        var queryable = _dbSet.AsExpandable();
        var query = queryable.ApplySpecification(specification);
        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Asynchronously gets the entities that satisfy the specification and maps them to DTOs.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="specification"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="InvalidOperationException">
    ///     The
    ///     <see>
    ///         <cref>P:System.Nullable`1.HasValue</cref>
    ///     </see>
    ///     property is <see langword="false" />.
    /// </exception>
    public async Task<LanguageExt.Common.Result<IEnumerable<TDto>>> GetAsync<TDto>(
        ISpecification<TEntity, TDto>? specification,
        CancellationToken cancellationToken)
        where TDto : class, IDto
    {
        var queryable = _dbSet.AsExpandable();
        var query = queryable.ApplySpecification(specification);
        var projection = specification is null
            ? query.ProjectTo<TDto>(_mapper.ConfigurationProvider)
            : query.ProjectTo(_mapper.ConfigurationProvider, specification.MapperObjectParams,
                specification.MembersToExpand.ToArray());
        return await projection.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Asynchronously gets the unique entity that satisfies the specification.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     contains more than one element.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<TEntity>> GetSingleAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken)
    {
        var queryable = _dbSet.AsExpandable();
        var query = queryable.ApplySpecification(specification);
        var entity = await query.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        return entity ?? new LanguageExt.Common.Result<TEntity>(new CoreException(EmptyQuery));
    }

    /// <summary>
    ///     Asynchronously gets the unique entity that satisfies the specification and maps it to a DTO.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     contains more than one element.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<TDto>> GetSingleAsync<TDto>(ISpecification<TEntity, TDto> specification,
        CancellationToken cancellationToken)
        where TDto : class, IDto
    {
        var queryable = _dbSet.AsExpandable();
        var query = queryable.ApplySpecification(specification);
        var projection = specification is null
            ? query.ProjectTo<TDto>(_mapper.ConfigurationProvider)
            : query.ProjectTo(_mapper.ConfigurationProvider, specification.MapperObjectParams,
                specification.MembersToExpand.ToArray());
        var dto = await projection.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        return dto ?? new LanguageExt.Common.Result<TDto>(new CoreException(EmptyQuery));
    }

    /// <summary>
    ///     Asynchronously inserts the entity.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<TEntity>> InsertAsync(TEntity entity,
        CancellationToken cancellationToken)
    {
        var newEntity = await _dbSet.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        return newEntity.Entity;
    }

    /// <summary>
    ///     Asynchronously inserts the collection entities.
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<bool>> InsertBulkAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <summary>
    ///     Asynchronously executes the query procedure.
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<bool>> ExecuteQueryProcedureAsync(string procedureName,
        CancellationToken cancellationToken, params object[] parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        var queryable = PrepareProcedureQueryable(procedureName, parameters);
        await queryable.ToListAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <summary>
    ///     Asynchronously executes the query procedure and returns the entities.
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="InvalidOperationException">
    ///     The
    ///     <see>
    ///         <cref>P:System.Nullable`1.HasValue</cref>
    ///     </see>
    ///     property is <see langword="false" />.
    /// </exception>
    public async Task<LanguageExt.Common.Result<ICollection<TEntity>>> ExecuteQueryProcedureAsync(string procedureName,
        ISpecification<TEntity>? specification,
        CancellationToken cancellationToken,
        params object[] parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        var queryable = PrepareProcedureQueryable(procedureName, parameters);
        var query = queryable.ApplySpecification(specification);
        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Asynchronously executes the query procedure and maps the entities to DTOs.
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parameters"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="InvalidOperationException">
    ///     The
    ///     <see>
    ///         <cref>P:System.Nullable`1.HasValue</cref>
    ///     </see>
    ///     property is <see langword="false" />.
    /// </exception>
    public async Task<LanguageExt.Common.Result<IEnumerable<TDto>>> ExecuteQueryProcedureAsync<TDto>(
        string procedureName,
        ISpecification<TEntity, TDto>? specification,
        CancellationToken cancellationToken,
        params object[] parameters) where TDto : class, IDto
    {
        ArgumentNullException.ThrowIfNull(parameters);
        var queryable = PrepareProcedureQueryable(procedureName, parameters);
        var query = queryable.ApplySpecification(specification);
        var projection = specification is null
            ? query.ProjectTo<TDto>(_mapper.ConfigurationProvider)
            : query.ProjectTo(_mapper.ConfigurationProvider, specification.MapperObjectParams,
                specification.MembersToExpand.ToArray());
        return await projection.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Asynchronously gets the first entity that satisfies the specification.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="InvalidOperationException">
    ///     The
    ///     <see>
    ///         <cref>P:System.Nullable`1.HasValue</cref>
    ///     </see>
    ///     property is <see langword="false" />.
    /// </exception>
    public async Task<LanguageExt.Common.Result<TEntity>> GetFirstAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken)
    {
        var queryable = _dbSet.AsExpandable();
        var query = queryable.ApplySpecification(specification);
        var entity = await query.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        return entity ?? new LanguageExt.Common.Result<TEntity>(new CoreException(EmptyQuery));
    }

    /// <summary>
    ///     Asynchronously gets the first entity that satisfies the specification and maps it to a DTO.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="InvalidOperationException">
    ///     The
    ///     <see>
    ///         <cref>P:System.Nullable`1.HasValue</cref>
    ///     </see>
    ///     property is <see langword="false" />.
    /// </exception>
    public async Task<LanguageExt.Common.Result<TDto>> GetFirstAsync<TDto>(ISpecification<TEntity, TDto> specification,
        CancellationToken cancellationToken)
        where TDto : class, IDto
    {
        var queryable = _dbSet.AsExpandable();
        var query = queryable.ApplySpecification(specification);
        var projection = specification is null
            ? query.ProjectTo<TDto>(_mapper.ConfigurationProvider)
            : query.ProjectTo(_mapper.ConfigurationProvider, specification.MapperObjectParams,
                specification.MembersToExpand.ToArray());
        var dto = await projection.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        return dto ?? new LanguageExt.Common.Result<TDto>(new CoreException(EmptyQuery));
    }

    /// <summary>
    ///     Synchronously deletes the entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public Try<TEntity> Delete(TEntity entity)
    {
        return () =>
        {
            if (Context.Entry(entity).State == EntityState.Detached) _dbSet.Attach(entity);

            _dbSet.Remove(entity);
            return entity;
        };
    }

    /// <summary>
    ///     Synchronously updates the entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     or
    ///     <paramref>
    ///         <name>predicate</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public Try<TEntity> Update(TEntity entity)
    {
        return () =>
        {
            var entry = Context.Attach(entity);
            //Should be Serial
            foreach (var ee in Context.ChangeTracker.Entries().Where(ee => ee.State == EntityState.Unchanged))
                ee.State = EntityState.Modified;

            return entry.Entity;
        };
    }

    /// <summary>
    ///     Synchronously gets the first entity that satisfies the specification and maps it to a DTO.
    /// </summary>
    /// <param name="specification"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     The
    ///     <see>
    ///         <cref>P:System.Nullable`1.HasValue</cref>
    ///     </see>
    ///     property is <see langword="false" />.
    /// </exception>
    public Try<TDto> GetFirst<TDto>(ISpecification<TEntity, TDto> specification)
        where TDto : class, IDto
    {
        return () =>
        {
            var queryable = _dbSet.AsExpandable();
            var query = queryable.ApplySpecification(specification);
            var projection = specification is null
                ? query.ProjectTo<TDto>(_mapper.ConfigurationProvider)
                : query.ProjectTo(_mapper.ConfigurationProvider, specification.MapperObjectParams,
                    specification.MembersToExpand.ToArray());
            var dto = projection.FirstOrDefault();
            return dto ?? new LanguageExt.Common.Result<TDto>(new CoreException(EmptyQuery));
        };
    }

    /// <summary>
    ///     Asynchronously deletes the entity by its key
    /// </summary>
    /// <param name="entityKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<TEntity>> DeleteAsync<TKey>(TKey? entityKey,
        CancellationToken cancellationToken)
    {
        var entity = await _dbSet.FindItemAsync(entityKey, cancellationToken).ConfigureAwait(false);
        if (entity is null)
            return new LanguageExt.Common.Result<TEntity>(
                new CoreException($"Entity with key:({entityKey}) not found"));

        return Delete(entity).Try();
    }

    private IQueryable<TEntity> PrepareProcedureQueryable(string procedureName, params object[] parameters)
    {
        return Context.Set<TEntity>().FromSql($"{procedureName} {parameters}").AsExpandable();
    }
}

/// <summary>
///     Generic Repository Scoped DbContext
/// </summary>
[CoreLibrary]
//TODO : Refactor this, not used and redundant
public sealed class GenericDbRepositoryScopedDbContext<TDbContext>(
    TDbContext context,
    IMapper mapper,
    ILogger<GenericDbRepositoryScopedDbContext<TDbContext>> logger) : DbRepositoryScopedDbContextBase(
    context, logger), IGenericDbRepositoryScopedDbContext<TDbContext>
    where TDbContext : DbContext
{
    private const string EmptyQuery = "Query return empty result";
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    /// <summary>
    ///     Get the current DbContext.
    /// </summary>
    public TDbContext DbContext()
    {
        return (TDbContext)Context;
    }

    /// <summary>
    ///     Asynchronously gets the composed DTOs.
    /// </summary>
    /// <param name="entitiesSpecifications"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDtoComposed"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="AmbiguousMatchException">More than one property is found with the specified name.</exception>
    /// <exception cref="ArgumentException">Condition.</exception>
    public async Task<LanguageExt.Common.Result<TDtoComposed>> GetComposedAsync<TDtoComposed>(
        IEnumerable<Tuple<string, object>> entitiesSpecifications, CancellationToken cancellationToken)
        where TDtoComposed : class, IDto, new()
    {
        ArgumentNullException.ThrowIfNull(entitiesSpecifications);
        var dataDto = new TDtoComposed();
        var type = dataDto.GetType();
        //Should be Serial
        foreach (var (propertyName, specification) in entitiesSpecifications)
        {
            var property = type.GetProperty(propertyName) ?? throw new ArgumentException(
                $"The property object {propertyName} is not defined in the class {type.FullName}");

            await ProcessEntitySpecificationAsync(dataDto, property, specification, cancellationToken)
                .ConfigureAwait(false);
        }

        return dataDto;
    }

    /// <summary>
    ///     Asynchronously verifies the composed entities.
    /// </summary>
    /// <param name="entitiesVerifications"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="ResultSuccessInvalidOperationException">
    ///     Result is not a success! Use ExtractException method instead
    ///     and Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractException method
    /// </exception>
    /// <exception cref="OutOfMemoryException">
    ///     The length of the resulting string overflows the maximum allowed length (
    ///     <see cref="System.Int32.MaxValue">Int32.MaxValue</see>).
    /// </exception>
    public async Task<LanguageExt.Common.Result<bool>> VerificateComposedAsync(
        IEnumerable<VerificationModel> entitiesVerifications, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entitiesVerifications);
        var validationErrors = new List<string>();
        // MUST BE SERIAL, SCOPED CONTEXT
        foreach (var model in entitiesVerifications)
        {
            var taskResponse = await VerificateDataAsync(model.IsUnique, model.Verification, cancellationToken)
                .ConfigureAwait(false);
            if (taskResponse is LanguageExt.Common.Result<bool> sucessResult &&
                sucessResult.ExtractData() != model.ExpecteResult)
                validationErrors.Add(model.Msg);
        }

        return validationErrors.Count == 0
            ? new LanguageExt.Common.Result<bool>(new CoreException(string.Join(";", validationErrors)))
            : new LanguageExt.Common.Result<bool>(true);
    }

    private async Task ProcessEntitySpecificationAsync<TDtoComposed>(
        TDtoComposed dataDto, PropertyInfo property, object specification,
        CancellationToken cancellationToken = default)
        where TDtoComposed : class, IDto, new()
    {
        var isCollection = typeof(IEnumerable).IsAssignableFrom(property.PropertyType);
        var taskResponse = await GetDataAsync(isCollection, specification, cancellationToken)
            .ConfigureAwait(false);
        var taskResponseData = taskResponse.GetType().GetProperty("Data");
        var data = taskResponseData!.GetValue(taskResponse)!;
        property.SetValue(dataDto, data);
    }

    private async Task<object> GetDataAsync(bool isCollection, object specification,
        CancellationToken cancellationToken = default)
    {
        //TODO : Refactor this to use Caller and UnsafeAccessor logic see Nick Chapsas video https://www.youtube.com/watch?v=WqeSRUXJ9VM
        var methodName = isCollection ? "GetAsync" : "GetSingleAsync";

        // Construct a MethodInfo object for the method with the appropriate type arguments
        var getAsyncMethod = GetRepositoryMethod(methodName);
        var genericArguments = GetGenericArguments(specification);
        var getAsyncMethodTyped = getAsyncMethod.MakeGenericMethod(genericArguments);
        // Invoke the generic method using reflection and await the result
        var resultTask = (Task)getAsyncMethodTyped.Invoke(this, [specification, cancellationToken])!;
        await resultTask.ConfigureAwait(false);

        // Get the result of the Task using reflection and return it as an object
        return GetTaskResult(resultTask);
    }

    private async Task<object> VerificateDataAsync(bool isUnique, object verification,
        CancellationToken cancellationToken = default)
    {
        //TODO : Refactor this to use Caller and UnsafeAccessor logic see Nick Chapsas video https://www.youtube.com/watch?v=WqeSRUXJ9VM
        var methodName = isUnique ? "VerificateSingleAsync" : "VerificateAsync";

        var getAsyncMethod = GetRepositoryMethod(methodName);
        var genericArguments = GetGenericArguments(verification);
        var getAsyncMethodTyped = getAsyncMethod.MakeGenericMethod(genericArguments);

        var resultTask = (Task)getAsyncMethodTyped.Invoke(this, [verification, cancellationToken])!;
        await resultTask.ConfigureAwait(false);

        return GetTaskResult(resultTask);
    }

    private static MethodInfo GetRepositoryMethod(string methodName)
    {
        return typeof(GenericDbRepositoryScopedDbContext<TDbContext>).GetMethod(methodName,
            BindingFlags.Instance) ?? throw new InvalidOperationException();
    }

    private static Type[] GetGenericArguments(object specification)
    {
        var typeSpecification = specification.GetType();
        var specificationBase = GetSpecificationBase(typeSpecification);
        return specificationBase.GetGenericArguments();
    }

    private static Type GetSpecificationBase(Type type)
    {
        //Get Ancestor Type until reach BaseSpecification
        while (IsSpecificationBaseType(type)) type = type?.BaseType!;

        return type;
    }

    private static bool IsSpecificationBaseType(Type type)
    {
        return type.BaseType is not null &&
               !(type.Name == typeof(SpecificationBase<,>).Name ||
                 type.Name == typeof(SpecificationBase<>).Name ||
                 type.Name == typeof(VerificationBase<>).Name);
    }

    private static object GetTaskResult(Task task)
    {
        var resultProperty = task.GetType().GetProperty("Result");
        return resultProperty!.GetValue(task)!;
    }

    /// <summary>
    ///     Get the entities that satisfy the specification.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="InvalidOperationException">
    ///     The
    ///     <see>
    ///         <cref>P:System.Nullable`1.HasValue</cref>
    ///     </see>
    ///     property is <see langword="false" />.
    /// </exception>
    [UsedImplicitly]
    public async Task<LanguageExt.Common.Result<IEnumerable<TDto>>> GetAsync<TEntity, TDto>(
        ISpecification<TEntity, TDto>? specification, CancellationToken cancellationToken)
        where TEntity : class, IEntity where TDto : class, IDto
    {
        var queryable = Context.Set<TEntity>().AsExpandable();
        var query = queryable.ApplySpecification(specification);
        var projection = specification is null
            ? query.ProjectTo<TDto>(_mapper.ConfigurationProvider)
            : query.ProjectTo(_mapper.ConfigurationProvider, specification.MapperObjectParams,
                specification.MembersToExpand.ToArray());
        var dtos = await projection.ToListAsync(cancellationToken).ConfigureAwait(false);
        return dtos;
    }

    /// <summary>
    ///     Get the unique entity that satisfies the specification.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     contains more than one element.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [UsedImplicitly]
    public async Task<LanguageExt.Common.Result<TDto>> GetSingleAsync<TEntity, TDto>(
        ISpecification<TEntity, TDto> specification, CancellationToken cancellationToken)
        where TEntity : class, IEntity where TDto : class, IDto
    {
        var queryable = Context.Set<TEntity>().AsExpandable();
        var query = queryable.ApplySpecification(specification);
        var projection = specification is null
            ? query.ProjectTo<TDto>(_mapper.ConfigurationProvider)
            : query.ProjectTo(_mapper.ConfigurationProvider, specification.MapperObjectParams,
                specification.MembersToExpand.ToArray());
        var dto = await projection.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        return dto ?? new LanguageExt.Common.Result<TDto>(new CoreException(EmptyQuery));
    }


    /// <summary>
    ///     Verificate the entity that satisfies the verification.
    /// </summary>
    /// <param name="verification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    [UsedImplicitly]
    public async Task<LanguageExt.Common.Result<bool>> VerificateAsync<TEntity>(IVerification<TEntity>? verification,
        CancellationToken cancellationToken) where TEntity : class, IEntity
    {
        var queryable = Context.Set<TEntity>().AsExpandable();
        var query = queryable.ApplyVerification(verification);
        var entityExist = await query.AnyAsync(cancellationToken).ConfigureAwait(false);
        return entityExist;
    }


    /// <summary>
    ///     Verificate the unique entity that satisfies the verification.
    /// </summary>
    /// <param name="verification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [UsedImplicitly]
    public async Task<LanguageExt.Common.Result<bool>> VerificateSingleAsync<TEntity>(
        IVerification<TEntity>? verification,
        CancellationToken cancellationToken) where TEntity : class, IEntity
    {
        var queryable = Context.Set<TEntity>().AsExpandable();
        var query = queryable.ApplyVerification(verification);
        var entityUniqueExist =
            await query.Take(2).CountAsync(cancellationToken).ConfigureAwait(false) == 1;
        return entityUniqueExist;
    }
}

/// <summary>
///     Generic Repository Transient DbContext with specific DbSet
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TDbContext"></typeparam>
[CoreLibrary]
public sealed class GenericDbRepositoryTransientDbContext<TDbContext, TEntity>(
    IDbContextFactory<TDbContext> dbContextFactory,
    IMapper mapper,
    ILogger<GenericDbRepositoryTransientDbContext<TDbContext, TEntity>> logger)
    : DbRepositoryTransientDbContextBase(logger),
        IGenericDbRepositoryTransientDbContext<TDbContext, TEntity>
    where TDbContext : DbContext where TEntity : class, IEntity
{
    private const string EmptyQuery = "Query return empty result";

    private readonly IDbContextFactory<TDbContext> _dbContextFactory =
        dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    /// <summary>
    ///     Create a new DbContext.
    /// </summary>
    public TDbContext CreateDbContext()
    {
        return _dbContextFactory.CreateDbContext();
    }

    /// <summary>
    ///     Create a new DbContext Async.
    /// </summary>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public Task<TDbContext> CreateDbContextAsync(CancellationToken cancellationToken)
    {
        return _dbContextFactory.CreateDbContextAsync(cancellationToken);
    }

    /// <summary>
    ///     Asynchronously verifies the entity that satisfies the verification.
    /// </summary>
    /// <param name="verification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public async Task<LanguageExt.Common.Result<bool>> VerificateAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken)
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var queryable = context.Set<TEntity>().AsExpandable();
        var query = queryable.ApplyVerification(verification);
        var entityExist = await query.AnyAsync(cancellationToken).ConfigureAwait(false);
        return entityExist;
    }

    /// <summary>
    ///     Asynchronously verifies the unique entity that satisfies the verification.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="verification"></param>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public async Task<LanguageExt.Common.Result<bool>> VerificateSingleAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken)
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var queryable = context.Set<TEntity>().AsExpandable();
        var query = queryable.ApplyVerification(verification);
        var entityUniqueExist =
            await query.Take(2).CountAsync(cancellationToken).ConfigureAwait(false) == 1;
        return entityUniqueExist;
    }

    /// <summary>
    ///     Asynchronously gets the entities that satisfy the specification.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     The
    ///     <see>
    ///         <cref>P:System.Nullable`1.HasValue</cref>
    ///     </see>
    ///     property is <see langword="false" />.
    /// </exception>
    public async Task<LanguageExt.Common.Result<ICollection<TEntity>>> GetAsync(
        ISpecification<TEntity>? specification, CancellationToken cancellationToken)
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var queryable = context.Set<TEntity>().AsExpandable();
        var query = queryable.ApplySpecification(specification);
        var entities = await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        return entities;
    }

    /// <summary>
    ///     Asynchronously gets the entities that satisfy the specification and maps them to DTOs.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="specification"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     The
    ///     <see>
    ///         <cref>P:System.Nullable`1.HasValue</cref>
    ///     </see>
    ///     property is <see langword="false" />.
    /// </exception>
    public async Task<LanguageExt.Common.Result<IEnumerable<TDto>>> GetAsync<TDto>(
        ISpecification<TEntity, TDto>? specification,
        CancellationToken cancellationToken)
        where TDto : class, IDto
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var queryable = context.Set<TEntity>().AsExpandable();
        var query = queryable.ApplySpecification(specification);
        var projection = specification is null
            ? query.ProjectTo<TDto>(_mapper.ConfigurationProvider)
            : query.ProjectTo(_mapper.ConfigurationProvider, specification.MapperObjectParams,
                specification.MembersToExpand.ToArray());
        var dtos = await projection.ToListAsync(cancellationToken).ConfigureAwait(false);
        return dtos;
    }

    /// <summary>
    ///     Asynchronously gets the unique entity that satisfies the specification.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     contains more than one element.
    /// </exception>
    public async Task<LanguageExt.Common.Result<TEntity>> GetSingleAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken)
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var queryable = context.Set<TEntity>().AsExpandable();
        var query = queryable.ApplySpecification(specification);
        var entity = await query.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        return entity ?? new LanguageExt.Common.Result<TEntity>(new CoreException(EmptyQuery));
    }

    /// <summary>
    ///     Asynchronously gets the unique entity that satisfies the specification and maps it to a DTO.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     contains more than one element.
    /// </exception>
    public async Task<LanguageExt.Common.Result<TDto>> GetSingleAsync<TDto>(ISpecification<TEntity, TDto> specification,
        CancellationToken cancellationToken)
        where TDto : class, IDto
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var queryable = context.Set<TEntity>().AsExpandable();
        var query = queryable.ApplySpecification(specification);
        var projection = specification is null
            ? query.ProjectTo<TDto>(_mapper.ConfigurationProvider)
            : query.ProjectTo(_mapper.ConfigurationProvider, specification.MapperObjectParams,
                specification.MembersToExpand.ToArray());
        var dto = await projection.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        return dto ?? new LanguageExt.Common.Result<TDto>(new CoreException(EmptyQuery));
    }

    /// <summary>
    ///     Asynchronously gets the first entity that satisfies the specification.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     The
    ///     <see>
    ///         <cref>P:System.Nullable`1.HasValue</cref>
    ///     </see>
    ///     property is <see langword="false" />.
    /// </exception>
    public async Task<LanguageExt.Common.Result<TEntity>> GetFirstAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken)
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var queryable = context.Set<TEntity>().AsExpandable();
        var query = queryable.ApplySpecification(specification);
        var entity = await query.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        return entity ?? new LanguageExt.Common.Result<TEntity>(new CoreException(EmptyQuery));
    }

    /// <summary>
    ///     Asynchronously gets the first entity that satisfies the specification and maps it to a DTO.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     The
    ///     <see>
    ///         <cref>P:System.Nullable`1.HasValue</cref>
    ///     </see>
    ///     property is <see langword="false" />.
    /// </exception>
    public async Task<LanguageExt.Common.Result<TDto>> GetFirstAsync<TDto>(ISpecification<TEntity, TDto> specification,
        CancellationToken cancellationToken)
        where TDto : class, IDto
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var queryable = context.Set<TEntity>().AsExpandable();
        var query = queryable.ApplySpecification(specification);
        var projection = specification is null
            ? query.ProjectTo<TDto>(_mapper.ConfigurationProvider)
            : query.ProjectTo(_mapper.ConfigurationProvider, specification.MapperObjectParams,
                specification.MembersToExpand.ToArray());
        var dto = await projection.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        return dto ?? new LanguageExt.Common.Result<TDto>(new CoreException(EmptyQuery));
    }

    /// <summary>
    ///     Asynchronously executes the query procedure.
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<bool>> ExecuteQueryProcedureAsync(string procedureName,
        CancellationToken cancellationToken,
        params object[] parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        var queryable = PrepareProcedureQueryable(procedureName, parameters);
        await queryable.ToListAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <summary>
    ///     Execute Query Procedure Async
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="InvalidOperationException">
    ///     The
    ///     <see>
    ///         <cref>P:System.Nullable`1.HasValue</cref>
    ///     </see>
    ///     property is <see langword="false" />.
    /// </exception>
    public async Task<LanguageExt.Common.Result<ICollection<TEntity>>> ExecuteQueryProcedureAsync(string procedureName,
        ISpecification<TEntity>? specification,
        CancellationToken cancellationToken,
        params object[] parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        var queryable = PrepareProcedureQueryable(procedureName, parameters);
        var query = queryable.ApplySpecification(specification);
        var result = await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    ///     Execute Query Procedure Async
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parameters"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="InvalidOperationException">
    ///     The
    ///     <see>
    ///         <cref>P:System.Nullable`1.HasValue</cref>
    ///     </see>
    ///     property is <see langword="false" />.
    /// </exception>
    public async Task<LanguageExt.Common.Result<IEnumerable<TDto>>> ExecuteQueryProcedureAsync<TDto>(
        string procedureName,
        ISpecification<TEntity, TDto>? specification,
        CancellationToken cancellationToken,
        params object[] parameters) where TDto : class, IDto
    {
        ArgumentNullException.ThrowIfNull(parameters);
        var queryable = PrepareProcedureQueryable(procedureName, parameters);
        var query = queryable.ApplySpecification(specification);
        var projection = specification is null
            ? query.ProjectTo<TDto>(_mapper.ConfigurationProvider)
            : query.ProjectTo(_mapper.ConfigurationProvider, specification.MapperObjectParams,
                specification.MembersToExpand.ToArray());
        var result = await projection.ToListAsync(cancellationToken).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    ///     Synchronously verifies the entity that satisfies the verification.
    /// </summary>
    /// <param name="verification"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public Try<bool> Verificate(IVerification<TEntity>? verification)
    {
        return () =>
        {
            using var context = CreateDbContext();
            var queryable = context.Set<TEntity>().AsExpandable();
            var query = queryable.ApplyVerification(verification);
            var entityExist = query.Any();
            return entityExist;
        };
    }

    /// <summary>
    ///     Synchronously gets the first entity that satisfies the specification and maps it to a DTO.
    /// </summary>
    /// <param name="specification"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     The
    ///     <see>
    ///         <cref>P:System.Nullable`1.HasValue</cref>
    ///     </see>
    ///     property is <see langword="false" />.
    /// </exception>
    public Try<TDto> GetFirst<TDto>(ISpecification<TEntity, TDto> specification)
        where TDto : class, IDto
    {
        return () =>
        {
            using var context = CreateDbContext();
            var queryable = context.Set<TEntity>().AsExpandable();
            var query = queryable.ApplySpecification(specification);
            var projection = specification is null
                ? query.ProjectTo<TDto>(_mapper.ConfigurationProvider)
                : query.ProjectTo(_mapper.ConfigurationProvider, specification.MapperObjectParams,
                    specification.MembersToExpand.ToArray());
            var dto = projection.FirstOrDefault();
            return dto ?? new LanguageExt.Common.Result<TDto>(new CoreException(EmptyQuery));
        };
    }

    private IQueryable<TEntity> PrepareProcedureQueryable(string procedureName, params object[] parameters)
    {
        using var context = CreateDbContext();
        var queryable = context.Set<TEntity>().FromSql($"{procedureName} {parameters}");
        return queryable;
    }
}

/// <summary>
///     Generic Repository Transient DbContext
/// </summary>
[CoreLibrary]
//TODO : Refactor this, not used and redundant
public sealed class GenericDbRepositoryTransientDbContext<TDbContext>(
    IDbContextFactory<TDbContext> dbContextFactory,
    IMapper mapper,
    ILogger<GenericDbRepositoryTransientDbContext<TDbContext>> logger)
    : DbRepositoryTransientDbContextBase(logger), IGenericDbRepositoryTransientDbContext<TDbContext>
    where TDbContext : DbContext
{
    private const string EmptyQuery = "Query return empty result";

    private readonly IDbContextFactory<TDbContext> _dbContextFactory =
        dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    /// <summary>
    ///     Create a new DbContext.
    /// </summary>
    public TDbContext CreateDbContext()
    {
        return _dbContextFactory.CreateDbContext();
    }

    /// <summary>
    ///     Create a new DbContext Async.
    /// </summary>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public Task<TDbContext> CreateDbContextAsync(CancellationToken cancellationToken)
    {
        return _dbContextFactory.CreateDbContextAsync(cancellationToken);
    }

    /// <summary>
    ///     Asynchronously gets the composed DTOs.
    /// </summary>
    /// <param name="entitiesSpecifications"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDtoComposed"></typeparam>
    /// <returns></returns>
    /// <exception cref="AmbiguousMatchException">More than one property is found with the specified name.</exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>name</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">Condition.</exception>
    public async Task<LanguageExt.Common.Result<TDtoComposed>> GetComposedAsync<TDtoComposed>(
        IEnumerable<Tuple<string, object>> entitiesSpecifications, CancellationToken cancellationToken)
        where TDtoComposed : class, IDto, new()
    {
        ArgumentNullException.ThrowIfNull(entitiesSpecifications);
        var dataDto = new TDtoComposed();
        var type = dataDto.GetType();
        //Should be Serial
        foreach (var (propertyName, specification) in entitiesSpecifications)
        {
            var property = type.GetProperty(propertyName) ?? throw new ArgumentException(
                $"The property object {propertyName} is not defined in the class {type.FullName}");

            await ProcessEntitySpecificationAsync(dataDto, property, specification, cancellationToken)
                .ConfigureAwait(false);
        }

        return dataDto;
    }

    /// <summary>
    ///     Asynchronously verifies the composed entities.
    /// </summary>
    /// <param name="entitiesVerifications"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="ResultSuccessInvalidOperationException">
    ///     Result is not a success! Use ExtractException method instead
    ///     and Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractException method
    /// </exception>
    /// <exception cref="OutOfMemoryException">
    ///     The length of the resulting string overflows the maximum allowed length (
    ///     <see cref="System.Int32.MaxValue">Int32.MaxValue</see>).
    /// </exception>
    public async Task<LanguageExt.Common.Result<bool>> VerificateComposedAsync(
        IEnumerable<VerificationModel> entitiesVerifications, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entitiesVerifications);
        var validationErrors = new List<string>();
        foreach (var model in entitiesVerifications)
        {
            var taskResponse = await VerificateDataAsync(model.IsUnique, model.Verification, cancellationToken)
                .ConfigureAwait(false);
            if (taskResponse is LanguageExt.Common.Result<bool> sucessResult &&
                sucessResult.ExtractData() != model.ExpecteResult)
                validationErrors.Add(model.Msg);
        }

        return validationErrors.Count == 0
            ? new LanguageExt.Common.Result<bool>(new CoreException(string.Join(";", validationErrors)))
            : new LanguageExt.Common.Result<bool>(true);
    }

    private async Task ProcessEntitySpecificationAsync<TDtoComposed>(
        TDtoComposed dataDto, PropertyInfo property, object specification,
        CancellationToken cancellationToken = default)
        where TDtoComposed : class, IDto, new()
    {
        var isCollection = typeof(IEnumerable).IsAssignableFrom(property.PropertyType);
        var taskResponse = await GetDataAsync(isCollection, specification, cancellationToken)
            .ConfigureAwait(false);
        var taskResponseData = taskResponse.GetType().GetProperty("Data");
        var data = taskResponseData!.GetValue(taskResponse)!;
        property.SetValue(dataDto, data);
    }

    private async Task<object> GetDataAsync(bool isCollection, object specification,
        CancellationToken cancellationToken = default)
    {
        //TODO : Refactor this to use Caller and UnsafeAccessor logic see Nick Chapsas video https://www.youtube.com/watch?v=WqeSRUXJ9VM
        var methodName = isCollection ? "GetAsync" : "GetSingleAsync";

        // Construct a MethodInfo object for the method with the appropriate type arguments
        var getAsyncMethod = GetRepositoryMethod(methodName);
        var genericArguments = GetGenericArguments(specification);
        var getAsyncMethodTyped = getAsyncMethod.MakeGenericMethod(genericArguments);
        // Invoke the generic method using reflection and await the result
        var resultTask = (Task)getAsyncMethodTyped.Invoke(this, [specification, cancellationToken])!;
        await resultTask.ConfigureAwait(false);

        // Get the result of the Task using reflection and return it as an object
        return GetTaskResult(resultTask);
    }

    private async Task<object> VerificateDataAsync(bool isUnique, object verification,
        CancellationToken cancellationToken = default)
    {
        //TODO : Refactor this to use Caller and UnsafeAccessor logic see Nick Chapsas video https://www.youtube.com/watch?v=WqeSRUXJ9VM
        var methodName = isUnique ? "VerificateSingleAsync" : "VerificateAsync";

        var getAsyncMethod = GetRepositoryMethod(methodName);
        var genericArguments = GetGenericArguments(verification);
        var getAsyncMethodTyped = getAsyncMethod.MakeGenericMethod(genericArguments);

        var resultTask = (Task)getAsyncMethodTyped.Invoke(this, [verification, cancellationToken])!;
        await resultTask.ConfigureAwait(false);

        return GetTaskResult(resultTask);
    }

    private static MethodInfo GetRepositoryMethod(string methodName)
    {
        return typeof(GenericDbRepositoryTransientDbContext<TDbContext>).GetMethod(methodName,
            BindingFlags.Instance) ?? throw new InvalidOperationException();
    }

    private static Type[] GetGenericArguments(object specification)
    {
        var typeSpecification = specification.GetType();
        var specificationBase = GetSpecificationBase(typeSpecification);
        return specificationBase.GetGenericArguments();
    }

    private static Type GetSpecificationBase(Type type)
    {
        //Get Ancestor Type until reach BaseSpecification
        while (IsSpecificationBaseType(type)) type = type?.BaseType!;

        return type;
    }

    private static bool IsSpecificationBaseType(Type type)
    {
        return type.BaseType is not null &&
               !(type.Name == typeof(SpecificationBase<,>).Name ||
                 type.Name == typeof(SpecificationBase<>).Name ||
                 type.Name == typeof(VerificationBase<>).Name);
    }

    private static object GetTaskResult(Task task)
    {
        var resultProperty = task.GetType().GetProperty("Result");
        return resultProperty!.GetValue(task)!;
    }

    /// <summary>
    ///     Get the entities that satisfy the specification.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     The
    ///     <see>
    ///         <cref>P:System.Nullable`1.HasValue</cref>
    ///     </see>
    ///     property is <see langword="false" />.
    /// </exception>
    [UsedImplicitly]
    public async Task<LanguageExt.Common.Result<IEnumerable<TDto>>> GetAsync<TEntity, TDto>(
        ISpecification<TEntity, TDto>? specification, CancellationToken cancellationToken)
        where TEntity : class, IEntity where TDto : class, IDto
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var queryable = context.Set<TEntity>().AsExpandable();
        var query = queryable.ApplySpecification(specification);
        var projection = specification is null
            ? query.ProjectTo<TDto>(_mapper.ConfigurationProvider)
            : query.ProjectTo(_mapper.ConfigurationProvider, specification.MapperObjectParams,
                specification.MembersToExpand.ToArray());
        var dtos = await projection.ToListAsync(cancellationToken).ConfigureAwait(false);
        return dtos;
    }

    /// <summary>
    ///     Get the unique entity that satisfies the specification.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     contains more than one element.
    /// </exception>
    [UsedImplicitly]
    public async Task<LanguageExt.Common.Result<TDto>> GetSingleAsync<TEntity, TDto>(
        ISpecification<TEntity, TDto> specification, CancellationToken cancellationToken)
        where TEntity : class, IEntity where TDto : class, IDto
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var queryable = context.Set<TEntity>().AsExpandable();
        var query = queryable.ApplySpecification(specification);
        var projection = specification is null
            ? query.ProjectTo<TDto>(_mapper.ConfigurationProvider)
            : query.ProjectTo(_mapper.ConfigurationProvider, specification.MapperObjectParams,
                specification.MembersToExpand.ToArray());
        var dto = await projection.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        return dto ?? new LanguageExt.Common.Result<TDto>(new CoreException(EmptyQuery));
    }

    /// <summary>
    ///     Verificate Async
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="verification"></param>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    [UsedImplicitly]
    public async Task<LanguageExt.Common.Result<bool>> VerificateAsync<TEntity>(IVerification<TEntity>? verification,
        CancellationToken cancellationToken) where TEntity : class, IEntity
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var queryable = context.Set<TEntity>().AsExpandable();
        var query = queryable.ApplyVerification(verification);
        var entityExist = await query.AnyAsync(cancellationToken).ConfigureAwait(false);
        return entityExist;
    }

    /// <summary>
    ///     Verificate Single Async
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="verification"></param>
    /// <returns></returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    [UsedImplicitly]
    public async Task<LanguageExt.Common.Result<bool>> VerificateSingleAsync<TEntity>(
        IVerification<TEntity>? verification,
        CancellationToken cancellationToken) where TEntity : class, IEntity
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var queryable = context.Set<TEntity>().AsExpandable();
        var query = queryable.ApplyVerification(verification);
        var entityUniqueExist =
            await query.Take(2).CountAsync(cancellationToken).ConfigureAwait(false) == 1;
        return entityUniqueExist;
    }
}