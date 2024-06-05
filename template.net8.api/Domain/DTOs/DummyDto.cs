using template.net8.api.Domain.Interfaces;

namespace template.net8.api.Domain.DTOs;

/// <summary>
///     Dummy DTO
/// </summary>
public sealed partial record DummyDto : IDto
{
    /// <summary>
    ///     Text
    /// </summary>

    public required string Text { get; init; }
}