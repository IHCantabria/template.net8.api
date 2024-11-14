using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Contracts;

/// <summary>
///     Query Get Dummy Params Resource
/// </summary>
public sealed partial record QueryGetDummyParamsResource : IPublicApiContract,
    IEqualityOperators<QueryGetDummyParamsResource, QueryGetDummyParamsResource, bool>
{
    /// <summary>Dummy Key</summary>
    [Required]
    [FromRoute(Name = "dummy-key")]
    public required string Key { get; init; }
}