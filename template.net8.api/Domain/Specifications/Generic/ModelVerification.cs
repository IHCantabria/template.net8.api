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
internal sealed class NamedEntityVerificationByName<TEntity> : VerificationBase<TEntity>
    where TEntity : class, INamedEntity
{
    /// <exception cref="NotSupportedException">The <see /> is read-only.</exception>
    internal NamedEntityVerificationByName(string name)
    {
        AddFilter(e => e.Name == name);
    }
}