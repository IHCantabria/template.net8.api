using System.Numerics;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Core.DTOs;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed partial record InfoDto : IDto, IEqualityOperators<InfoDto, InfoDto, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string Version { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required string Info { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal required short Status { get; init; }
}