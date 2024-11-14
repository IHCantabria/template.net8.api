using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Interfaces;
using template.net8.api.Domain.Base;
using template.net8.api.Domain.Persistence.Models.Interfaces;

namespace template.net8.api.Domain.Specifications.Generic;

/// <typeparam name="TEntity"></typeparam>
[CoreLibrary]
internal sealed class EntityByIdShortSpecification<TEntity> : SpecificationBase<TEntity>
    where TEntity : class, IEntityWithId
{
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    internal EntityByIdShortSpecification(short entityId, bool trackData = false)
    {
        AddFilter(e => e.Id == entityId);
        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
    }
}

/// <typeparam name="TEntity"></typeparam>
[CoreLibrary]
internal sealed class EntitiesByIdsShortSpecification<TEntity> : SpecificationBase<TEntity>
    where TEntity : class, IEntityWithIdShort
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
    internal EntitiesByIdsShortSpecification(IEnumerable<short>? entityIds = null,
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
[CoreLibrary]
internal sealed class DtoByIdShortSpecification<TEntity, TDto> : SpecificationBase<TEntity, TDto>
    where TEntity : class, IEntityWithIdShort where TDto : class, IDto
{
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    internal DtoByIdShortSpecification(short dtoId, bool trackData = false)
    {
        AddFilter(e => e.Id == dtoId);
        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
    }
}

/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TDto"></typeparam>
[CoreLibrary]
internal sealed class DtosByIdsShortSpecification<TEntity, TDto> : SpecificationBase<TEntity, TDto>
    where TEntity : class, IEntityWithIdShort where TDto : class, IDto
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
    internal DtosByIdsShortSpecification(IEnumerable<short>? dtosIds = null, bool trackData = false)
    {
        if (dtosIds != null)
        {
            var enumerable = dtosIds.ToList();
            AddFilter(e => enumerable.Contains(e.Id));
        }

        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
    }
}

/// <typeparam name="TEntity"></typeparam>
[CoreLibrary]
internal sealed class EntityByIdSpecification<TEntity> : SpecificationBase<TEntity> where TEntity : class, IEntityWithId
{
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    internal EntityByIdSpecification(int entityId, bool trackData = false)
    {
        AddFilter(e => e.Id == entityId);
        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
    }
}

/// <typeparam name="TEntity"></typeparam>
[CoreLibrary]
internal sealed class EntitiesByIdsSpecification<TEntity> : SpecificationBase<TEntity>
    where TEntity : class, IEntityWithId
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
    internal EntitiesByIdsSpecification(IEnumerable<int>? entityIds = null,
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
[CoreLibrary]
internal sealed class DtoByIdSpecification<TEntity, TDto> : SpecificationBase<TEntity, TDto>
    where TEntity : class, IEntityWithId where TDto : class, IDto
{
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    internal DtoByIdSpecification(int dtoId, bool trackData = false)
    {
        AddFilter(e => e.Id == dtoId);
        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
    }
}

/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TDto"></typeparam>
[CoreLibrary]
internal sealed class DtosByIdsSpecification<TEntity, TDto> : SpecificationBase<TEntity, TDto>
    where TEntity : class, IEntityWithId where TDto : class, IDto
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
    internal DtosByIdsSpecification(IEnumerable<int>? dtosIds = null, bool trackData = false)
    {
        if (dtosIds != null)
        {
            var enumerable = dtosIds.ToList();
            AddFilter(e => enumerable.Contains(e.Id));
        }

        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
    }
}

/// <typeparam name="TEntity"></typeparam>
[CoreLibrary]
internal sealed class EntityByIdLongSpecification<TEntity> : SpecificationBase<TEntity>
    where TEntity : class, IEntityWithId
{
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    internal EntityByIdLongSpecification(long entityId, bool trackData = false)
    {
        AddFilter(e => e.Id == entityId);
        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
    }
}

/// <typeparam name="TEntity"></typeparam>
[CoreLibrary]
internal sealed class EntitiesByIdsLongSpecification<TEntity> : SpecificationBase<TEntity>
    where TEntity : class, IEntityWithIdLong
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
    internal EntitiesByIdsLongSpecification(IEnumerable<long>? entityIds = null,
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
[CoreLibrary]
internal sealed class DtoByIdLongSpecification<TEntity, TDto> : SpecificationBase<TEntity, TDto>
    where TEntity : class, IEntityWithIdLong where TDto : class, IDto
{
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    internal DtoByIdLongSpecification(long dtoId, bool trackData = false)
    {
        AddFilter(e => e.Id == dtoId);
        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
    }
}

/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TDto"></typeparam>
[CoreLibrary]
internal sealed class DtosByIdsLongSpecification<TEntity, TDto> : SpecificationBase<TEntity, TDto>
    where TEntity : class, IEntityWithIdLong where TDto : class, IDto
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
    internal DtosByIdsLongSpecification(IEnumerable<long>? dtosIds = null, bool trackData = false)
    {
        if (dtosIds != null)
        {
            var enumerable = dtosIds.ToList();
            AddFilter(e => enumerable.Contains(e.Id));
        }

        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
    }
}