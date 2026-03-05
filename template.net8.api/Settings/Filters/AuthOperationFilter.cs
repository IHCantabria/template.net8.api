using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.Mime;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using template.net8.api.Controllers;
using template.net8.api.Core.Contracts;
using template.net8.api.Settings.Options;
using ZLinq;

namespace template.net8.api.Settings.Filters;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class AuthOperationFilter : IOperationFilter, IOrderedFilter
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly SwaggerSecurityOptions _config;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    public AuthOperationFilter(IOptions<SwaggerSecurityOptions> config)
    {
        Order = 2;
        _config = config.Value ?? throw new ArgumentNullException(nameof(config));
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="operation" /> is <see langword="null" />.
    ///     <paramref name="context" /> is <see langword="null" />.
    /// </exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
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

        var attr = attributes?[0];

        // Add what should be show inside the security section
        IList<string> securityInfos =
        [
            $"{nameof(AuthorizeAttribute.Policy)}:{attr?.Policy}", $"{nameof(AuthorizeAttribute.Roles)}:{attr?.Roles}",
            $"{nameof(AuthorizeAttribute.AuthenticationSchemes)}:{attr?.AuthenticationSchemes}"
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
                            Id = _config
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
    ///     ADD DOCUMENTATION
    /// </summary>
    public int Order { get; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static List<AuthorizeAttribute> GetAttributes(OperationFilterContext context)
    {
        // Get Authorize attribute
        var declaringTypeAttributes = context.MethodInfo.DeclaringType?.GetCustomAttributes(true);
        var methodInfoAttributes = context.MethodInfo.GetCustomAttributes(true);
        return declaringTypeAttributes is not null
            ? methodInfoAttributes.Union(declaringTypeAttributes).OfType<AuthorizeAttribute>().ToList()
            : methodInfoAttributes.OfType<AuthorizeAttribute>().ToList();
    }
}