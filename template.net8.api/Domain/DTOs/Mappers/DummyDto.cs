using JetBrains.Annotations;
using template.net8.api.Contracts;

namespace template.net8.api.Domain.DTOs;

/// <summary>
///     Dummy DTO
/// </summary>
public sealed partial record DummyDto
{
    /// <summary>
    ///     Convert Dto to Resource
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static implicit operator DummyResource(DummyDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return new DummyResource
        {
            Key = dto.Key,
            Text = dto.Text
        };
    }

    /// <summary>
    ///     This method is used to convert DummyDto to a DummyResource.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    [UsedImplicitly]
    public static DummyResource ToDummyResource(DummyDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return dto;
    }


    /// <summary>
    ///     This method is used to convert a collection of DummyDto to a collection of DummyResource.
    /// </summary>
    /// <param name="dtos"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     or
    ///     <paramref>
    ///         <name>selector</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public static IEnumerable<DummyResource> ToCollection(
        IEnumerable<DummyDto> dtos)
    {
        // Si el enumerable ya es una lista, iteramos más rápido usando índices
        if (dtos is not IReadOnlyList<DummyDto> list) return dtos.Select(ToDummyResource);

        var resources = new DummyResource[list.Count];
        for (var i = 0; i < list.Count; i++)
            resources[i] = list[i];
        return resources;
    }
}