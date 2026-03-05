using System.Globalization;
using System.Net.Mime;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using template.net8.api.Controllers;
using template.net8.api.Core.Contracts;

namespace template.net8.api.Settings.Filters;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class DocumentationOperationFilter : IOperationFilter, IOrderedFilter
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public DocumentationOperationFilter()
    {
        Order = 1;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="operation" /> is <see langword="null" />.
    ///     <paramref name="context" /> is <see langword="null" />.
    /// </exception>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Add common response types.
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(context);
        operation.Responses.TryAdd(StatusCodes.Status408RequestTimeout.ToString(CultureInfo.InvariantCulture),
            new OpenApiResponse
            {
                Description = SwaggerDocumentation.Filter.RequestTimeoutErrorDescription,
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    [MediaTypeNames.Application.ProblemJson] = new()
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(RequestTimeoutProblemDetailsResource),
                            context.SchemaRepository)
                    }
                }
            });
        operation.Responses.TryAdd(StatusCodes.Status500InternalServerError.ToString(CultureInfo.InvariantCulture),
            new OpenApiResponse
            {
                Description = SwaggerDocumentation.Filter.InternalServerErrorDescription,
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    [MediaTypeNames.Application.ProblemJson] = new()
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(InternalServerProblemDetailsResource),
                            context.SchemaRepository)
                    }
                }
            });
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public int Order { get; }
}