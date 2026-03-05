using System.Diagnostics.CodeAnalysis;
using template.net8.api.Core.Base;
using template.net8.api.Persistence.Models.Interfaces;

namespace template.net8.api.Domain.Specifications.Generic;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class EntityVerificationById<TEntity, TKey> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithId<TKey> where TKey : struct
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal EntityVerificationById(TKey id)
    {
        AddFilter(e => e.Id.Equals(id));
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
internal sealed class EntitiesVerificationByIds<TEntity, TKey> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithId<TKey> where TKey : struct
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal EntitiesVerificationByIds(IEnumerable<TKey>? entityIds = null)
    {
        if (entityIds is null) return;

        var enumerable = entityIds.ToList();
        AddFilter(e => enumerable.Contains(e.Id));
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
internal sealed class EntityVerificationByDatahubId<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithDatahubId
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal EntityVerificationByDatahubId(short id)
    {
        AddFilter(e => e.DatahubId == id);
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
internal sealed class EntitiesVerificationByDatahubIds<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithDatahubId
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal EntitiesVerificationByDatahubIds(IEnumerable<short>? entityIds = null)
    {
        if (entityIds is null) return;

        var enumerable = entityIds.ToList();
        AddFilter(e => enumerable.Contains(e.DatahubId));
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class EntityVerificationByUuid<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithUuid
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal EntityVerificationByUuid(Guid uuid)
    {
        AddFilter(e => e.Uuid == uuid);
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
internal sealed class EntitiesVerificationByUuids<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithUuid
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal EntitiesVerificationByUuids(IEnumerable<string>? entityUuids = null)
    {
        if (entityUuids is null) return;

        var enumerable = entityUuids.ToList();
        AddFilter(e => enumerable.Contains(e.Uuid.ToString()));
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class EntityVerificationByNameKey<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithNameKey
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal EntityVerificationByNameKey(string name)
    {
        AddFilter(e => e.Name == name);
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
internal sealed class EntitiesVerificationByNameKeys<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithNameKey
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal EntitiesVerificationByNameKeys(IEnumerable<string>? entityNames = null)
    {
        if (entityNames is null) return;

        var enumerable = entityNames.ToList();
        AddFilter(e => enumerable.Contains(e.Name));
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
internal sealed class EntityVerificationByName<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithName
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal EntityVerificationByName(string name)
    {
        AddFilter(e => e.Name == name);
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
internal sealed class EntitiesVerificationByNames<TEntity> : VerificationBase<TEntity>
    where TEntity : class, IEntityWithNameKey
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal EntitiesVerificationByNames(IEnumerable<string>? entityNames = null)
    {
        if (entityNames is null) return;

        var enumerable = entityNames.ToList();
        AddFilter(e => enumerable.Contains(e.Name));
    }
}