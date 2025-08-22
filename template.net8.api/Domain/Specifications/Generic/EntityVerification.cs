using template.net8.api.Core.Attributes;
using template.net8.api.Core.Base;
using template.net8.api.Domain.Persistence.Models.Interfaces;

namespace template.net8.api.Domain.Specifications.Generic;

[CoreLibrary]
internal sealed class EntityVerificationById<TEntity, TKey> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithId<TKey> where TKey : struct
{
    /// <summary>
    ///     Constructs a specification to filter an entity by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    internal EntityVerificationById(TKey id)
    {
        AddFilter(e => e.Id.Equals(id));
    }
}

[CoreLibrary]
internal sealed class EntitiesVerificationByIds<TEntity, TKey> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithId<TKey> where TKey : struct
{
    /// <summary>
    ///     Constructs a specification to filter entities by a collection of IDs.
    /// </summary>
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
    internal EntitiesVerificationByIds(IEnumerable<TKey>? entityIds = null)
    {
        if (entityIds is null) return;

        var enumerable = entityIds.ToList();
        AddFilter(e => enumerable.Contains(e.Id));
    }
}

[CoreLibrary]
internal sealed class EntityVerificationByDatahubId<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithDatahubId
{
    /// <summary>
    ///     Constructs a specification to filter an entity by its Datahub ID.
    /// </summary>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    internal EntityVerificationByDatahubId(short id)
    {
        AddFilter(e => e.DatahubId == id);
    }
}

[CoreLibrary]
internal sealed class EntitiesVerificationByDatahubIds<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithDatahubId
{
    /// <summary>
    ///     Constructs a specification to filter entities by a collection of Datahub IDs.
    /// </summary>
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
    internal EntitiesVerificationByDatahubIds(IEnumerable<short>? entityIds = null)
    {
        if (entityIds is null) return;

        var enumerable = entityIds.ToList();
        AddFilter(e => enumerable.Contains(e.DatahubId));
    }
}

[CoreLibrary]
internal sealed class EntityVerificationByUuid<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithUuid
{
    /// <summary>
    ///     Constructs a specification to filter an entity by its UUID.
    /// </summary>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    internal EntityVerificationByUuid(Guid uuid)
    {
        AddFilter(e => e.Uuid == uuid);
    }
}

[CoreLibrary]
internal sealed class EntitiesVerificationByUuids<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithUuid
{
    /// <summary>
    ///     Constructs a specification to filter entities by a collection of UUIDs.
    /// </summary>
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
    internal EntitiesVerificationByUuids(IEnumerable<string>? entityUuids = null)
    {
        if (entityUuids is null) return;

        var enumerable = entityUuids.ToList();
        AddFilter(e => enumerable.Contains(e.Uuid.ToString()));
    }
}

[CoreLibrary]
internal sealed class EntityVerificationByNameKey<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithNameKey
{
    /// <summary>
    ///     Constructs a specification to filter an entity by its Name.
    /// </summary>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    internal EntityVerificationByNameKey(string name)
    {
        AddFilter(e => e.Name == name);
    }
}

[CoreLibrary]
internal sealed class EntitiesVerificationByNameKeys<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithNameKey
{
    /// <summary>
    ///     Constructs a specification to filter entities by a collection of Names.
    /// </summary>
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
    internal EntitiesVerificationByNameKeys(IEnumerable<string>? entityNames = null)
    {
        if (entityNames is null) return;

        var enumerable = entityNames.ToList();
        AddFilter(e => enumerable.Contains(e.Name));
    }
}

[CoreLibrary]
internal sealed class EntityVerificationByName<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithName
{
    /// <summary>
    ///     Constructs a specification to filter an entity by its Name.
    /// </summary>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    internal EntityVerificationByName(string name)
    {
        AddFilter(e => e.Name == name);
    }
}

[CoreLibrary]
internal sealed class EntitiesVerificationByNames<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithNameKey
{
    /// <summary>
    ///     Constructs a specification to filter entities by a collection of Names.
    /// </summary>
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
    internal EntitiesVerificationByNames(IEnumerable<string>? entityNames = null)
    {
        if (entityNames is null) return;

        var enumerable = entityNames.ToList();
        AddFilter(e => enumerable.Contains(e.Name));
    }
}