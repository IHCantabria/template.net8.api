using LanguageExt.Common;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.File;

[CoreLibrary]
internal static class FileReader
{
    /// <summary>
    ///     Converts a file to a byte array asynchronously.
    /// </summary>
    /// <param name="file">The file to convert.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="bufferSize">The buffer size in bytes. Default is 65536 (64 KB).</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the byte array.</returns>
    /// <exception cref="ArrayTypeMismatchException">
    ///     <paramref>
    ///         <name>array</name>
    ///     </paramref>
    ///     is covariant, and the array's type is not exactly <see langword="T[]" />.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref>
    ///         <name>start</name>
    ///     </paramref>
    ///     ,
    ///     <paramref>
    ///         <name>length</name>
    ///     </paramref>
    ///     , or
    ///     <paramref>
    ///         <name>start</name>
    ///     </paramref>
    ///     +
    ///     <paramref>
    ///         <name>length</name>
    ///     </paramref>
    ///     is not in the range of
    ///     <paramref>
    ///         <name>array</name>
    ///     </paramref>
    ///     .
    /// </exception>
    internal static async Task<Result<byte[]>> ConvertFileToByteArrayAsync(IFormFile file,
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
    ///     Converts a file to a byte array synchronously.
    /// </summary>
    /// <param name="file">The file to convert.</param>
    /// <param name="bufferSize">The buffer size in bytes. Default is 65536 (64 KB).</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the byte array.</returns>
    /// <exception cref="ArrayTypeMismatchException">
    ///     <paramref>
    ///         <name>array</name>
    ///     </paramref>
    ///     is covariant, and the array's type is not exactly <see langword="T[]" />.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref>
    ///         <name>start</name>
    ///     </paramref>
    ///     ,
    ///     <paramref>
    ///         <name>length</name>
    ///     </paramref>
    ///     , or
    ///     <paramref>
    ///         <name>start</name>
    ///     </paramref>
    ///     +
    ///     <paramref>
    ///         <name>length</name>
    ///     </paramref>
    ///     is not in the range of
    ///     <paramref>
    ///         <name>array</name>
    ///     </paramref>
    ///     .
    /// </exception>
    internal static Result<byte[]> ConvertFileToByteArray(IFormFile file,
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