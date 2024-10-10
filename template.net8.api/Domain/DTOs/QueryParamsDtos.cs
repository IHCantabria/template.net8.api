using System.Numerics;
using template.net8.api.Domain.Interfaces;

namespace template.net8.api.Domain.DTOs;

/// <summary>
///     Query Get Dummy Params Dto
/// </summary>
public sealed record QueryGetDummyParamsDto : IDto,
    IEqualityOperators<QueryGetDummyParamsDto, QueryGetDummyParamsDto, bool>
{
    /// <summary>Dummy Key</summary>
    public required string Key { get; init; }
}