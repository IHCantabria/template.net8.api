using System.Numerics;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Core.DTOs;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed partial record VersionDto : IDto, IEqualityOperators<VersionDto, VersionDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required short Id { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string Name { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string Tag { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required DateTimeOffset Date { get; init; }
}