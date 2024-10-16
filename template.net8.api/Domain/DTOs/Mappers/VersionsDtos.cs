using JetBrains.Annotations;
using template.net8.api.Contracts;

namespace template.net8.api.Domain.DTOs;

/// <summary>
///     Version DTO
/// </summary>
public sealed partial record VersionDto
{
    /// <summary>
    ///     Convert Dto to Resource
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static implicit operator VersionResource(VersionDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return new VersionResource
        {
            Id = dto.Id,
            Name = dto.Name,
            Tag = dto.Tag,
            Date = dto.Date.UtcDateTime
        };
    }

    /// <summary>
    ///     This method is used to convert VersionDto to a VersionResource.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [UsedImplicitly]
    public static VersionResource ToVersionResource(VersionDto dto)
    {
        return dto;
    }
}