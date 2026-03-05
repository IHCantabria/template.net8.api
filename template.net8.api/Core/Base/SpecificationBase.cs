using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using template.net8.api.Core.Interfaces;
using template.net8.api.Persistence.Models.Interfaces;

namespace template.net8.api.Core.Base;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification =
        "Base infrastructure class. Members are intended to be used selectively by derived verification implementations.")]
internal class VerificationBase<TEntity> : IVerification<TEntity> where TEntity : class, IEntity
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    protected VerificationBase()
    {
    }

    /// <inheritdoc cref="IVerification{TEntity}.Filters" />
    public ICollection<Expression<Func<TEntity, bool>>> Filters { get; } = [];

    /// <inheritdoc cref="IVerification{TEntity}.OrderBys" />
    public ICollection<Tuple<Expression<Func<TEntity, object>>, OrderByType>> OrderBys { get; } = [];

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression, OrderByType orderType)
    {
        OrderBys.Add(new Tuple<Expression<Func<TEntity, object>>, OrderByType>(orderByExpression, orderType));
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    protected void AddFilter(Expression<Func<TEntity, bool>> filterExpression)
    {
        Filters.Add(filterExpression);
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification =
        "Base infrastructure class. Members are intended to be used selectively by derived verification implementations.")]
internal class SpecificationBase<TEntity> : ISpecification<TEntity> where TEntity : class, IEntity
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    protected SpecificationBase()
    {
    }

    /// <inheritdoc cref="ISpecification{TEntity}.QuerySplitStrategy" />
    public QuerySplittingBehavior QuerySplitStrategy { get; private set; } = QuerySplittingBehavior.SingleQuery;

    /// <inheritdoc cref="ISpecification{TEntity}.QueryTrackStrategy" />
    public QueryTrackingBehavior QueryTrackStrategy { get; private set; } = QueryTrackingBehavior.TrackAll;

    /// <inheritdoc cref="ISpecification{TEntity}.Includes" />
    public ICollection<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>
        Includes { get; } = [];

    /// <inheritdoc cref="ISpecification{TEntity}.OrderBys" />
    public ICollection<Tuple<Expression<Func<TEntity, object>>, OrderByType>> OrderBys { get; } = [];

    /// <inheritdoc cref="ISpecification{TEntity}.TakeRows" />
    public int? TakeRows { get; private set; }

    /// <inheritdoc cref="ISpecification{TEntity}.Filters" />
    public ICollection<Expression<Func<TEntity, bool>>> Filters { get; } = [];

    /// <inheritdoc cref="ISpecification{TEntity}.GroupBy" />
    public Expression<Func<TEntity, object>>? GroupBy { get; private set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    protected void AddInclude(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression, OrderByType orderType)
    {
        OrderBys.Add(new Tuple<Expression<Func<TEntity, object>>, OrderByType>(orderByExpression, orderType));
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    protected void AddTakeRows(int rows)
    {
        TakeRows = rows;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    protected void AddFilter(Expression<Func<TEntity, bool>> filterExpression)
    {
        Filters.Add(filterExpression);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    protected void ApplyGroupBy(Expression<Func<TEntity, object>> groupByExpression)
    {
        GroupBy = groupByExpression;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    protected void SetQuerySplitStrategy(QuerySplittingBehavior querySplitStrategy)
    {
        QuerySplitStrategy = querySplitStrategy;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    protected void SetQueryTrackStrategy(QueryTrackingBehavior queryTrackStrategy)
    {
        QueryTrackStrategy = queryTrackStrategy;
    }
}