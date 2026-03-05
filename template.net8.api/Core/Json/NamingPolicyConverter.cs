using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using template.net8.api.Core.Extensions;

namespace template.net8.api.Core.Json;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class NamingPolicyConverter(IHttpContextAccessor httpContextAccessor) : JsonConverter<object>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor =
        httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public override bool CanConvert(Type typeToConvert)
    {
        return true;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ObjectDisposedException">The parent <see cref="JsonDocument" /> has been disposed.</exception>
    /// <exception cref="JsonException">A value could not be read from the reader.</exception>
    /// <exception cref="ArgumentException">
    ///     <paramref name="reader" /> contains unsupported options.
    ///     -or-
    ///     The current <paramref name="reader" /> token does not start or represent a value.
    /// </exception>
    /// <exception cref="InvalidOperationException">This property is set after serialization or deserialization has occurred.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
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
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="writer" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">This property is set after serialization or deserialization has occurred.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
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

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
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