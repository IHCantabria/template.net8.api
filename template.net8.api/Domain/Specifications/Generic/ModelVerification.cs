using template.net8.api.Core.Attributes;
using template.net8.api.Domain.Base;
using template.net8.api.Domain.Persistence.Models.Interfaces;

namespace template.net8.api.Domain.Specifications.Generic;

[CoreLibrary]
internal sealed class EntityVerification<TEntity> : VerificationBase<TEntity> where TEntity : class, IEntity
{
    /// <exception cref="NotSupportedException">The <see /> is read-only.</exception>
    internal EntityVerification(short id)
    {
        AddFilter(e => e.Id == id);
    }
}

[CoreLibrary]
internal sealed class NamedEntityVerification<TEntity> : VerificationBase<TEntity>
    where TEntity : class, INamedEntity
{
    /// <exception cref="NotSupportedException">The <see /> is read-only.</exception>
    internal NamedEntityVerification(string name)
    {
        AddFilter(e => e.Name == name);
    }
}