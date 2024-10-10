using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Attributes;

namespace template.net8.api.Settings.Options;

/// <summary>
///     File Storage Options for the application
/// </summary>
[CoreLibrary]
public sealed record FileStorageOptions : IEqualityOperators<FileStorageOptions, FileStorageOptions, bool>
{
    /// <summary>
    ///     AppSettings Key for File Storage
    /// </summary>
    public const string FileStorage = nameof(FileStorage);

    /// <summary>
    ///     Root Temp Path
    /// </summary>
    [Required]
    [LocalAbsolutePath]
    public string RootTempPath { get; init; } = null!;

    /// <summary>
    ///     Root File Path
    /// </summary>
    [Required]
    [LocalAbsolutePath]
    public string RootFilePath { get; init; } = null!;
}

/// <summary>
///     File Storage Options Validator
/// </summary>
[OptionsValidator]
[CoreLibrary]
public sealed partial class FileStorageOptionsValidator : IValidateOptions<FileStorageOptions>;