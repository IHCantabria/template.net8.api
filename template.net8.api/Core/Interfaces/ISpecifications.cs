using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using template.net8.api.Persistence.Models.Interfaces;

namespace template.net8.api.Core.Interfaces;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "The enum is part of the public API contract and must remain publicly accessible.")]
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
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "The interface is part of the public API contract and must remain publicly accessible.")]
internal interface IVerification<TEntity> where TEntity : class, IEntity
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    ICollection<Expression<Func<TEntity, bool>>> Filters { get; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    ICollection<Tuple<Expression<Func<TEntity, object>>, OrderByType>> OrderBys { get; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "The interface is part of the public API contract and must remain publicly accessible.")]
internal interface ISpecification<TEntity> : IVerification<TEntity> where TEntity : class, IEntity
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    ICollection<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> Includes { get; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Expression<Func<TEntity, object>>? GroupBy { get; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    int? TakeRows { get; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    QuerySplittingBehavior QuerySplitStrategy { get; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    QueryTrackingBehavior QueryTrackStrategy { get; }
}