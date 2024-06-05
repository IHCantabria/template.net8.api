using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using template.net8.Api.Core.Attributes;
using template.net8.Api.Settings.Interfaces;
using template.net8.Api.Settings.Options;

namespace template.net8.Api.Settings.ServiceInstallers;

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
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var config = builder.Configuration;
        // Configure strongly typed options objects
        var swaggerOptions = config.GetSection(SwaggerOptions.Swagger).Get<SwaggerOptions>();
        var version = config.Get<ProjectOptions>()?.Version ?? "";
        AddSwaggerGen(builder, swaggerOptions, version);
        return Task.CompletedTask;
    }

    private static void AddSwaggerGen(IHostApplicationBuilder builder, SwaggerOptions? swaggerOptions, string version)
    {
        if (swaggerOptions is null) return;
        // Register the swagger generator, defining 1 or more swagger documents
        builder.Services.AddSwaggerGen(c =>
        {
            AddSwaggerDoc(c, swaggerOptions, version);
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

    private static void AddSwaggeConfig(SwaggerGenOptions swagger, SwaggerOptions swaggerOptions)
    {
        //Add Server API url
        swagger.AddServer(new OpenApiServer
        {
            Url = swaggerOptions.ServerUrl.ToString()
        });
        // XML documentation
        var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        swagger.IncludeXmlComments(xmlPath);
        // Add swagger UI annotations
        swagger.EnableAnnotations();
    }
}