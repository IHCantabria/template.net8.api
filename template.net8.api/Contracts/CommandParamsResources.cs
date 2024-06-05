using System.Text.Json.Serialization;
using template.net8.Api.Communications.Interfaces;

namespace template.net8.Api.Contracts;

/// <summary>
///     Command Dummy Create Params Resource
/// </summary>
public sealed partial record CommandDummyCreateParamsResource : IPublicApiContract
{
    /// <summary>Dummy Text</summary>
    [JsonRequired]
    public required string Text { get; init; }
}