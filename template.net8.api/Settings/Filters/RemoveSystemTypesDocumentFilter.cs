using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Filters;

/// <summary>
///     Remove System Types Document Filter
/// </summary>
[CoreLibrary]
public sealed class RemoveSystemTypesDocumentFilter : IDocumentFilter, IOrderedFilter
{
    /// <summary>
    ///     Default Constructor
    /// </summary>
    public RemoveSystemTypesDocumentFilter()
    {
        Order = 1;
    }

    /// <summary>
    ///     Apply the filter to the operation.
    /// </summary>
    /// <param name="swaggerDoc"></param>
    /// <param name="context"></param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="NotSupportedException">The <see cref="IDictionary{TKey,TValue}" /> is read-only.</exception>
    /// <exception cref="ArgumentException">
    ///     <paramref>
    ///         <name>comparisonType</name>
    ///     </paramref>
    ///     is not a <see cref="StringComparison" /> value.
    /// </exception>
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
    /// </summary>
    public int Order { get; }
}