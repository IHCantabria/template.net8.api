using JetBrains.Annotations;
using template.net8.api.Contracts;

namespace template.net8.api.Domain.Persistence.Models;

/// <summary>
///     Dummy
/// </summary>
public partial class Dummy
{
    /// <summary>
    ///     Convert Entity to Resource
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static implicit operator DummyCreatedResource(Dummy entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new DummyCreatedResource
        {
            Text = entity.Text,
            Key = entity.Key
        };
    }

    /// <summary>
    ///     This method converts a Dummy to a DummyCreatedResource
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [UsedImplicitly]
    public static DummyCreatedResource ToDummyCreatedResource(
        Dummy entity)
    {
        return entity;
    }
}