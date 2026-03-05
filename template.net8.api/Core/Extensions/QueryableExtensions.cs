using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using HotChocolate.Resolvers;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Interfaces;
using template.net8.api.Persistence.Models.Interfaces;

namespace template.net8.api.Core.Extensions;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class QueryableExtensions
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static IQueryable<TEntity> ApplyVerification<TEntity>(this IQueryable<TEntity> query,
        IVerification<TEntity>? verification) where TEntity : class, IEntity
    {
        // Do not apply anything if specification is null
        if (verification is null) return query;

        // Modify the IQueryable

        query = verification.Filters.Count > 0 ? query.ApplyFilters(verification.Filters) : query;
        return query;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static IQueryable<TEntity> ApplySpecification<TEntity>(this IQueryable<TEntity> query,
        ISpecification<TEntity>? specification) where TEntity : class, IEntity
    {
        // Do not apply anything if specification is null
        if (specification is null) return query;

        // Modify the IQueryable

        query = specification.Filters.Count > 0 ? query.ApplyFilters(specification.Filters) : query;
        query = specification.Includes.Count > 0 ? query.ApplyIncludes(specification.Includes) : query;
        query = specification.GroupBy is not null ? query.ApplyGroupBy(specification.GroupBy) : query;
        query = specification.OrderBys.Count > 0 ? query.ApplyOrderBys(specification.OrderBys) : query;
        query = specification.TakeRows is not null ? query.ApplyTakeRows((int)specification.TakeRows) : query;
        query = query.ApplyQuerySplitStrategy(specification.QuerySplitStrategy);
        query = query.ApplyQueryTrackStrategy(specification.QueryTrackStrategy);

        return query;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static IQueryable<TDto> ApplyProjection<TEntity, TDto>(this IQueryable<TEntity> query,
        IProjection<TEntity, TDto> projection) where TEntity : class, IEntity where TDto : class, IDto
    {
        return query.Select(projection.Expression);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static IQueryable<TDto> ApplyProjection<TEntity, TDto>(this IQueryable<TEntity> query,
        IProjection<TEntity, TDto> projection,
        IResolverContext context) where TEntity : class, IEntity where TDto : class, IDto
    {
        var selectedFieldsTree = context.GetSelectedFieldsTree();
        var lambdaBody = BuildMemberInit(projection.Expression.Body, selectedFieldsTree);
        var lambda = Expression.Lambda<Func<TEntity, TDto>>(lambdaBody, projection.Expression.Parameters);
        return query.Select(lambda);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static Expression BuildMemberInit(Expression source, Dictionary<string, HashSet<string>> selectedFieldsTree,
        string path = "")
    {
        if (source is not MemberInitExpression init) return source;

        var bindings = init.Bindings
            .OfType<MemberAssignment>()
            .Where(b => IsSelected(path, b.Member.Name, selectedFieldsTree))
            .Select(b =>
            {
                var childPath = string.IsNullOrEmpty(path) ? b.Member.Name : $"{path}.{b.Member.Name}";

                if (IsSimpleType(b.Expression.Type))
                    return Expression.Bind(b.Member, b.Expression);

                if (IsCollectionType(b.Expression.Type, out var elementType))
                    return Expression.Bind(b.Member,
                        BuildCollectionInit(b.Expression, elementType, selectedFieldsTree, childPath));

                var childInit = BuildMemberInit(b.Expression, selectedFieldsTree, childPath);
                return Expression.Bind(b.Member, childInit);
            });

        return Expression.MemberInit(Expression.New(init.Type), bindings);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static BinaryExpression BuildCollectionInit(
        Expression source,
        Type elementType,
        Dictionary<string, HashSet<string>> selectedFieldsTree,
        string path)
    {
        var itemParam = Expression.Parameter(elementType, "x");
        var itemInit = BuildMemberInit(itemParam, selectedFieldsTree, path);

        var selectMethod = typeof(Queryable)
            .GetMethods()
            .First(static m => m.Name == "Select" && m.GetParameters().Length == 2)
            .MakeGenericMethod(elementType, elementType);

        var selectCall = Expression.Call(selectMethod, source, Expression.Lambda(itemInit, itemParam));

        var defaultValue = Expression.Default(source.Type);
        return Expression.Coalesce(selectCall, defaultValue);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static bool IsSelected(string path, string propName, Dictionary<string, HashSet<string>> tree)
    {
        return tree.TryGetValue(path, out var props) && props.Contains(propName);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static bool IsSimpleType(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;
        return type.IsPrimitive
               || type.IsEnum
               || type == typeof(string)
               || type == typeof(decimal)
               || type == typeof(DateTime)
               || type == typeof(DateTimeOffset)
               || type == typeof(Guid)
               || type == typeof(TimeSpan);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static bool IsCollectionType(Type type, [NotNullWhen(true)] out Type? elementType)
    {
        var enumerableType = type.GetInterfaces()
            .FirstOrDefault(static i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

        if (enumerableType != null)
        {
            elementType = enumerableType.GetGenericArguments()[0];
            return true;
        }

        elementType = null;
        return false;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static IQueryable<TEntity> ApplyFilters<TEntity>(this IQueryable<TEntity> queryable,
        IEnumerable<Expression<Func<TEntity, bool>>> filters) where TEntity : class, IEntity
    {
        var predicate = PredicateBuilder.New<TEntity>();

        predicate = filters.Aggregate(predicate, static (current, filter) => current.And(filter));

        var expandedPredicate = (Expression<Func<TEntity, bool>>)predicate.Expand();
        return queryable.Where(expandedPredicate);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static IQueryable<TEntity> ApplyIncludes<TEntity>(this IQueryable<TEntity> queryable,
        IEnumerable<Func<IQueryable<TEntity>, IQueryable<TEntity>>> includes)
    {
        return includes.Aggregate(queryable, static (current, include) => include(current));
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static IQueryable<TEntity> ApplyOrderBys<TEntity>(this IQueryable<TEntity> queryable,
        IEnumerable<Tuple<Expression<Func<TEntity, object>>, OrderByType>> orderBys) where TEntity : class, IEntity
    {
        return orderBys.Aggregate(queryable,
            (current, orderBy) => current.SmartOrderBy(orderBy.Item1, orderBy.Item2, current.Equals(queryable)));
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
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
            _ => queryable.OrderBy(expression)
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static IQueryable<TEntity> ApplyGroupBy<TEntity>(this IQueryable<TEntity> queryable,
        Expression<Func<TEntity, object>> groupBy) where TEntity : class, IEntity
    {
        return queryable.GroupBy(groupBy).SelectMany(static x => x);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static IQueryable<TEntity> ApplyTakeRows<TEntity>(this IQueryable<TEntity> queryable,
        int takeRows) where TEntity : class, IEntity
    {
        return queryable.Take(takeRows);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static IQueryable<TEntity> ApplyQuerySplitStrategy<TEntity>(this IQueryable<TEntity> queryable,
        QuerySplittingBehavior querySplitStrategy) where TEntity : class, IEntity
    {
        return querySplitStrategy switch
        {
            QuerySplittingBehavior.SplitQuery => queryable.AsSplitQuery(),
            _ => queryable.AsSingleQuery()
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static IQueryable<TEntity> ApplyQueryTrackStrategy<TEntity>(this IQueryable<TEntity> queryable,
        QueryTrackingBehavior queryTrackStrategy) where TEntity : class, IEntity
    {
        return queryTrackStrategy switch
        {
            QueryTrackingBehavior.NoTracking => queryable.AsNoTracking(),
            QueryTrackingBehavior.NoTrackingWithIdentityResolution => queryable.AsNoTrackingWithIdentityResolution(),
            _ => queryable.AsTracking()
        };
    }
}