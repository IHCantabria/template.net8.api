using System.Globalization;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using template.net8.api.Controllers;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Contracts;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.Filters;

/// <summary>
///     Auth Operation Filter
/// </summary>
[CoreLibrary]
public class AuthOperationFilter : IOperationFilter, IOrderedFilter
{
    private readonly IOptions<SwaggerSecurityOptions> _config;

    /// <summary>
    ///     Default Constructor
    /// </summary>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    public AuthOperationFilter(IOptions<SwaggerSecurityOptions> config)
    {
        Order = 2;
        _config = config ?? throw new ArgumentNullException(nameof(config));
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
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Add common response types.
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(context);

        var attributes = GetAttributes(context);
        var isAuthorized = attributes?.Count > 0;
        if (!isAuthorized)
        {
            operation.Security.Clear();
            return;
        }

        var attr = attributes![0];

        // Add what should be show inside the security section
        IList<string> securityInfos =
        [
            $"{nameof(AuthorizeAttribute.Policy)}:{attr.Policy}", $"{nameof(AuthorizeAttribute.Roles)}:{attr.Roles}",
            $"{nameof(AuthorizeAttribute.AuthenticationSchemes)}:{attr.AuthenticationSchemes}"
        ];

        // Add common security response types.
        operation.Responses.TryAdd(StatusCodes.Status401Unauthorized.ToString(CultureInfo.InvariantCulture),
            new OpenApiResponse
            {
                Description = SwaggerDocumentation.Filter.AuthorizationErrorDescription,
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    [MediaTypeNames.Application.ProblemJson] = new()
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(UnauthorizedProblemDetailsResource),
                            context.SchemaRepository)
                    }
                }
            });
        operation.Responses.TryAdd(StatusCodes.Status403Forbidden.ToString(CultureInfo.InvariantCulture),
            new OpenApiResponse
            {
                Description = SwaggerDocumentation.Filter.ForbiddenErrorDescription,
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    [MediaTypeNames.Application.ProblemJson] = new()
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(ForbiddenProblemDetailsResource),
                            context.SchemaRepository)
                    }
                }
            });

        operation.Security =
        [
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = _config.Value
                                .SchemeId, // Must fit the defined SchemeId of SecurityDefinition in global configuration
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    securityInfos
                }
            }
        ];
    }

    /// <summary>
    /// </summary>
    public int Order { get; }

    private static List<AuthorizeAttribute> GetAttributes(OperationFilterContext context)
    {
        // Get Authorize attribute
        var declaringTypeAttributes = context.MethodInfo.DeclaringType?.GetCustomAttributes(true);
        var methodInfoAttributes = context.MethodInfo.GetCustomAttributes(true);
        var attributes = declaringTypeAttributes is not null
            ? methodInfoAttributes.Union(declaringTypeAttributes).OfType<AuthorizeAttribute>().ToList()
            : methodInfoAttributes.OfType<AuthorizeAttribute>().ToList();
        return attributes;
    }
}