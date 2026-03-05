using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace template.net8.api.Core.Json;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class CamelCaseStringEnumerableConverter : JsonConverter<IEnumerable<string>>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="JsonException">Thrown when the current token is not the start of an array.</exception>
    public override IEnumerable<string> Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        ValidateArrayStart(reader);
        return ReadStringArray(ref reader);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="JsonException">Thrown when the current token is not the start of an array.</exception>
    private static void ValidateArrayStart(Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Expected JSON array");
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ReturnTypeCanBeEnumerable.Local",
        Justification =
            "Concrete return type is intentional for performance and to avoid interface-based enumeration overhead.")]
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
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="JsonException">Thrown when the token is neither a string nor null.</exception>
    [SuppressMessage(
        "ReSharper",
        "SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault",
        Justification = "Default case intentionally throws for unsupported JSON token types.")]
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
    ///     ADD DOCUMENTATION
    /// </summary>
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
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase",
        Justification = "Lowercase normalization is required to implement camelCase naming rules.")]
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