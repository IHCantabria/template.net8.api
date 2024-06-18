using System.Globalization;
using System.Net.Mime;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using template.net8.api.Core.Contracts;

namespace template.net8.api.Settings.Filters;

/// <summary>
///     Documentation Operation Filter
/// </summary>
[UsedImplicitly]
public class DocumentationOperationFilter : IOperationFilter, IOrderedFilter
{
    private const string InternalServerErrorDescription =
        "Unable to execute the requested operation due to a server error. Please, try again after a couple of mins. if the error persist contact with IH-IT.";

    private const string RequestTimeoutErrorDescription =
        "Unable to execute the requested operation due to a request timeout issue, please retry the request.";

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
    /// <exception cref="ArgumentNullException">source is <see langword="null" />.</exception>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Add coomon response types.
        operation.Responses.TryAdd(StatusCodes.Status408RequestTimeout.ToString(CultureInfo.InvariantCulture),
            new OpenApiResponse
            {
                Description = RequestTimeoutErrorDescription,
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
                Description = InternalServerErrorDescription,
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
    /// </summary>
    public int Order { get; }
}