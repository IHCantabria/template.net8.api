using template.net8.api.Core.Contracts;

namespace template.net8.api.Core.DTOs;

internal sealed partial record VersionDto
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
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
}