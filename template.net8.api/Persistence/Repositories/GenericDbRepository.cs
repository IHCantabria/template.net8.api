using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Base;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Extensions;
using template.net8.api.Core.Interfaces;
using template.net8.api.Persistence.Models.Interfaces;
using template.net8.api.Persistence.Repositories.Extensions;
using template.net8.api.Persistence.Repositories.Interfaces;

namespace template.net8.api.Persistence.Repositories;

/// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}" />
internal sealed class GenericDbRepositoryWriteContext<TDbContext, TEntity>(
    TDbContext context,
    ILogger<GenericDbRepositoryWriteContext<TDbContext, TEntity>> logger)
    : StatefulRepositoryBase(context, logger),
        IGenericDbRepositoryWriteContext<TDbContext, TEntity>
    where TDbContext : DbContext where TEntity : class, IEntity
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.DbContext" />
    public TDbContext DbContext()
    {
        return (TDbContext)Context;
    }

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.Verificate" />
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public Try<bool> Verificate(IVerification<TEntity>? verification)
    {
        return () =>
        {
            var query = _dbSet.ApplyVerification(verification);
            return query.Any();
        };
    }

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.VerificateAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public async Task<LanguageExt.Common.Result<bool>> VerificateAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken)
    {
        var query = _dbSet.ApplyVerification(verification);
        return await query.AnyAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.VerificateSingleAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public async Task<LanguageExt.Common.Result<bool>> VerificateSingleAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken)
    {
        var query = _dbSet.ApplyVerification(verification);
        return await query.Take(2).CountAsync(cancellationToken).ConfigureAwait(false) == 1;
    }

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.GetAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public async Task<LanguageExt.Common.Result<ICollection<TEntity>>> GetAsync(
        ISpecification<TEntity>? specification, CancellationToken cancellationToken)
    {
        var query = _dbSet.ApplySpecification(specification);
        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.GetAsync{TDTO}" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="projection" /> is <see langword="null" />.</exception>
    public async Task<LanguageExt.Common.Result<IEnumerable<TDto>>> GetAsync<TDto>(
        ISpecification<TEntity>? specification,
        IProjection<TEntity, TDto> projection,
        CancellationToken cancellationToken)
        where TDto : class, IDto
    {
        ArgumentNullException.ThrowIfNull(projection);
        var query = _dbSet.ApplySpecification(specification).ApplyProjection(projection);
        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.InsertAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<TEntity>> InsertAsync(TEntity entity,
        CancellationToken cancellationToken)
    {
        var newEntity = await _dbSet.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        return newEntity.Entity;
    }

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.InsertBulkAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public async Task<LanguageExt.Common.Result<bool>> InsertBulkAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.Delete" />
    public Try<TEntity> Delete(TEntity entity)
    {
        return () =>
        {
            if (Context.Entry(entity).State == EntityState.Detached) _dbSet.Attach(entity);

            _dbSet.Remove(entity);
            return entity;
        };
    }

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.Update" />
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public Try<TEntity> Update(TEntity entity)
    {
        return () =>
        {
            var entry = Context.Attach(entity);
            //Should be Serial
            foreach (var ee in Context.ChangeTracker.Entries().Where(static ee => ee.State == EntityState.Unchanged))
                ee.State = EntityState.Modified;

            return entry.Entity;
        };
    }

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.DeleteAsync" />
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

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.GetSingleAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public async Task<LanguageExt.Common.Result<TEntity>> GetSingleAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken)
    {
        var query = _dbSet.ApplySpecification(specification);
        return await query.SingleAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.GetSingleAsync{TDto}" />
    /// <exception cref="ArgumentNullException"><paramref name="projection" /> is <see langword="null" />.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public async Task<LanguageExt.Common.Result<TDto>> GetSingleAsync<TDto>(ISpecification<TEntity> specification,
        IProjection<TEntity, TDto> projection,
        CancellationToken cancellationToken)
        where TDto : class, IDto
    {
        ArgumentNullException.ThrowIfNull(projection);
        var query = _dbSet.ApplySpecification(specification).ApplyProjection(projection);
        return await query.SingleAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.GetFirst{TDto}" />
    /// <exception cref="InvalidOperationException">The source sequence is empty.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public Try<TDto> GetFirst<TDto>(ISpecification<TEntity> specification, IProjection<TEntity, TDto> projection)
        where TDto : class, IDto
    {
        return () =>
        {
            var query = _dbSet.ApplySpecification(specification).ApplyProjection(projection);
            return query.First();
        };
    }

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.GetFirstAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public async Task<LanguageExt.Common.Result<TEntity>> GetFirstAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken)
    {
        var query = _dbSet.ApplySpecification(specification);
        return await query.FirstAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.GetFirstAsync{TDto}" />
    /// <exception cref="ArgumentNullException"><paramref name="projection" /> is <see langword="null" />.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public async Task<LanguageExt.Common.Result<TDto>> GetFirstAsync<TDto>(ISpecification<TEntity> specification,
        IProjection<TEntity, TDto> projection,
        CancellationToken cancellationToken)
        where TDto : class, IDto
    {
        ArgumentNullException.ThrowIfNull(projection);
        var query = _dbSet.ApplySpecification(specification).ApplyProjection(projection);
        return await query.FirstAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.ExecuteProcedureAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="procedureCall" /> is <see langword="null" />.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<bool>> ExecuteProcedureAsync(IProcedureCall procedureCall,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(procedureCall);
        var queryable = PrepareProcedureQueryable(procedureCall.ProcedureName, procedureCall.Parameters.ToArray());
        await queryable.ToListAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.ExecuteQueryProcedureAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="procedureCall" /> is <see langword="null" />.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<ICollection<TEntity>>> ExecuteQueryProcedureAsync(
        IProcedureCall procedureCall,
        ISpecification<TEntity>? specification,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(procedureCall);
        var queryable = PrepareProcedureQueryable(procedureCall.ProcedureName, procedureCall.Parameters.ToArray());
        var query = queryable.ApplySpecification(specification);
        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IGenericDbRepositoryWriteContext{TDbContext,TEntity}.ExecuteQueryProcedureAsync{TDto}" />
    /// <exception cref="ArgumentNullException"><paramref name="procedureCall" /> is <see langword="null" />.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<IEnumerable<TDto>>> ExecuteQueryProcedureAsync<TDto>(
        IProcedureCall procedureCall,
        ISpecification<TEntity>? specification, IProjection<TEntity, TDto> projection,
        CancellationToken cancellationToken) where TDto : class, IDto
    {
        ArgumentNullException.ThrowIfNull(projection);
        ArgumentNullException.ThrowIfNull(procedureCall);
        var queryable = PrepareProcedureQueryable(procedureCall.ProcedureName, procedureCall.Parameters.ToArray());
        var query = queryable.ApplySpecification(specification).ApplyProjection(projection);
        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private IQueryable<TEntity> PrepareProcedureQueryable(string procedureName, params object[] parameters)
    {
        return Context.Set<TEntity>().FromSql($"{procedureName} {parameters}");
    }
}

/// <inheritdoc cref="IGenericDbRepositoryReadContext{TDbContext,TEntity}" />
internal sealed class GenericDbRepositoryReadContext<TDbContext, TEntity>(
    IDbContextFactory<TDbContext> dbContextFactory,
    ILogger<GenericDbRepositoryReadContext<TDbContext, TEntity>> logger)
    : StatelessRepositoryBase(logger),
        IGenericDbRepositoryReadContext<TDbContext, TEntity>
    where TDbContext : DbContext where TEntity : class, IEntity
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IDbContextFactory<TDbContext> _dbContextFactory =
        dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));

    /// <inheritdoc cref="IGenericDbRepositoryReadContext{TDbContext,TEntity}.CreateDbContextAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public Task<TDbContext> CreateDbContextAsync(CancellationToken cancellationToken)
    {
        return _dbContextFactory.CreateDbContextAsync(cancellationToken);
    }

    /// <inheritdoc cref="IGenericDbRepositoryReadContext{TDbContext,TEntity}.Verificate" />
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public Try<bool> Verificate(IVerification<TEntity>? verification)
    {
        return () =>
        {
            using var context = CreateDbContext();
            var query = context.Set<TEntity>().ApplyVerification(verification);
            return query.Any();
        };
    }

    /// <inheritdoc cref="IGenericDbRepositoryReadContext{TDbContext,TEntity}.VerificateAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public async Task<LanguageExt.Common.Result<bool>> VerificateAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken)
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var query = context.Set<TEntity>().ApplyVerification(verification);
        return await query.AnyAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IGenericDbRepositoryReadContext{TDbContext,TEntity}.VerificateSingleAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public async Task<LanguageExt.Common.Result<bool>> VerificateSingleAsync(IVerification<TEntity>? verification,
        CancellationToken cancellationToken)
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var query = context.Set<TEntity>().ApplyVerification(verification);
        return await query.Take(2).CountAsync(cancellationToken).ConfigureAwait(false) == 1;
    }

    /// <inheritdoc cref="IGenericDbRepositoryReadContext{TDbContext,TEntity}.GetAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public async Task<LanguageExt.Common.Result<ICollection<TEntity>>> GetAsync(
        ISpecification<TEntity>? specification, CancellationToken cancellationToken)
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var query = context.Set<TEntity>().ApplySpecification(specification);
        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IGenericDbRepositoryReadContext{TDbContext,TEntity}.GetAsync{TDto}" />
    /// <exception cref="ArgumentNullException"><paramref name="projection" /> is <see langword="null" />.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<IEnumerable<TDto>>> GetAsync<TDto>(
        ISpecification<TEntity>? specification,
        IProjection<TEntity, TDto> projection,
        CancellationToken cancellationToken)
        where TDto : class, IDto
    {
        ArgumentNullException.ThrowIfNull(projection);
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var query = context.Set<TEntity>().ApplySpecification(specification).ApplyProjection(projection);
        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IGenericDbRepositoryReadContext{TDbContext,TEntity}.GetSingleAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public async Task<LanguageExt.Common.Result<TEntity>> GetSingleAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken)
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var query = context.Set<TEntity>().ApplySpecification(specification);
        return await query.SingleAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IGenericDbRepositoryReadContext{TDbContext,TEntity}.GetSingleAsync{TDto}" />
    /// <exception cref="ArgumentNullException"><paramref name="projection" /> is <see langword="null" />.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public async Task<LanguageExt.Common.Result<TDto>> GetSingleAsync<TDto>(ISpecification<TEntity> specification,
        IProjection<TEntity, TDto> projection,
        CancellationToken cancellationToken)
        where TDto : class, IDto
    {
        ArgumentNullException.ThrowIfNull(projection);
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var query = context.Set<TEntity>().ApplySpecification(specification).ApplyProjection(projection);
        return await query.SingleAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IGenericDbRepositoryReadContext{TDbContext,TEntity}.GetFirst" />
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public Try<TDto> GetFirst<TDto>(ISpecification<TEntity> specification, IProjection<TEntity, TDto> projection)
        where TDto : class, IDto
    {
        return () =>
        {
            using var context = CreateDbContext();
            var query = context.Set<TEntity>().ApplySpecification(specification).ApplyProjection(projection);
            return query.First();
        };
    }

    /// <inheritdoc cref="IGenericDbRepositoryReadContext{TDbContext,TEntity}.GetFirstAsync" />
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public async Task<LanguageExt.Common.Result<TEntity>> GetFirstAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken)
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var query = context.Set<TEntity>().ApplySpecification(specification);
        return await query.FirstAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IGenericDbRepositoryReadContext{TDbContext,TEntity}.GetFirstAsync{TDto}" />
    /// <exception cref="ArgumentNullException"><paramref name="projection" /> is <see langword="null" />.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public async Task<LanguageExt.Common.Result<TDto>> GetFirstAsync<TDto>(ISpecification<TEntity> specification,
        IProjection<TEntity, TDto> projection,
        CancellationToken cancellationToken)
        where TDto : class, IDto
    {
        ArgumentNullException.ThrowIfNull(projection);
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);
        var query = context.Set<TEntity>().ApplySpecification(specification).ApplyProjection(projection);
        return await query.FirstAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IGenericDbRepositoryReadContext{TDbContext,TEntity}.ExecuteProcedureAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="procedureCall" /> is <see langword="null" />.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<bool>> ExecuteProcedureAsync(IProcedureCall procedureCall,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(procedureCall);
        var queryable = PrepareProcedureQueryable(procedureCall.ProcedureName, procedureCall.Parameters.ToArray());
        await queryable.ToListAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <inheritdoc cref="IGenericDbRepositoryReadContext{TDbContext,TEntity}.ExecuteQueryProcedureAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="procedureCall" /> is <see langword="null" />.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<ICollection<TEntity>>> ExecuteQueryProcedureAsync(
        IProcedureCall procedureCall,
        ISpecification<TEntity>? specification,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(procedureCall);
        var queryable = PrepareProcedureQueryable(procedureCall.ProcedureName, procedureCall.Parameters.ToArray());
        var query = queryable.ApplySpecification(specification);
        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IGenericDbRepositoryReadContext{TDbContext,TEntity}.ExecuteQueryProcedureAsync{TDto}" />
    /// <exception cref="ArgumentNullException"><paramref name="procedureCall" /> is <see langword="null" />.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<LanguageExt.Common.Result<IEnumerable<TDto>>> ExecuteQueryProcedureAsync<TDto>(
        IProcedureCall procedureCall,
        ISpecification<TEntity>? specification,
        IProjection<TEntity, TDto> projection,
        CancellationToken cancellationToken) where TDto : class, IDto
    {
        ArgumentNullException.ThrowIfNull(projection);
        ArgumentNullException.ThrowIfNull(procedureCall);
        var queryable = PrepareProcedureQueryable(procedureCall.ProcedureName, procedureCall.Parameters.ToArray());
        var query = queryable.ApplySpecification(specification).ApplyProjection(projection);
        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [MustDisposeResource]
    private TDbContext CreateDbContext()
    {
        return _dbContextFactory.CreateDbContext();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private IQueryable<TEntity> PrepareProcedureQueryable(string procedureName, params object[] parameters)
    {
        using var context = CreateDbContext();
        return context.Set<TEntity>().FromSql($"{procedureName} {parameters}");
    }
}