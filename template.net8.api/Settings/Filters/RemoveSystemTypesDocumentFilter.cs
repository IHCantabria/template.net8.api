using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace template.net8.api.Settings.Filters;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class RemoveSystemTypesDocumentFilter : IDocumentFilter, IOrderedFilter
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public RemoveSystemTypesDocumentFilter()
    {
        Order = 1;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="swaggerDoc" /> is <see langword="null" />.
    ///     <paramref name="context" /> is <see langword="null" />.
    /// </exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        ArgumentNullException.ThrowIfNull(swaggerDoc);

        var keysToRemove = (from schema in swaggerDoc.Components.Schemas
            where schema.Key.StartsWith("System", StringComparison.InvariantCulture)
            select schema.Key).ToList();

        // Loop through all schemas in the swagger document

        // Remove the unwanted schemas
        foreach (var key in keysToRemove) swaggerDoc.Components.Schemas.Remove(key);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public int Order { get; }
}