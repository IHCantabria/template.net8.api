using System.Numerics;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Domain.DTOs;

/// <summary>
///     Command Create Dummy Params Dto
/// </summary>
public sealed record CommandCreateDummyParamsDto : IDto,
    IEqualityOperators<CommandCreateDummyParamsDto, CommandCreateDummyParamsDto, bool>
{
    /// <summary>Dummy Text</summary>
    public required string Text { get; init; }
}