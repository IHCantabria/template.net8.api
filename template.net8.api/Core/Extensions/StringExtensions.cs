using System.Text;
using System.Text.RegularExpressions;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Extensions;

[CoreLibrary]
internal static class StringExtensions
{
    /// <summary>
    ///     Precompiled regex for unwanted characters (invisible, emojis, etc.)
    /// </summary>
    private static readonly Regex UnwantedCharsRegex = new(
        @"[\p{C}\p{Zl}\p{Zp}\u200B-\u200D\uFEFF\p{So}]", // Incluye caracteres invisibles y emojis
        RegexOptions.Compiled);

    /// <summary>
    ///     Removes unwanted characters such as invisible characters, emojis, and trims spaces from the input.
    /// </summary>
    /// <param name="input">The input string to be cleaned.</param>
    /// <returns>A string with unwanted characters removed.</returns>
    private static string RemoveUnwantedCharacters(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        // Trim spaces and remove unwanted characters using regex
        var trimmedInput = input.AsSpan().Trim();
        return trimmedInput.IsEmpty ? string.Empty : UnwantedCharsRegex.Replace(trimmedInput.ToString(), "");
    }

    /// <summary>
    ///     Cleans a string by removing unwanted characters and optionally additional user-defined characters.
    /// </summary>
    /// <param name="input">The input string to be cleaned.</param>
    /// <param name="additionalCharsToRemove">Optional: Additional characters to remove from the string.</param>
    /// <returns>A cleaned string.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref>
    ///         <name>capacity</name>
    ///     </paramref>
    ///     is less than zero.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     or
    ///     <paramref>
    ///         <name>predicate</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    internal static string CleanString(this string input, string? additionalCharsToRemove = null)
    {
        // Remove unwanted characters first
        var cleaned = RemoveUnwantedCharacters(input);

        // If no additional characters to remove, return the result
        if (string.IsNullOrEmpty(additionalCharsToRemove))
        {
            return cleaned;
        }

        // Remove additional characters
        StringBuilder finalCleaned = new(cleaned.Length);
        foreach (var c in cleaned.Where(c => !additionalCharsToRemove.Contains(c)))
        {
            finalCleaned.Append(c);
        }

        return finalCleaned.ToString();
    }
}