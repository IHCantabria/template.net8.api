using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Json;

/// <summary>
///     Converts a string to camelCase format during JSON serialization and deserialization.
/// </summary>
[CoreLibrary]
internal sealed class CamelCaseStringConverter : JsonConverter<string>
{
    /// <summary>
    ///     Reads a string from JSON and converts it to camelCase format.
    /// </summary>
    /// <param name="reader">The UTF-8 JSON reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">The serialization options.</param>
    /// <returns>A string in camelCase format or null if the token is null.</returns>
    /// <exception cref="JsonException">Thrown when the token is neither a string nor null.</exception>
    /// <exception cref="InvalidOperationException">
    ///     The JSON token value isn't a string (that is, not a <see cref="System.Text.Json.JsonTokenType.String" />,
    ///     <see cref="System.Text.Json.JsonTokenType.PropertyName" />, or <see cref="System.Text.Json.JsonTokenType.Null" />).
    ///     -or-
    ///     The JSON string contains invalid UTF-8 bytes or invalid UTF-16 surrogates.
    /// </exception>
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        return reader.TokenType != JsonTokenType.String
            ? throw new JsonException("Expected JSON string")
            : ToCamelCase(reader.GetString());
    }

    /// <summary>
    ///     Writes a string to JSON, converting it to camelCase format.
    /// </summary>
    /// <param name="writer">The UTF-8 JSON writer.</param>
    /// <param name="value">The string to write.</param>
    /// <param name="options">The serialization options.</param>
    /// <exception cref="InvalidOperationException">
    ///     Validation is enabled, and the operation would result in writing invalid JSON.
    /// </exception>
    /// <exception cref="ArgumentException">The specified value is too large.</exception>
    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(ToCamelCase(value));
    }

    /// <summary>
    ///     Converts a string to camelCase format.
    /// </summary>
    /// <param name="str">The string to convert.</param>
    /// <returns>The string in camelCase format.</returns>
    [SuppressMessage("Pragma", "CA1308",
        Justification =
            "Business logic is change this string ToLowerInvariant")]
    private static string? ToCamelCase(string? str)
    {
        // Handle special cases
        if (string.IsNullOrEmpty(str) || str.Length == 1)
            return str?.ToLowerInvariant();

        // Optimization to avoid creating a new string if already in camelCase
        if (char.IsLower(str[0]))
            return str;

        // Convert the first character to lowercase and keep the rest unchanged
        return char.ToLowerInvariant(str[0]) + str[1..];
    }
}