using System.Numerics;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Core.DTOs;

/// <summary>
///     Version DTO
/// </summary>
[CoreLibrary]
public sealed partial record VersionDto : IDto, IEqualityOperators<VersionDto, VersionDto, bool>
{
    /// <summary>
    ///     id
    /// </summary>
    public required short Id { get; init; }

    /// <summary>
    ///     Name
    /// </summary>
    public required string Name { get; init; } = null!;

    /// <summary>
    ///     Tag
    /// </summary>
    public required string Tag { get; init; } = null!;

    /// <summary>
    ///     Date UTC
    /// </summary>
    public required DateTimeOffset Date { get; init; }
}