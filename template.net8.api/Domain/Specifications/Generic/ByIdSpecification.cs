using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Interfaces;
using template.net8.api.Domain.Base;
using template.net8.api.Domain.Persistence.Models.Interfaces;

namespace template.net8.api.Domain.Specifications.Generic;

/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
[CoreLibrary]
internal sealed class EntityByIdSpecification<TEntity, TKey> : SpecificationBase<TEntity>
    where TEntity : class, IEntityWithId<TKey>
    where TKey : struct
{
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

/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
[CoreLibrary]
internal sealed class EntitiesByIdsSpecification<TEntity, TKey> : SpecificationBase<TEntity>
    where TEntity : class, IEntityWithId<TKey>
    where TKey : struct
{
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

/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TDto"></typeparam>
/// <typeparam name="TKeyEntity"></typeparam>
[CoreLibrary]
internal sealed class DtoByIdSpecification<TEntity, TKeyEntity, TDto> : SpecificationBase<TEntity, TDto>
    where TEntity : class, IEntityWithId<TKeyEntity> where TKeyEntity : struct where TDto : class, IDto
{
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    internal DtoByIdSpecification(TKeyEntity dtoId, bool trackData = false)
    {
        AddFilter(e => e.Id.Equals(dtoId));
        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
    }
}

/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TDto"></typeparam>
/// <typeparam name="TKeyEntity"></typeparam>
[CoreLibrary]
internal sealed class DtosByIdsSpecification<TEntity, TKeyEntity, TDto> : SpecificationBase<TEntity, TDto>
    where TEntity : class, IEntityWithId<TKeyEntity> where TKeyEntity : struct where TDto : class, IDto
{
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
    internal DtosByIdsSpecification(IEnumerable<TKeyEntity>? dtosIds = null, bool trackData = false)
    {
        if (dtosIds != null)
        {
            var enumerable = dtosIds.ToList();
            AddFilter(e => enumerable.Contains(e.Id));
        }

        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
    }
}