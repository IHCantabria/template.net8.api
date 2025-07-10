using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Json;

/// <summary>
///     Converts strings to camelCase format during JSON serialization and deserialization.
/// </summary>
[CoreLibrary]
internal sealed class CamelCaseStringEnumerableConverter : JsonConverter<IEnumerable<string>>
{
    /// <summary>
    ///     Reads a collection of strings from JSON and converts each item to camelCase format.
    /// </summary>
    /// <param name="reader">The UTF-8 JSON reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">The serialization options.</param>
    /// <returns>A collection of strings in camelCase format or null if the token is null.</returns>
    /// <exception cref="JsonException">Thrown when the current token is not the start of an array.</exception>
    public override IEnumerable<string> Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        ValidateArrayStart(reader);
        return ReadStringArray(ref reader);
    }

    /// <summary>
    ///     Validates that the current token is the start of an array.
    /// </summary>
    /// <param name="reader">The JSON reader.</param>
    /// <exception cref="JsonException">Thrown when the current token is not the start of an array.</exception>
    private static void ValidateArrayStart(Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Expected JSON array");
    }

    /// <summary>
    ///     Reads a string array from JSON, converting each element to camelCase.
    /// </summary>
    /// <param name="reader">The JSON reader.</param>
    /// <returns>A collection of camelCase strings.</returns>
    private static List<string> ReadStringArray(ref Utf8JsonReader reader)
    {
        var result = new List<string>();

        // Move past the start array token
        reader.Read();

        while (reader.TokenType != JsonTokenType.EndArray)
        {
            result.Add(ReadArrayElement(ref reader) ?? throw new InvalidOperationException());
            reader.Read();
        }

        return result;
    }

    /// <summary>
    ///     Reads a single element from a JSON array.
    /// </summary>
    /// <param name="reader">The JSON reader.</param>
    /// <returns>The element converted to camelCase if it's a string.</returns>
    /// <exception cref="JsonException">Thrown when the token is neither a string nor null.</exception>
    private static string? ReadArrayElement(ref Utf8JsonReader reader)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => ToCamelCase(reader.GetString()),
            JsonTokenType.Null => null,
            _ => throw new JsonException($"Unexpected token type: {reader.TokenType}")
        };
    }

    /// <summary>
    ///     Writes a collection of strings to JSON, converting each to camelCase format.
    /// </summary>
    /// <param name="writer">The UTF-8 JSON writer.</param>
    /// <param name="value">The collection to write.</param>
    /// <param name="options">The serialization options.</param>
    /// <exception cref="InvalidOperationException">
    ///     Validation is enabled, and the operation would result in writing invalid
    ///     JSON.
    /// </exception>
    /// <exception cref="ArgumentException">The specified value is too large.</exception>
    public override void Write(Utf8JsonWriter writer, IEnumerable<string> value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();

        foreach (var item in value)
            if (item is null)
                writer.WriteNullValue();
            else
                writer.WriteStringValue(ToCamelCase(item));

        writer.WriteEndArray();
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