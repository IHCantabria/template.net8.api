using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Interfaces;
using template.net8.api.Domain.Persistence.Models.Interfaces;

namespace template.net8.api.Core.Base;

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
    ///     Constructs a VerificationBase instance.
    /// </summary>
    protected VerificationBase()
    {
    }

    /// <summary>
    ///     Filters for the Query Specification Pattern Implementation for Querying Data with EF Core Queryable Extensions.
    /// </summary>
    public ICollection<Expression<Func<TEntity, bool>>> Filters { get; } = [];

    /// <summary>
    ///     OrderBy Conditions for the Query Specification Pattern Implementation for Querying Data with EF Core Queryable
    ///     Extensions.
    /// </summary>
    public ICollection<Tuple<Expression<Func<TEntity, object>>, OrderByType>> OrderBys { get; } = [];

    /// <summary>
    ///     Add an Include expression to load related data with the Query Specification Pattern Implementation for Querying
    ///     Data with EF Core Queryable Extensions.
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
    ///     Add a filter expression to the collection of filters for the Query Specification Pattern Implementation for
    ///     Querying Data with EF Core Queryable Extensions.
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
///     Specification Base
/// </summary>
/// <typeparam name="TEntity"></typeparam>
[CoreLibrary]
public class SpecificationBase<TEntity> : ISpecification<TEntity> where TEntity : class, IEntity
{
    /// <summary>
    ///     Constructs a SpecificationBase instance.
    /// </summary>
    protected SpecificationBase()
    {
    }

    /// <summary>
    ///     Query Splitting Behavior for the Query Specification Pattern Implementation for Querying Data with EF Core
    ///     Queryable Extensions.
    /// </summary>
    public QuerySplittingBehavior QuerySplitStrategy { get; private set; } = QuerySplittingBehavior.SingleQuery;

    /// <summary>
    ///     Query Tracking Behavior for the Query Specification Pattern Implementation for Querying Data with EF Core Queryable
    ///     Extensions.
    /// </summary>
    public QueryTrackingBehavior QueryTrackStrategy { get; private set; } = QueryTrackingBehavior.TrackAll;

    /// <summary>
    ///     Include collection to load related data with the Query Specification Pattern Implementation for Querying Data with
    ///     EF Core Queryable Extensions.
    /// </summary>
    public ICollection<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>
        Includes { get; } = [];

    /// <summary>
    ///     OrderBy Conditions for the Query Specification Pattern Implementation for Querying Data with EF Core Queryable
    ///     Extensions.
    /// </summary>
    public ICollection<Tuple<Expression<Func<TEntity, object>>, OrderByType>> OrderBys { get; } = [];

    /// <summary>
    ///     Take Rows for the Query Specification Pattern Implementation for Querying Data with EF Core Queryable Extensions.
    /// </summary>
    public int? TakeRows { get; private set; }

    /// <summary>
    ///     Filter Conditions for the Query Specification Pattern Implementation for Querying Data with EF Core Queryable
    ///     Extensions.
    /// </summary>
    public ICollection<Expression<Func<TEntity, bool>>> Filters { get; } = [];

    /// <summary>
    ///     GroupBy expression for the Query Specification Pattern Implementation for Querying Data with EF Core Queryable
    ///     Extensions.
    /// </summary>
    public Expression<Func<TEntity, object>>? GroupBy { get; private set; }

    /// <summary>
    ///     Add an Include expression to load related data with the Query Specification Pattern Implementation for Querying
    ///     Data with EF Core Queryable Extensions.
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
    ///     Add an OrderBy expression to the collection of OrderBys for the Query Specification Pattern Implementation for
    ///     Querying Data with EF Core Queryable Extensions.
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
    ///     Add a Take expression to the collection of TakeRows for the Query Specification Pattern Implementation for Querying
    ///     Data with EF Core Queryable Extensions.
    /// </summary>
    /// <param name="rows"></param>
    protected void AddTakeRows(int rows)
    {
        TakeRows = rows;
    }

    /// <summary>
    ///     Add a filter expression to the collection of filters for the Query Specification Pattern Implementation for
    ///     Querying Data with EF Core Queryable Extensions.
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
    ///     Add a GroupBy expression to the Query Specification Pattern Implementation for Querying Data with EF Core Queryable
    ///     Extensions.
    /// </summary>
    /// <param name="groupByExpression"></param>
    protected void ApplyGroupBy(Expression<Func<TEntity, object>> groupByExpression)
    {
        GroupBy = groupByExpression;
    }

    /// <summary>
    ///     Sets the Query Split Strategy for the Query Specification Pattern Implementation for Querying Data with EF Core
    ///     Queryable Extensions.
    /// </summary>
    /// <param name="querySplitStrategy"></param>
    protected void SetQuerySplitStrategy(QuerySplittingBehavior querySplitStrategy)
    {
        QuerySplitStrategy = querySplitStrategy;
    }

    /// <summary>
    ///     Sets the Query Tracking Behavior for the Query Specification Pattern Implementation for Querying Data with EF Core
    ///     Queryable Extensions.
    /// </summary>
    /// <param name="queryTrackStrategy"></param>
    protected void SetQueryTrackStrategy(QueryTrackingBehavior queryTrackStrategy)
    {
        QueryTrackStrategy = queryTrackStrategy;
    }
}