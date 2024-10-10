using System.Numerics;
using template.net8.api.Domain.Interfaces;

namespace template.net8.api.Domain.DTOs;

/// <summary>
///     Dummy DTO
/// </summary>
public sealed partial record DummyDto : IDto, IEqualityOperators<DummyDto, DummyDto, bool>
{
    /// <summary>
    ///     Key
    /// </summary>

    public required string Key { get; init; }

    /// <summary>
    ///     Text
    /// </summary>

    public required string Text { get; init; }
}