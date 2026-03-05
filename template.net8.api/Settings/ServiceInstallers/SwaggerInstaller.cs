using System.Reflection;
using JetBrains.Annotations;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using template.net8.api.Core;
using template.net8.api.Settings.Filters;
using template.net8.api.Settings.Interfaces;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class SwaggerInstaller : IServiceInstaller
{
    /// <inheritdoc cref="IServiceInstaller.LoadOrder" />
    public short LoadOrder => 17;

    /// <inheritdoc cref="IServiceInstaller.InstallServiceAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidConfigurationException">
    ///     The Swagger configuration in the appsettings file is incorrect.
    ///     The Swagger Security configuration in the appsettings file is incorrect.
    /// </exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var config = builder.Configuration;
        // Configure strongly typed options objects
        var swaggerOptions = config.GetSection(SwaggerOptions.Swagger).Get<SwaggerOptions>();
        OptionsValidator.ValidateSwaggerOptions(swaggerOptions);
        if (swaggerOptions is null)
            throw new InvalidConfigurationException(
                "The Swagger configuration in the appsettings file is incorrect.");
        // Configure strongly typed options objects
        var swaggerSecurityOptions =
            config.GetSection(SwaggerSecurityOptions.SwaggerSecurity).Get<SwaggerSecurityOptions>();
        OptionsValidator.ValidateSwaggerSecurityOptions(swaggerSecurityOptions);
        var version = config.Get<ProjectOptions>()?.Version ?? "";

        AddSwaggerGen(builder, swaggerOptions, swaggerSecurityOptions, version);
        return Task.CompletedTask;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void AddSwaggerGen(WebApplicationBuilder builder, SwaggerOptions swaggerOptions,
        SwaggerSecurityOptions? swaggerSecurityOptions, string version)
    {
        // Register the swagger generator, defining 1 or more swagger documents
        builder.Services.AddSwaggerGen(c => ConfigureSwaggerGen(c, swaggerOptions, swaggerSecurityOptions, version));
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureSwaggerGen(SwaggerGenOptions c, SwaggerOptions swaggerOptions,
        SwaggerSecurityOptions? swaggerSecurityOptions, string version)
    {
        AddSwaggerDoc(c, swaggerOptions, version);

        if (swaggerSecurityOptions is not null)
            AddSwaggerSecurity(c, swaggerSecurityOptions);
        c.OperationFilter<AuthOperationFilter>();
        c.CustomSchemaIds(static type => type.ToString()); // Avoid problems with nested models
        c.IgnoreObsoleteActions(); // Ignoring obsolete methods to improve performance
        c.IgnoreObsoleteProperties();
        c.OperationFilter<DocumentationOperationFilter>();
        c.UseInlineDefinitionsForEnums();
        c.DocumentFilter<RemoveSystemTypesDocumentFilter>();
        AddSwaggeConfig(c, swaggerOptions);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void AddSwaggerDoc(SwaggerGenOptions swagger, SwaggerOptions swaggerOptions, string version)
    {
        swagger.SwaggerDoc(swaggerOptions.VersionSwagger, new OpenApiInfo
        {
            Title = swaggerOptions.Title,
            Version = version,
            Description = swaggerOptions.LongDescription,
            License = new OpenApiLicense
            {
                Name = swaggerOptions.License
            }
        });
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void AddSwaggerSecurity(SwaggerGenOptions swagger, SwaggerSecurityOptions swaggerSecurityOptions)
    {
        var securityScheme = new OpenApiSecurityScheme
        {
            Description = swaggerSecurityOptions.Description,
            Name = swaggerSecurityOptions.Name,
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = swaggerSecurityOptions.SchemeName,
            BearerFormat = swaggerSecurityOptions.BearerFormat,
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = swaggerSecurityOptions.SchemeId
            }
        };
        swagger.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void AddSwaggeConfig(SwaggerGenOptions swagger, SwaggerOptions swaggerOptions)
    {
        //Add Server API url
        swagger.AddServer(new OpenApiServer
        {
            Url = swaggerOptions.ServerUrl.ToString()
        });
        // XML documentation
        swagger.IncludeXmlComments(Assembly.GetExecutingAssembly());
        // Add swagger UI annotations
        swagger.EnableAnnotations();
    }
}