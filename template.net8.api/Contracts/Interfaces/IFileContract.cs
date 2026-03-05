using System.Diagnostics.CodeAnalysis;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Contracts.Interfaces;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "The interface is part of the public API contract and must remain publicly accessible.")]
[PublicApiContract]
public interface IFileContract
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    IEnumerable<byte> Data { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    string FileName { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    string ContentType { get; init; }
}