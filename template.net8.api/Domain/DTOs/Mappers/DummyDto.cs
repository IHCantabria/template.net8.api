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

    internal static IEnumerable<DummyResource> ToCollection(
        IReadOnlyList<DummyDto> dtos)
    {
        var resources = new DummyResource[dtos.Count];
        for (var i = 0; i < dtos.Count; i++) resources[i] = dtos[i];
        return resources;
    }
}