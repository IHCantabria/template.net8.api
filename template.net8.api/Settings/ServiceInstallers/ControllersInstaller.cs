using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Extensions;
using template.net8.api.Core.Factory;
using template.net8.api.Core.Json;
using template.net8.api.Localize.Resources;
using template.net8.api.Settings.Attributes;
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
    public short LoadOrder => 12;

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
    /// <exception cref="InvalidOperationException">
    ///     There is no service of type
    ///     <typeparamref>
    ///         <name>T</name>
    ///     </typeparamref>
    ///     .
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <paramref>
    ///         <name>keySelector</name>
    ///     </paramref>
    ///     produces duplicate keys for two elements.
    /// </exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddControllers(x => ConfigureControllers(x, builder))
            .AddJsonOptions(ConfigureJsonOptions)
            .ConfigureApiBehaviorOptions(x =>
            {
                x.InvalidModelStateResponseFactory = context =>
                {
                    var localizer = context.HttpContext.RequestServices
                        .GetRequiredService<IStringLocalizer<ResourceMain>>();
                    var problemDetails =
                        ProblemDetailsFactoryCore.CreateProblemDetailsBadRequestValidationPayload(context.ModelState,
                            localizer);
                    return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
                };
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