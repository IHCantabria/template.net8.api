using JetBrains.Annotations;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Domain.Persistence.Models.Interfaces;

/// <summary>
///     This interface is intended  to mark class models of the database context.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.WithMembers)]
[CoreLibrary]
public interface IDbModel
{
    /// <summary>
    ///     Check if the model is valid.
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
public interface IEntity : IDbModel
{
    /// <summary>
    ///     Id of the entity.
    /// </summary>
    public short Id { get; set; }
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
    public string Name { get; set; }
}

/// <summary>
///     This interface is intended  to mark class models of the database context that have Name and Alias fields.
/// </summary>
[CoreLibrary]
public interface IAliasEntity : INamedEntity
{
    /// <summary>
    ///     Alias of the entity.
    /// </summary>
    public string AliasText { get; set; }
}