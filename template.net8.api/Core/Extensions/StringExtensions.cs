using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace template.net8.api.Core.Extensions;

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
internal static class StringExtensions
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static readonly Regex UnwantedCharsRegex = new(
        @"[\p{C}\p{Zl}\p{Zp}\u200B-\u200D\uFEFF\p{So}]", // Incluye caracteres invisibles y emojis
        RegexOptions.Compiled);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static string RemoveUnwantedCharacters(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        // Trim spaces and remove unwanted characters using regex
        var trimmedInput = input.AsSpan().Trim();
        return trimmedInput.IsEmpty ? string.Empty : UnwantedCharsRegex.Replace(trimmedInput.ToString(), "");
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static string CleanString(this string input, char[]? additionalCharsToRemove = null)
    {
        // Remove unwanted characters first
        var cleaned = RemoveUnwantedCharacters(input);

        // If no additional characters to remove, return the result
        if (additionalCharsToRemove is not { Length: not 0 })
            return cleaned;

        // Remove additional characters
        StringBuilder finalCleaned = new(cleaned.Length);
        foreach (var c in cleaned.Where(c => !additionalCharsToRemove.Contains(c)))
            finalCleaned.Append(c);

        return finalCleaned.ToString();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentException">The current instance contains invalid Unicode characters.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static string RemoveDiacritics(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return text;

        return string.Concat(
            text.Normalize(NormalizationForm.FormD)
                .Where(static c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
        ).Normalize(NormalizationForm.FormC);
    }
}