using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Base;
using template.net8.api.Core.Interfaces;
using template.net8.api.Persistence.Models.Interfaces;

namespace template.net8.api.Domain.Specifications.Generic;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "ReSharper",
    "UnusedType.Global",
    Justification =
        "Generic specification type intended for reuse in repository queries; usage may be indirect or consumer-dependent.")]
internal sealed class EntityByIdSpecification<TEntity, TKey> : SpecificationBase<TEntity>
    where TEntity : class, IEntityWithId<TKey>
    where TKey : struct
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal EntityByIdSpecification(TKey entityId, bool trackData = false)
    {
        AddFilter(e => e.Id.Equals(entityId));
        AddOrderBy(static e => e.Id, OrderByType.Asc);
        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
        SetQuerySplitStrategy(QuerySplittingBehavior.SingleQuery);
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "ReSharper",
    "UnusedType.Global",
    Justification =
        "Generic specification type intended for reuse in repository queries; usage may be indirect or consumer-dependent.")]
internal sealed class EntitiesByIdsSpecification<TEntity, TKey> : SpecificationBase<TEntity>
    where TEntity : class, IEntityWithId<TKey>
    where TKey : struct
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal EntitiesByIdsSpecification(IEnumerable<TKey>? entityIds = null,
        bool trackData = false)
    {
        if (entityIds != null)
        {
            var enumerable = entityIds.ToList();
            AddFilter(e => enumerable.Contains(e.Id));
        }

        AddOrderBy(static e => e.Id, OrderByType.Asc);
        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
        SetQuerySplitStrategy(QuerySplittingBehavior.SingleQuery);
    }
}