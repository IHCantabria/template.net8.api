using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using template.net8.api.Core.Attributes;
using template.net8.api.Domain.Interfaces;
using template.net8.api.Domain.Persistence.Models.Interfaces;
using template.net8.api.Domain.Specifications.Interfaces;

namespace template.net8.api.Domain.Base;

// Generic Verification
// For additional expressions, class needs to be derived.
/// <summary>
///     Verification Base
/// </summary>
/// <typeparam name="TEntity"></typeparam>
[CoreLibrary]
public class VerificationBase<TEntity> : IVerification<TEntity> where TEntity : class, IEntity
{
    /// <summary>
    /// </summary>
    protected VerificationBase()
    {
    }

    /// <summary>
    /// </summary>
    public ICollection<Expression<Func<TEntity, bool>>> Filters { get; } = [];

    /// <summary>
    /// </summary>
    public ICollection<Tuple<Expression<Func<TEntity, object>>, OrderByType>> OrderBys { get; } = [];

    /// <summary>
    /// </summary>
    /// <param name="orderByExpression"></param>
    /// <param name="orderType"></param>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression, OrderByType orderType)
    {
        OrderBys.Add(new Tuple<Expression<Func<TEntity, object>>, OrderByType>(orderByExpression, orderType));
    }

    /// <summary>
    /// </summary>
    /// <param name="filterExpression"></param>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    protected void AddFilter(Expression<Func<TEntity, bool>> filterExpression)
    {
        Filters.Add(filterExpression);
    }
}

// Generic Specification
// For additional expressions, class needs to be derived.
/// <summary>
/// </summary>
/// <typeparam name="TEntity"></typeparam>
[CoreLibrary]
public class SpecificationBase<TEntity> : ISpecification<TEntity> where TEntity : class, IEntity
{
    /// <summary>
    /// </summary>
    protected SpecificationBase()
    {
    }

    /// <summary>
    /// </summary>
    public QuerySplittingBehavior QuerySplitStrategy { get; private set; } = QuerySplittingBehavior.SingleQuery;

    /// <summary>
    /// </summary>
    public QueryTrackingBehavior QueryTrackStrategy { get; private set; } = QueryTrackingBehavior.TrackAll;

    /// <summary>
    /// </summary>
    public ICollection<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>
        Includes { get; } = [];

    /// <summary>
    /// </summary>
    public ICollection<Tuple<Expression<Func<TEntity, object>>, OrderByType>> OrderBys { get; } = [];

    /// <summary>
    /// </summary>
    public ICollection<Expression<Func<TEntity, bool>>> Filters { get; } = [];

    /// <summary>
    /// </summary>
    public Expression<Func<TEntity, object>>? GroupBy { get; private set; }

    /// <summary>
    /// </summary>
    /// <param name="includeExpression"></param>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    protected void AddInclude(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    /// <summary>
    /// </summary>
    /// <param name="orderByExpression"></param>
    /// <param name="orderType"></param>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression, OrderByType orderType)
    {
        OrderBys.Add(new Tuple<Expression<Func<TEntity, object>>, OrderByType>(orderByExpression, orderType));
    }

    /// <summary>
    /// </summary>
    /// <param name="filterExpression"></param>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    protected void AddFilter(Expression<Func<TEntity, bool>> filterExpression)
    {
        Filters.Add(filterExpression);
    }

    /// <summary>
    /// </summary>
    /// <param name="groupByExpression"></param>
    protected void ApplyGroupBy(Expression<Func<TEntity, object>> groupByExpression)
    {
        GroupBy = groupByExpression;
    }

    /// <summary>
    /// </summary>
    /// <param name="querySplitStrategy"></param>
    protected void SetQuerySplitStrategy(QuerySplittingBehavior querySplitStrategy)
    {
        QuerySplitStrategy = querySplitStrategy;
    }

    /// <summary>
    /// </summary>
    /// <param name="queryTrackStrategy"></param>
    protected void SetQueryTrackStrategy(QueryTrackingBehavior queryTrackStrategy)
    {
        QueryTrackStrategy = queryTrackStrategy;
    }
}

// Overload Generic Specification with Projection Dto
// For additional expressions, class needs to be derived.
/// <summary>
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TDto"></typeparam>
[CoreLibrary]
public class SpecificationBase<TEntity, TDto> : ISpecification<TEntity, TDto>
    where TEntity : class, IEntity where TDto : class, IDto
{
    /// <summary>
    /// </summary>
    protected SpecificationBase()
    {
    }

    /// <summary>
    /// </summary>
    public ICollection<Expression<Func<TEntity, object>>> Includes { get; } = [];

    /// <summary>
    /// </summary>
    public QuerySplittingBehavior QuerySplitStrategy { get; private set; } = QuerySplittingBehavior.SingleQuery;

    /// <summary>
    /// </summary>
    public QueryTrackingBehavior QueryTrackStrategy { get; private set; } = QueryTrackingBehavior.TrackAll;

    /// <summary>
    /// </summary>
    public ICollection<Expression<Func<TDto, object?>>> MembersToExpand { get; } = [];

    /// <summary>
    /// </summary>
    public ICollection<Tuple<Expression<Func<TEntity, object>>, OrderByType>> OrderBys { get; } = [];

    /// <summary>
    /// </summary>
    public ICollection<Expression<Func<TEntity, bool>>> Filters { get; } = [];

    /// <summary>
    /// </summary>
    public Expression<Func<TEntity, object>>? GroupBy { get; private set; }

    /// <summary>
    /// </summary>
    public object MapperObjectParams { get; private set; } = new();

    /// <summary>
    /// </summary>
    /// <param name="includeExpression"></param>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    /// <summary>
    /// </summary>
    /// <param name="orderByExpression"></param>
    /// <param name="orderType"></param>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression, OrderByType orderType)
    {
        OrderBys.Add(new Tuple<Expression<Func<TEntity, object>>, OrderByType>(orderByExpression, orderType));
    }

    /// <summary>
    /// </summary>
    /// <param name="filterExpression"></param>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    protected void AddFilter(Expression<Func<TEntity, bool>> filterExpression)
    {
        Filters.Add(filterExpression);
    }

    /// <summary>
    /// </summary>
    /// <param name="groupByExpression"></param>
    protected void ApplyGroupBy(Expression<Func<TEntity, object>> groupByExpression)
    {
        GroupBy = groupByExpression;
    }

    /// <summary>
    /// </summary>
    /// <param name="querySplitStrategy"></param>
    protected void SetQuerySplitStrategy(QuerySplittingBehavior querySplitStrategy)
    {
        QuerySplitStrategy = querySplitStrategy;
    }

    /// <summary>
    /// </summary>
    /// <param name="queryTrackStrategy"></param>
    protected void SetQueryTrackStrategy(QueryTrackingBehavior queryTrackStrategy)
    {
        QueryTrackStrategy = queryTrackStrategy;
    }

    /// <summary>
    /// </summary>
    /// <param name="objParams"></param>
    protected void AddParams(object objParams)
    {
        MapperObjectParams = objParams;
    }

    /// <summary>
    /// </summary>
    /// <param name="memberExpression"></param>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    protected void AddMember(Expression<Func<TDto, object?>> memberExpression)
    {
        MembersToExpand.Add(memberExpression);
    }
}