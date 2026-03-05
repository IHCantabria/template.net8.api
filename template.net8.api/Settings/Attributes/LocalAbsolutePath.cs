using System.ComponentModel.DataAnnotations;
using Path = System.IO.Path;

namespace template.net8.api.Settings.Attributes;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
internal sealed class LocalAbsolutePathAttribute : ValidationAttribute
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string path) return new ValidationResult("Invalid type for local absolute path validation.");

        return IsLocalAbsolutePath(path)
            ? ValidationResult.Success
            : new ValidationResult(GetErrorMessage(path));
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static bool IsLocalAbsolutePath(string path)
    {
        // Check if the path is rooted and not a UNC path
        return Path.IsPathRooted(path) && !IsUncPath(path) &&
               !(path is [.., var lastChar] && lastChar == Path.DirectorySeparatorChar);
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
        return path is [.., var lastChar] && lastChar == Path.DirectorySeparatorChar
            ? "Path must not end with '/'. Please remove the trailing slash."
            : $"Invalid local absolute path: {path}";
    }
}