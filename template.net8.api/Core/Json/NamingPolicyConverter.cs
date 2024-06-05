using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using template.net8.Api.Core.Attributes;
using template.net8.Api.Core.Extensions;

namespace template.net8.Api.Core.Json;

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
    /// <returns></returns>
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
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">This property is set after serialization or deserialization has occurred.</exception>
    /// <exception cref="ArgumentException">
    ///     <paramref name="reader" /> contains unsupported options.
    ///     -or-
    ///     The current <paramref name="reader" /> token does not start or represent a value.
    /// </exception>
    /// <exception cref="JsonException">
    ///     The JSON is invalid.
    ///     -or-
    ///     <typeparamref /> is not compatible with the JSON.
    ///     -or-
    ///     There is remaining data in the string beyond a single JSON value.
    /// </exception>
    /// <exception cref="ObjectDisposedException">The parent <see cref="JsonDocument" /> has been disposed.</exception>
    /// <exception cref="NotSupportedException">
    ///     There is no compatible
    ///     <see cref="System.Text.Json.Serialization.JsonConverter" /> for <paramref /> or its serializable members.
    /// </exception>
    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
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
    /// <exception cref="InvalidOperationException">This property is set after serialization or deserialization has occurred.</exception>
    /// <exception cref="NotSupportedException">
    ///     There is no compatible
    ///     <see cref="System.Text.Json.Serialization.JsonConverter" /> for <typeparamref /> or its serializable members.
    /// </exception>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
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