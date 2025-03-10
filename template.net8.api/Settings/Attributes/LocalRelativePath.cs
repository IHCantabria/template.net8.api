using System.ComponentModel.DataAnnotations;
using template.net8.api.Core.Attributes;
using Path = System.IO.Path;

namespace template.net8.api.Settings.Attributes;

/// <summary>
///     Relative Path Attribute
/// </summary>
[CoreLibrary]
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LocalRelativePathAttribute : ValidationAttribute
{
    /// <summary>
    ///     Checks if the path is a relative path
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string path) return new ValidationResult("Invalid type for relative path validation.");

        return IsRelativePath(path)
            ? ValidationResult.Success
            : new ValidationResult(GetErrorMessage(path));
    }

    private static bool IsRelativePath(string path)
    {
        // Check if the path is NOT rooted, meaning it is relative
        return !Path.IsPathRooted(path) && !IsUncPath(path);
    }

    private static bool IsUncPath(string path)
    {
        // Check if the path is a UNC path
        return Uri.TryCreate(path, UriKind.Absolute, out var uri) && uri.IsUnc;
    }

    private static string GetErrorMessage(string path)
    {
        return $"Invalid relative path: {path}. The path must not be absolute or a UNC path.";
    }
}