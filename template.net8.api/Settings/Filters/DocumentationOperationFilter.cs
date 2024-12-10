using System.Globalization;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using template.net8.api.Controllers;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Contracts;

namespace template.net8.api.Settings.Filters;

/// <summary>
///     Documentation Operation Filter
/// </summary>
[CoreLibrary]
public sealed class DocumentationOperationFilter : IOperationFilter, IOrderedFilter
{
    /// <summary>
    ///     Default Constructor
    /// </summary>
    public DocumentationOperationFilter()
    {
        Order = 1;
    }

    /// <summary>
    ///     Apply the filter to the operation.
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
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
    ///     Order of the filter
    /// </summary>
    public int Order { get; }
}