using template.net8.api.Core.DTOs;
using template.net8.api.Core.Interfaces;
using template.net8.api.Domain.DTOs;

namespace template.net8.api.Persistence.Models;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class CurrentVersionProjections
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static IProjection<CurrentVersion, VersionDto> VersionProjection =>
        new Projection<CurrentVersion, VersionDto>(static p => new VersionDto
        {
            Id = p.Version.Id,
            Name = p.Version.Name,
            Tag = p.Version.Tag,
            Date = p.Version.Date
        });
}