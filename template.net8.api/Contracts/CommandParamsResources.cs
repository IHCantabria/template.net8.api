using System.Numerics;
using System.Text.Json.Serialization;
using template.net8.api.Communications.Interfaces;

namespace template.net8.api.Contracts;

/// <summary>
///     Command Create Dummy Params Resource
/// </summary>
public sealed partial record CommandCreateDummyParamsResource : IPublicApiContract,
    IEqualityOperators<CommandCreateDummyParamsResource, CommandCreateDummyParamsResource, bool>
{
    /// <summary>Dummy Text</summary>
    [JsonRequired]
    public required string Text { get; init; }
}