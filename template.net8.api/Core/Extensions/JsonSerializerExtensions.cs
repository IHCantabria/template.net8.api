using System.Text.Json;
using System.Text.Json.Serialization;
using template.net8.Api.Core.Attributes;

namespace template.net8.Api.Core.Extensions;

[CoreLibrary]
internal static class JsonSerializerExtensions
{
    /// <exception cref="InvalidOperationException">This property is set after serialization or deserialization has occurred.</exception>
    internal static JsonSerializerOptions AddCoreOptions(this JsonSerializerOptions options)
    {
        options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
        return options;
    }
}