using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using template.net8.api.Core.Attributes;
using template.net8.api.Domain.Interfaces;
using template.net8.api.Domain.Persistence.Models.Interfaces;

namespace template.net8.api.Domain.Specifications.Interfaces;

/// <summary>
///     Enumeration for Query Splitting Behavior for the Query Specification Pattern Implementation for Querying Data with
///     EF
/// </summary>
[CoreLibrary]
public enum OrderByType
{
    /// <summary>
    ///     Ascending Order
    /// </summary>
    Asc = 0,

    /// <summary>
    ///     Descending Order
    /// </summary>
    Desc = 1
}

/// <summary>
///     Interface for Verification Pattern Implementation for Querying Data with EF Core Queryable Extensions.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
[CoreLibrary]
public interface IVerification<TEntity> where TEntity : class, IEntity
{
    /// <summary>
    ///     Filter Conditions for the Query Specification Pattern Implementation for Querying Data with EF Core Queryable
    ///     Extensions.
    /// </summary>
    ICollection<Expression<Func<TEntity, bool>>> Filters { get; }

    /// <summary>
    ///     OrderBy Conditions for the Query Specification Pattern Implementation for Querying Data with EF Core Queryable
    ///     Extensions.
    /// </summary>
    ICollection<Tuple<Expression<Func<TEntity, object>>, OrderByType>> OrderBys { get; }
}

/// <summary>
///     Interface for Specification Pattern Implementation for Querying Data with EF Core Queryable Extensions and EF Core
///     Queryable Extensions for Dto Projection.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
[CoreLibrary]
public interface ISpecification<TEntity> : IVerification<TEntity> where TEntity : class, IEntity
{
    /// <summary>
    ///     Include collection to load related data with the Query Specification Pattern Implementation for Querying Data with
    ///     EF Core Queryable Extensions.
    /// </summary>
    ICollection<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> Includes { get; }

    /// <summary>
    ///     GroupBy expression for the Query Specification Pattern Implementation for Querying Data with EF Core Queryable
    ///     Extensions.
    /// </summary>
    Expression<Func<TEntity, object>>? GroupBy { get; }

    /// <summary>
    ///     Query Split Strategy for the Query Specification Pattern Implementation for Querying Data with EF Core Queryable
    ///     Extensions. Default is SingleQuery.
    /// </summary>
    QuerySplittingBehavior QuerySplitStrategy { get; }

    /// <summary>
    ///     Query Tracking Behavior for the Query Specification Pattern Implementation for Querying Data with EF Core Queryable
    ///     Extensions. Default is TrackAll.
    /// </summary>
    QueryTrackingBehavior QueryTrackStrategy { get; }
}

/// <summary>
///     Interface for Specification Pattern Implementation for Querying Data with EF Core Queryable Extensions and EF Core
///     Queryable Extensions for Dto Projection. This interface is used for Projection Dto.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TDto"></typeparam>
[CoreLibrary]
public interface ISpecification<TEntity, TDto> : IVerification<TEntity>
    where TEntity : class, IEntity
    where TDto : class, IDto
{
    /// <summary>
    ///     GroupBy expression for the Query Specification Pattern Implementation for Querying Data with EF Core Queryable
    ///     Extensions.
    /// </summary>
    Expression<Func<TEntity, object>>? GroupBy { get; }

    /// <summary>
    ///     Query Split Strategy for the Query Specification Pattern Implementation for Querying Data with EF Core Queryable
    ///     Extensions. Default is SingleQuery.
    /// </summary>
    QuerySplittingBehavior QuerySplitStrategy { get; }

    /// <summary>
    ///     Query Tracking Behavior for the Query Specification Pattern Implementation for Querying Data with EF Core Queryable
    ///     Extensions. Default is TrackAll.
    /// </summary>
    QueryTrackingBehavior QueryTrackStrategy { get; }

    /// <summary>
    ///     Members to Expand in the Dto Projection for the Query Specification Pattern Implementation for Querying Data with
    ///     EF Core Queryable Extensions. Used for Projection Dto with AutoMapper.
    /// </summary>
    ICollection<Expression<Func<TDto, object?>>> MembersToExpand { get; }

    /// <summary>
    ///     Mapper Object Params for the Query Specification Pattern Implementation for Querying Data with EF Core Queryable
    ///     Extensions. Used for Projection Dto with AutoMapper.
    /// </summary>
    object MapperObjectParams { get; }
}