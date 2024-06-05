using Microsoft.EntityFrameworkCore;
using template.net8.Api.Core.Attributes;
using template.net8.Api.Domain.Base;
using template.net8.Api.Domain.Interfaces;
using template.net8.Api.Domain.Persistence.Models.Interfaces;

namespace template.net8.Api.Domain.Specifications.Generic;

[CoreLibrary]
internal sealed class EntityByIdVerification<TEntity> : VerificationBase<TEntity> where TEntity : class, IEntity
{
    /// <exception cref="NotSupportedException">The <see /> is read-only.</exception>
    /// <exception cref="InvalidOperationException">
    ///     The <see /> property is
    ///     <see langword="false" />.
    /// </exception>
    internal EntityByIdVerification(short? entityId = null)
    {
        if (entityId.HasValue) AddFilter(e => e.Id == entityId.Value);
    }
}

/// <typeparam name="TEntity"></typeparam>
[CoreLibrary]
internal sealed class EntitiesByIdsVerification<TEntity> : VerificationBase<TEntity> where TEntity : class, IEntity
{
    /// <exception cref="ArgumentNullException">source is <see langword="null" />.</exception>
    /// <exception cref="NotSupportedException">The <see /> is read-only.</exception>
    internal EntitiesByIdsVerification(IEnumerable<short>? entityIds = null)
    {
        if (entityIds is null) return;
        var enumerable = entityIds.ToList();
        AddFilter(e => enumerable.Contains(e.Id));
    }
}

/// <typeparam name="TEntity"></typeparam>
[CoreLibrary]
internal sealed class EntityByIdSpecification<TEntity> : SpecificationBase<TEntity> where TEntity : class, IEntity
{
    /// <exception cref="NotSupportedException">The <see /> is read-only.</exception>
    internal EntityByIdSpecification(short entityId, bool trackData = false)
    {
        AddFilter(e => e.Id == entityId);
        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
    }
}

/// <typeparam name="TEntity"></typeparam>
[CoreLibrary]
internal sealed class EntitiesByIdsSpecification<TEntity> : SpecificationBase<TEntity> where TEntity : class, IEntity
{
    /// <exception cref="ArgumentNullException">source is <see langword="null" />.</exception>
    /// <exception cref="NotSupportedException">The <see /> is read-only.</exception>
    internal EntitiesByIdsSpecification(IEnumerable<short>? entityIds = null,
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
    where TEntity : class, IEntity where TDto : class, IDto
{
    /// <exception cref="NotSupportedException">The <see /> is read-only.</exception>
    internal DtoByIdSpecification(short dtoId, bool trackData = false)
    {
        AddFilter(e => e.Id == dtoId);
        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
    }
}

/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TDto"></typeparam>
[CoreLibrary]
internal sealed class DtosByIdsSpecification<TEntity, TDto> : SpecificationBase<TEntity, TDto>
    where TEntity : class, IEntity where TDto : class, IDto
{
    /// <exception cref="ArgumentNullException">source is <see langword="null" />.</exception>
    /// <exception cref="NotSupportedException">The <see /> is read-only.</exception>
    internal DtosByIdsSpecification(IEnumerable<short>? dtosIds = null, bool trackData = false)
    {
        if (dtosIds != null)
        {
            var enumerable = dtosIds.ToList();
            AddFilter(e => enumerable.Contains(e.Id));
        }

        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
    }
}