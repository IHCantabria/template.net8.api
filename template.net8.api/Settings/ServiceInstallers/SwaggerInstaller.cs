using System.Reflection;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Filters;
using template.net8.api.Settings.Interfaces;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Swagger Service Installer
/// </summary>
[CoreLibrary]
public sealed class SwaggerInstaller : IServiceInstaller
{
    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 17;

    /// <summary>
    ///     Install Swagger Service
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var config = builder.Configuration;
        // Configure strongly typed options objects
        var swaggerOptions = config.GetSection(SwaggerOptions.Swagger).Get<SwaggerOptions>();
        ValidateSwaggerOptions(swaggerOptions);
        // Configure strongly typed options objects
        var swaggerSecurityOptions =
            config.GetSection(SwaggerSecurityOptions.SwaggerSecurity).Get<SwaggerSecurityOptions>();
        ValidateSwaggerSecurityOptions(swaggerSecurityOptions);
        var version = config.Get<ProjectOptions>()?.Version ?? "";
        AddSwaggerGen(builder, swaggerOptions, swaggerSecurityOptions, version);
        return Task.CompletedTask;
    }

    private static void ValidateSwaggerOptions(SwaggerOptions? config)
    {
        var optionsValidator = new SwaggerOptionsValidator();
        if (config is null)
            throw new InvalidConfigurationException(
                "The Swagger configuration in the appsettings file is incorrect");

        var validation = optionsValidator.Validate(null, config);
        if (validation.Failed)
            throw new InvalidConfigurationException(validation.FailureMessage);
    }

    private static void ValidateSwaggerSecurityOptions(SwaggerSecurityOptions? config)
    {
        var optionsValidator = new SwaggerSecurityOptionsValidator();
        if (config is null)
            throw new InvalidConfigurationException(
                "The Swagger Security configuration in the appsettings file is incorrect");

        var validation = optionsValidator.Validate(null, config);
        if (validation.Failed)
            throw new InvalidConfigurationException(validation.FailureMessage);
    }

    private static void AddSwaggerGen(WebApplicationBuilder builder, SwaggerOptions? swaggerOptions,
        SwaggerSecurityOptions? swaggerSecurityOptions, string version)
    {
        if (swaggerOptions is null) return;
        // Register the swagger generator, defining 1 or more swagger documents
        builder.Services.AddSwaggerGen(c =>
        {
            ConfigureSwaggerGen(c, swaggerOptions, swaggerSecurityOptions, version);
        });
    }

    private static void ConfigureSwaggerGen(SwaggerGenOptions c, SwaggerOptions swaggerOptions,
        SwaggerSecurityOptions? swaggerSecurityOptions, string version)
    {
        AddSwaggerDoc(c, swaggerOptions, version);

        //Commented because it is not used in this project template
        //if (swaggerSecurityOptions is not null)
        //    AddSwaggerSecurity(c, swaggerSecurityOptions);
        //c.OperationFilter<AuthOperationFilter>();
        c.CustomSchemaIds(type => type.ToString()); // Avoid problems with nested models
        c.IgnoreObsoleteActions(); // Ignoring obsolete methods to improve performance
        c.IgnoreObsoleteProperties();
        c.OperationFilter<DocumentationOperationFilter>();
        c.UseInlineDefinitionsForEnums();
        c.DocumentFilter<RemoveSystemTypesDocumentFilter>();
        AddSwaggeConfig(c, swaggerOptions);
    }

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