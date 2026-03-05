using template.net8.api.Core.Contracts;

namespace template.net8.api.Core.DTOs;

internal sealed partial record InfoDto
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static implicit operator InfoResource(InfoDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return new InfoResource
        {
            Info = dto.Info,
            Version = dto.Version,
            Status = dto.Status
        };
    }
}