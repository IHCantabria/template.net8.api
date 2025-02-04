using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Interfaces;
using template.net8.api.Domain.Persistence.Models.Interfaces;

namespace template.net8.api.Core.Extensions;

[CoreLibrary]
internal static class QueryableExtensions
{
    [CoreLibrary]
    internal static IQueryable<TEntity> ApplyVerification<TEntity>(this IQueryable<TEntity> query,
        IVerification<TEntity>? verification) where TEntity : class, IEntity
    {
        // Do not apply anything if specification is null
        if (verification is null) return query;

        // Modify the IQueryable

        query = query.ApplyFilters(verification.Filters);
        return query;
    }

    [CoreLibrary]
    internal static IQueryable<TEntity> ApplySpecification<TEntity>(this IQueryable<TEntity> query,
        ISpecification<TEntity>? specification) where TEntity : class, IEntity
    {
        // Do not apply anything if specification is null
        if (specification is null) return query;

        // Modify the IQueryable

        query = query.ApplyFilters(specification.Filters);
        query = query.ApplyIncludes(specification.Includes);
        query = query.ApplyOrderBys(specification.OrderBys);
        query = query.ApplyGroupBy(specification.GroupBy);
        query = query.ApplyTakeRows(specification.TakeRows);
        query = query.ApplyQuerySplitStrategy(specification.QuerySplitStrategy);
        query = query.ApplyQueryTrackStrategyy(specification.QueryTrackStrategy);

        return query;
    }

    [CoreLibrary]
    internal static IQueryable<TEntity> ApplySpecification<TEntity, TDto>(this IQueryable<TEntity> query,
        ISpecification<TEntity, TDto>? specification) where TEntity : class, IEntity where TDto : class, IDto
    {
        // Do not apply anything if specification is null
        if (specification is null) return query;

        // Modify the IQueryable

        query = query.ApplyFilters(specification.Filters);
        query = query.ApplyOrderBys(specification.OrderBys);
        query = query.ApplyGroupBy(specification.GroupBy);
        query = query.ApplyTakeRows(specification.TakeRows);
        query = query.ApplyQuerySplitStrategy(specification.QuerySplitStrategy);
        query = query.ApplyQueryTrackStrategyy(specification.QueryTrackStrategy);

        return query;
    }

    private static IQueryable<TEntity> ApplyFilters<TEntity>(this IQueryable<TEntity> queryable,
        ICollection<Expression<Func<TEntity, bool>>> filters) where TEntity : class, IEntity
    {
        return filters.Aggregate(queryable, (current, filter) => current.Where(filter));
    }

    private static IQueryable<TEntity> ApplyIncludes<TEntity>(this IQueryable<TEntity> queryable,
        IEnumerable<Func<IQueryable<TEntity>, IQueryable<TEntity>>> includes)
    {
        return includes.Aggregate(queryable, (current, include) => include(current));
    }

    private static IQueryable<TEntity> ApplyOrderBys<TEntity>(this IQueryable<TEntity> queryable,
        ICollection<Tuple<Expression<Func<TEntity, object>>, OrderByType>> orderBys) where TEntity : class, IEntity
    {
        return orderBys.Aggregate(queryable,
            (current, orderBy) => current.SmartOrderBy(orderBy.Item1, orderBy.Item2, current.Equals(queryable)));
    }

    private static IQueryable<TEntity> SmartOrderBy<TEntity>(this IQueryable<TEntity> queryable,
        Expression<Func<TEntity, object>> expression, OrderByType orderType, bool isFirstIteration)
        where TEntity : class, IEntity
    {
        return orderType switch
        {
            OrderByType.Asc when !isFirstIteration && queryable is IOrderedQueryable<TEntity> orderedQueryable =>
                orderedQueryable.ThenBy(expression),
            OrderByType.Desc when !isFirstIteration && queryable is IOrderedQueryable<TEntity> orderedQueryable =>
                orderedQueryable.ThenByDescending(expression),
            OrderByType.Desc => queryable.OrderByDescending(expression),
            OrderByType.Asc => queryable.OrderBy(expression),
            _ => queryable.OrderBy(expression)
        };
    }

    private static IQueryable<TEntity> ApplyGroupBy<TEntity>(this IQueryable<TEntity> queryable,
        Expression<Func<TEntity, object>>? groupBy) where TEntity : class, IEntity
    {
        return groupBy != null ? queryable.GroupBy(groupBy).SelectMany(x => x) : queryable;
    }

    private static IQueryable<TEntity> ApplyTakeRows<TEntity>(this IQueryable<TEntity> queryable,
        int? takeRows) where TEntity : class, IEntity
    {
        return takeRows is null ? queryable : queryable.Take(takeRows.Value);
    }

    private static IQueryable<TEntity> ApplyQuerySplitStrategy<TEntity>(this IQueryable<TEntity> queryable,
        QuerySplittingBehavior querySplitStrategy) where TEntity : class, IEntity
    {
        return querySplitStrategy switch
        {
            QuerySplittingBehavior.SplitQuery => queryable.AsSplitQuery(),
            QuerySplittingBehavior.SingleQuery => queryable.AsSingleQuery(),
            _ => queryable.AsSingleQuery()
        };
    }

    private static IQueryable<TEntity> ApplyQueryTrackStrategyy<TEntity>(this IQueryable<TEntity> queryable,
        QueryTrackingBehavior queryTrackStrategy) where TEntity : class, IEntity
    {
        return queryTrackStrategy switch
        {
            QueryTrackingBehavior.NoTracking => queryable.AsNoTracking(),
            QueryTrackingBehavior.NoTrackingWithIdentityResolution => queryable.AsNoTrackingWithIdentityResolution(),
            QueryTrackingBehavior.TrackAll => queryable.AsTracking(),
            _ => queryable.AsTracking()
        };
    }
}