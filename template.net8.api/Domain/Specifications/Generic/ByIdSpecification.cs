using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Base;
using template.net8.api.Domain.Persistence.Models.Interfaces;

namespace template.net8.api.Domain.Specifications.Generic;

[CoreLibrary]
internal sealed class EntityByIdSpecification<TEntity, TKey> : SpecificationBase<TEntity>
    where TEntity : class, IEntityWithId<TKey>
    where TKey : struct
{
    /// <summary>
    ///     Constructs a specification to filter an entity by its ID.
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="trackData"></param>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    internal EntityByIdSpecification(TKey entityId, bool trackData = false)
    {
        AddFilter(e => e.Id.Equals(entityId));
        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
    }
}

[CoreLibrary]
internal sealed class EntitiesByIdsSpecification<TEntity, TKey> : SpecificationBase<TEntity>
    where TEntity : class, IEntityWithId<TKey>
    where TKey : struct
{
    /// <summary>
    ///     Constructs a specification to filter entities by a collection of IDs.
    /// </summary>
    /// <param name="entityIds"></param>
    /// <param name="trackData"></param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    internal EntitiesByIdsSpecification(IEnumerable<TKey>? entityIds = null,
        bool trackData = false)
    {
        if (entityIds != null)
        {
            var enumerable = entityIds.ToList();
            AddFilter(e => enumerable.Contains(e.Id));
        }

        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
    }
}