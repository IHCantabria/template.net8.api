using JetBrains.Annotations;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Domain.Persistence.Models.Interfaces;

/// <summary>
///     This interface is intended  to mark class models of the database context.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.WithMembers)]
[CoreLibrary]
public interface IEntity
{
    /// <summary>
    ///     Check if the entity is valid.
    /// </summary>
    /// <returns></returns>
    public bool Check()
    {
        return true;
    }
}

/// <summary>
///     This interface is intended  to mark class models of the database context that have a primary key Id smallint type.
/// </summary>
[CoreLibrary]
public interface IEntityWithIdShort : IEntity
{
    /// <summary>
    ///     Pk of the entity.
    /// </summary>
    public short Id { get; init; }
}

/// <summary>
///     This interface is intended  to mark class models of the database context that have a primary key Id integer type.
/// </summary>
[CoreLibrary]
public interface IEntityWithId : IEntity
{
    /// <summary>
    ///     Pk of the entity.
    /// </summary>
    public int Id { get; init; }
}

/// <summary>
///     This interface is intended  to mark class models of the database context that have a primary key Id bigint type.
/// </summary>
[CoreLibrary]
public interface IEntityWithIdLong : IEntity
{
    /// <summary>
    ///     Pk of the entity.
    /// </summary>
    public long Id { get; init; }
}

/// <summary>
///     This interface is intended  to mark class models of the database context that have a primary key using Datahub Id.
/// </summary>
[CoreLibrary]
public interface IEntityWithDatahubId : IEntity
{
    /// <summary>
    ///     Pk of the entity.
    /// </summary>
    public short DatahubId { get; init; }
}

/// <summary>
///     This interface is intended  to mark class models of the database context that have a primary key uuid.
/// </summary>
[CoreLibrary]
public interface IEntityWithUuid : IEntity
{
    /// <summary>
    ///     Pk of the entity.
    /// </summary>
    public Guid Uuid { get; init; }
}

/// <summary>
///     This interface is intended  to mark class models of the database context that have a primary key uuid.
/// </summary>
[CoreLibrary]
public interface IEntityWithName : IEntity
{
    /// <summary>
    ///     Name of the entity.
    /// </summary>
    public string Name { get; set; }
}

/// <summary>
///     This interface is intended  to mark class models of the database context that have Name mutable field.
/// </summary>
[CoreLibrary]
public interface IEntityWithNameKey : IEntity
{
    /// <summary>
    ///     Name of the entity.
    /// </summary>
    public string Name { get; init; }
}

/// <summary>
///     This interface is intended  to mark class models of the database context that have Name and Alias fields.
/// </summary>
[CoreLibrary]
public interface IEntityWithAlias : IEntity
{
    /// <summary>
    ///     Alias of the entity.
    /// </summary>
    public string AliasText { get; set; }
}