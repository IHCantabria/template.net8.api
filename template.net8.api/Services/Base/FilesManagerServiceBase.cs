using LanguageExt;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Options;

namespace template.net8.api.Services.Base;

/// <summary>
///     Files Manager Service Base
/// </summary>
[ServiceLifetime(ServiceLifetime.Transient)]
[CoreLibrary]
public class FilesManagerServiceBase(IOptions<FileStorageOptions> config, ILogger<FilesManagerServiceBase> logger)
    : ServiceBase(logger)
{
    private FileStorageOptions Config { get; } = config.Value ?? throw new ArgumentNullException(nameof(config));

    /// <summary>
    ///     Create Temp Directory for file processing
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    ///     .NET Framework and .NET Core versions older than 2.1:
    ///     <paramref>
    ///         <name>path1</name>
    ///     </paramref>
    ///     or
    ///     <paramref>
    ///         <name>path2</name>
    ///     </paramref>
    ///     contains one or more of the invalid characters defined in
    ///     <see>
    ///         <cref>M:System.IO.Path.GetInvalidPathChars</cref>
    ///     </see>
    ///     .
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>path1</name>
    ///     </paramref>
    ///     or
    ///     <paramref>
    ///         <name>path2</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="IOException">
    ///     The directory specified by
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    ///     is a file.
    ///     -or-
    ///     The network name is not known.
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    ///     contains a colon character (:) that is not part of a drive label ("C:\").
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    protected Try<string> CreateTempDirectory()
    {
        return () =>
        {
            var tempDirectory = Path.Combine(Config.RootTempPath, Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        };
    }

    /// <summary>
    ///     Define Temp File
    /// </summary>
    /// <param name="tempDirectory"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    ///     .NET Framework and .NET Core versions older than 2.1:
    ///     <paramref>
    ///         <name>path1</name>
    ///     </paramref>
    ///     or
    ///     <paramref>
    ///         <name>path2</name>
    ///     </paramref>
    ///     contains one or more of the invalid characters defined in
    ///     <see>
    ///         <cref>M:System.IO.Path.GetInvalidPathChars</cref>
    ///     </see>
    ///     .
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>path1</name>
    ///     </paramref>
    ///     or
    ///     <paramref>
    ///         <name>path2</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    protected static Try<string> DefineTempFile(string tempDirectory)
    {
        return () =>
        {
            var tempZipPath = Path.Combine(tempDirectory, Path.GetRandomFileName());
            return tempZipPath;
        };
    }

    /// <summary>
    ///     Delete File Directory
    /// </summary>
    /// <param name="filepDirectory"></param>
    /// <exception cref="IOException">
    ///     A file with the same name and location specified by
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    ///     exists.
    ///     -or-
    ///     The directory specified by
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    ///     is read-only, or
    ///     <paramref>
    ///         <name>recursive</name>
    ///     </paramref>
    ///     is <see langword="false" /> and
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    ///     is not an empty directory.
    ///     -or-
    ///     The directory is the application's current working directory.
    ///     -or-
    ///     The directory contains a read-only file.
    ///     -or-
    ///     The directory is being used by another process.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    ///     does not exist or could not be found.
    ///     -or-
    ///     The specified path is invalid (for example, it is on an unmapped drive).
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="ArgumentException">
    ///     .NET Framework and .NET Core versions older than 2.1:
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    ///     is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for
    ///     invalid characters by using the
    ///     <see>
    ///         <cref>M:System.IO.Path.GetInvalidPathChars</cref>
    ///     </see>
    ///     method.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    protected static Try<bool> TryDeleteFileDirectory(string filepDirectory)
    {
        return () =>
        {
            if (Directory.Exists(filepDirectory))
                Directory.Delete(filepDirectory, true);
            return true;
        };
    }
}