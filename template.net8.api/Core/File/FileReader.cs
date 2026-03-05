using System.Diagnostics.CodeAnalysis;

namespace template.net8.api.Core.File;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "ReSharper",
    "UnusedType.Global",
    Justification = "General-purpose helper type; usage depends on consumer requirements.")]
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification = "General-purpose helper methods; not all members are used in every scenario.")]
internal static class FileReader
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumented",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static async Task<LanguageExt.Common.Result<byte[]>> ConvertFileToByteArrayAsync(IFormFile file,
        CancellationToken cancellationToken,
        int bufferSize = 65536)
    {
        using var memoryStream = new MemoryStream();
        var stream = file.OpenReadStream();
        await using (stream)
        {
            var buffer = new byte[bufferSize];
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer.AsMemory(0, bufferSize), cancellationToken)
                       .ConfigureAwait(false)) >
                   0)
                await memoryStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken)
                    .ConfigureAwait(false);
        }

        return memoryStream.ToArray();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumented",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static LanguageExt.Common.Result<byte[]> ConvertFileToByteArray(IFormFile file,
        int bufferSize = 65536)
    {
        using var memoryStream = new MemoryStream();
        var stream = file.OpenReadStream();
        using (stream)
        {
            var buffer = new byte[bufferSize];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer.AsMemory(0, bufferSize).Span)) > 0)
                memoryStream.Write(buffer.AsMemory(0, bytesRead).Span);
        }

        return memoryStream.ToArray();
    }
}