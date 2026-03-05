using System.Diagnostics.CodeAnalysis;
using LanguageExt;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Base;
using template.net8.api.Settings.Options;
using Path = System.IO.Path;

namespace template.net8.api.Services.Base;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[ServiceLifetime(ServiceLifetime.Transient)]
[SuppressMessage(
    "ReSharper",
    "UnusedType.Global",
    Justification = "Base service/template intended for reuse; it may be referenced indirectly by derived services.")]
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification =
        "Protected helper members are intended for derived services; not all are used in every implementation.")]
internal class FilesManagerServiceBase(IOptions<FileStorageOptions> config, ILogger<FilesManagerServiceBase> logger)
    : ServiceBase(logger)
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private FileStorageOptions Config { get; } = config.Value ?? throw new ArgumentNullException(nameof(config));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
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
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    protected static Try<string> DefineTempFile(string tempDirectory)
    {
        return () => Path.Combine(tempDirectory, Path.GetRandomFileName());
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
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