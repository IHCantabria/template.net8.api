using template.net8.api.Core.Attributes;
using template.net8.api.Domain.Base;
using template.net8.api.Domain.Persistence.Models.Interfaces;

namespace template.net8.api.Domain.Specifications.Generic;

[CoreLibrary]
internal sealed class EntityVerificationById<TEntity> : VerificationBase<TEntity> where TEntity : class, IEntityWithId
{
    /// <exception cref="NotSupportedException">The <see /> is read-only.</exception>
    internal EntityVerificationById(short id)
    {
        AddFilter(e => e.Id == id);
    }
}

[CoreLibrary]
internal sealed class EntitiesVerificationByIds<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithId
{
    /// <exception cref="ArgumentNullException">source is <see langword="null" />.</exception>
    /// <exception cref="NotSupportedException">The <see /> is read-only.</exception>
    internal EntitiesVerificationByIds(IEnumerable<short>? entityIds = null)
    {
        if (entityIds is null) return;
        var enumerable = entityIds.ToList();
        AddFilter(e => enumerable.Contains(e.Id));
    }
}

[CoreLibrary]
internal sealed class EntityVerificationByUuid<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithUuid
{
    /// <exception cref="NotSupportedException">The <see /> is read-only.</exception>
    internal EntityVerificationByUuid(string uuid)
    {
        AddFilter(e => e.Uuid == uuid);
    }
}

[CoreLibrary]
internal sealed class EntitiesVerificationByUuids<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithUuid
{
    /// <exception cref="ArgumentNullException">source is <see langword="null" />.</exception>
    /// <exception cref="NotSupportedException">The <see /> is read-only.</exception>
    internal EntitiesVerificationByUuids(IEnumerable<string>? entityUuids = null)
    {
        if (entityUuids is null) return;
        var enumerable = entityUuids.ToList();
        AddFilter(e => enumerable.Contains(e.Uuid));
    }
}

[CoreLibrary]
internal sealed class EntityVerificationByName<TEntity> : VerificationBase<TEntity>
    where TEntity : class, INamedEntity
{
    /// <exception cref="NotSupportedException">The <see /> is read-only.</exception>
    internal EntityVerificationByName(string name)
    {
        AddFilter(e => e.Name == name);
    }
}

[CoreLibrary]
internal sealed class EntitiesVerificationByNames<TEntity> : VerificationBase<TEntity>
    where TEntity : class, INamedEntity
{
    /// <exception cref="ArgumentNullException">source is <see langword="null" />.</exception>
    /// <exception cref="NotSupportedException">The <see /> is read-only.</exception>
    internal EntitiesVerificationByNames(IEnumerable<string>? entityNames = null)
    {
        if (entityNames is null) return;
        var enumerable = entityNames.ToList();
        AddFilter(e => enumerable.Contains(e.Name));
    }
}