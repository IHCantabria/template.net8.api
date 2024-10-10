using System.ComponentModel.DataAnnotations;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Attributes;

/// <summary>
/// </summary>
[CoreLibrary]
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LocalAbsolutePathAttribute : ValidationAttribute
{
    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string path) return new ValidationResult("Invalid type for local absolute path validation.");

        return IsLocalAbsolutePath(path)
            ? ValidationResult.Success
            : new ValidationResult(GetErrorMessage(path));
    }

    private static bool IsLocalAbsolutePath(string path)
    {
        // Check if the path is rooted and not a UNC path
        return Path.IsPathRooted(path) && !IsUncPath(path) && !path.EndsWith(Path.DirectorySeparatorChar);
    }

    private static bool IsUncPath(string path)
    {
        // Check if the path is a UNC path
        return Uri.TryCreate(path, UriKind.Absolute, out var uri) && uri.IsUnc;
    }

    private static string GetErrorMessage(string path)
    {
        return path.EndsWith(Path.DirectorySeparatorChar)
            ? "Path must not end with '/'. Please remove the trailing slash."
            : $"Invalid local absolute path: {path}";
    }
}