using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Extensions;
using template.net8.api.Core.Json;
using template.net8.api.Settings.ActionResult;
using template.net8.api.Settings.Attributes;
using template.net8.api.Settings.Filters;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Controllers Installer
/// </summary>
[CoreLibrary]
public sealed class ControllersInstaller : IServiceInstaller
{
    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 13;

    /// <summary>
    ///     Install Controller Services
    /// </summary>
    /// <param name="builder"></param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddControllers(x => ConfigureControllers(x, builder))
            .AddJsonOptions(ConfigureJsonOptions)
            .ConfigureApiBehaviorOptions(x =>
            {
                x.InvalidModelStateResponseFactory = _ => new ValidationProblemDetailsResult();
            });
        //TODO: fix Body params Json Serializer error with missing properties
        //TODO: Add OutputFormatter default with msg Header negotiation fail
        //.AddMvcOptions(options => options.OutputFormatters.Add(new FallBackOutputFormatter()));
        return Task.CompletedTask;
    }

    private static void ConfigureControllers(MvcOptions options, WebApplicationBuilder builder)
    {
        options.RespectBrowserAcceptHeader = true;
        options.ReturnHttpNotAcceptable = true;
        options.Filters.Add<RequestLogFilter>();
        options.Conventions.Add(new ActionHidingConvention(builder.Environment.EnvironmentName));
    }

    private static void ConfigureJsonOptions(JsonOptions options)
    {
        options.JsonSerializerOptions.AddCoreOptions();
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        options.JsonSerializerOptions.Converters.Add(new NamingPolicyConverter(new HttpContextAccessor()));
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }
}