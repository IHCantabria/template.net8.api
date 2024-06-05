using template.net8.Api.Domain.Interfaces;

namespace template.net8.Api.Domain.DTOs;

/// <summary>
///     Command Dummy Create Params Dto
/// </summary>
public sealed record CommandDummyCreateParamsDto : IDto
{
    /// <summary>Dummy Text</summary>
    public required string Text { get; init; }
}