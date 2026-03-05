using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Microsoft.Extensions.Options;
using template.net8.api.Settings.Attributes;

namespace template.net8.api.Settings.Options;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "Performance",
    "IDE0051:Remove unused private members",
    Justification = "Instantiated via configuration binding (IOptions) when the feature is enabled.")]
[SuppressMessage(
    "Performance",
    "UnusedAutoPropertyAccessor.Global",
    Justification = "Instantiated via configuration binding (IOptions) when the feature is enabled.")]
internal sealed record FileStorageOptions : IEqualityOperators<FileStorageOptions, FileStorageOptions, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public const string FileStorage = nameof(FileStorage);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [LocalAbsolutePath]
    public required string RootTempPath { get; init; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [LocalAbsolutePath]
    public required string RootFilePath { get; init; }
}

/// <summary>
///     File Storage Options Validator
/// </summary>
[OptionsValidator]
[SuppressMessage(
    "ReSharper",
    "UnusedType.Global",
    Justification =
        "Validator for options instantiated via configuration binding (IOptions) when the feature is enabled.")]
internal sealed partial class FileStorageOptionsValidator : IValidateOptions<FileStorageOptions>;