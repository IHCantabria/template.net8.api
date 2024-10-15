using System.Reflection;
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
    public short LoadOrder => 18;

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
        // Configure strongly typed options objects
        var swaggerSecurityOptions =
            config.GetSection(SwaggerSecurityOptions.SwaggerSecurity).Get<SwaggerSecurityOptions>();
        var version = config.Get<ProjectOptions>()?.Version ?? "";
        AddSwaggerGen(builder, swaggerOptions, swaggerSecurityOptions, version);
        return Task.CompletedTask;
    }

    private static void AddSwaggerGen(IHostApplicationBuilder builder, SwaggerOptions? swaggerOptions,
        SwaggerSecurityOptions? swaggerSecurityOptions, string version)
    {
        if (swaggerOptions is null) return;
        // Register the swagger generator, defining 1 or more swagger documents
        builder.Services.AddSwaggerGen(c =>
        {
            AddSwaggerDoc(c, swaggerOptions, version);
            //Commented because it is not used in this project template
            //if (swaggerSecurityOptions is not null)
            //    AddSwaggerSecurity(c, swaggerSecurityOptions);
            //c.OperationFilter<AuthOperationFilter>();
            c.OperationFilter<DocumentationOperationFilter>();
            AddSwaggeConfig(c, swaggerOptions);
        });
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