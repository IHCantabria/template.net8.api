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
///     This interface is intended  to mark class models of the database context that have a primary key Id.
/// </summary>
[CoreLibrary]
public interface IEntityWithId : IEntity
{
    /// <summary>
    ///     Pk of the entity.
    /// </summary>
    public short Id { get; init; }
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
    public string Uuid { get; init; }
}

/// <summary>
///     This interface is intended  to mark class models of the database context that have Name field.
/// </summary>
[CoreLibrary]
public interface INamedEntity : IEntity
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
public interface IAliasEntity : IEntity
{
    /// <summary>
    ///     Alias of the entity.
    /// </summary>
    public string AliasText { get; init; }
}