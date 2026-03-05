using System.Diagnostics.CodeAnalysis;

namespace template.net8.api.Settings.Interfaces;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "ReSharper",
    "UnusedType.Global",
    Justification = "Reusable configuration contract; it may not be used in every application scenario.")]
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification =
        "Members are part of the reusable configuration contract and may not be used in all implementations.")]
internal interface IHttpClientsOptions : IConnectionsOptions
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Uri UriBase { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    TimeSpan Timeout { get; init; }
}