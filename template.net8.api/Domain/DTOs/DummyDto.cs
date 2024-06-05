using template.net8.Api.Domain.Interfaces;

namespace template.net8.Api.Domain.DTOs;

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