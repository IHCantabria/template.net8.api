using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Extensions;

namespace template.net8.api.Core.Json;

/// <summary>
///     Naming Policy Converter for JSON Serialization and Deserialization based on the header value. Default to SnakeCase
///     if header is not present or not recognized.
/// </summary>
[CoreLibrary]
public sealed class NamingPolicyConverter(IHttpContextAccessor httpContextAccessor) : JsonConverter<object>
{
    private readonly IHttpContextAccessor _httpContextAccessor =
        httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    /// <summary>
    ///     CanConvert method to check if the type can be converted. DONT DELETE THIS METHOD.
    /// </summary>
    /// <param name="typeToConvert"></param>
    /// <returns> </returns>
    [UsedImplicitly]
    public override bool CanConvert(Type typeToConvert)
    {
        return true; // Convert any type
    }

    /// <summary>
    ///     Read method to convert JSON to object based on the header value. Default to SnakeCase if header is not present or
    ///     not recognized.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns> </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">This property is set after serialization or deserialization has occurred.</exception>
    /// <exception cref="ArgumentException">
    ///     <paramref name="reader" /> contains unsupported options.
    ///     -or-
    ///     The current <paramref name="reader" /> token does not start or represent a value.
    /// </exception>
    /// <exception cref="JsonException">A value could not be read from the reader.</exception>
    /// <exception cref="NotSupportedException">
    ///     There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for
    ///     <paramref>
    ///         <name>returnType</name>
    ///     </paramref>
    ///     or its serializable members.
    /// </exception>
    /// <exception cref="ObjectDisposedException">The parent <see cref="JsonDocument" /> has been disposed.</exception>
    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        var headerValue = request?.Headers["x-json-input-naming-policy"].ToString();
        var namingPolicy = GetNamingPolicyFromHeader(headerValue);
        options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = namingPolicy
        };
        options.AddCoreOptions();

        using var doc = JsonDocument.ParseValue(ref reader);
        return JsonSerializer.Deserialize(doc.RootElement.GetRawText(), typeToConvert, options);
    }

    /// <summary>
    ///     Write method to convert object to JSON based on the header value. Default to SnakeCase if header is not present or
    ///     not recognized.
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">This property is set after serialization or deserialization has occurred.</exception>
    /// <exception cref="NotSupportedException">
    ///     There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for
    ///     <typeparamref>
    ///         <name>TValue</name>
    ///     </typeparamref>
    ///     or its serializable members.
    /// </exception>
    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        var headerValue = request?.Headers["x-json-output-naming-policy"].ToString();
        var namingPolicy = GetNamingPolicyFromHeader(headerValue);

        options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = namingPolicy
        };
        options.AddCoreOptions();

        JsonSerializer.Serialize(writer, value, options);
    }

    private static JsonNamingPolicy GetNamingPolicyFromHeader(string? headerValue)
    {
        if (string.Equals(headerValue, "camel", StringComparison.OrdinalIgnoreCase))
            return JsonNamingPolicy.CamelCase;
        if (string.Equals(headerValue, "snake", StringComparison.OrdinalIgnoreCase))
            return JsonNamingPolicy.SnakeCaseLower;

        return string.Equals(headerValue, "kebab", StringComparison.OrdinalIgnoreCase)
            ? JsonNamingPolicy.KebabCaseLower
            : JsonNamingPolicy.SnakeCaseLower; // Default to Snake if header is not present or not recognized
    }
}