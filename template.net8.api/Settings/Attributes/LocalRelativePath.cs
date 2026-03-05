using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Path = System.IO.Path;

namespace template.net8.api.Settings.Attributes;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
[SuppressMessage(
    "ReSharper",
    "UnusedType.Global",
    Justification =
        "Demonstration/base attribute provided as reusable validation example; usage depends on consumer scenarios.")]
internal sealed class LocalRelativePathAttribute : ValidationAttribute
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string path) return new ValidationResult("Invalid type for relative path validation.");

        return IsRelativePath(path)
            ? ValidationResult.Success
            : new ValidationResult(GetErrorMessage(path));
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static bool IsRelativePath(string path)
    {
        // Check if the path is NOT rooted, meaning it is relative
        return !Path.IsPathRooted(path) && !IsUncPath(path);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static bool IsUncPath(string path)
    {
        // Check if the path is a UNC path
        return Uri.TryCreate(path, UriKind.Absolute, out var uri) && uri.IsUnc;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static string GetErrorMessage(string path)
    {
        return $"Invalid relative path: {path}. The path must not be absolute or a UNC path.";
    }
}