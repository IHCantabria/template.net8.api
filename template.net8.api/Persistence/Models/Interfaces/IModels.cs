using JetBrains.Annotations;

namespace template.net8.api.Persistence.Models.Interfaces;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.WithMembers)]
internal interface IEntity
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public bool Check()
    {
        return true;
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal interface IEntityWithId<TKey> : IEntity where TKey : struct
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    TKey Id { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal interface IEntityWithDatahubId : IEntity
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    short DatahubId { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal interface IEntityWithUuid : IEntity
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public Guid Uuid { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal interface IEntityWithName : IEntity
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    string Name { get; set; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal interface IEntityWithNameKey : IEntity
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    string Name { get; init; }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal interface IEntityWithAlias : IEntity
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    string AliasText { get; set; }
}