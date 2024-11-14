using template.net8.api.Core.Attributes;

namespace template.net8.api.Contracts.Interfaces;

/// <summary>
///     This interfaces is intended to mark file contracts
/// </summary>
[PublicApiContract]
[CoreLibrary]
internal interface IFileContract
{
    internal IEnumerable<byte> Data { get; init; }

    internal string FileName { get; init; }

    internal string ContentType { get; init; }
}