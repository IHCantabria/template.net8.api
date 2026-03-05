using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using template.net8.api.Core.Extensions;
using template.net8.api.Core.Factory;
using template.net8.api.Core.Json;
using template.net8.api.Localize.Resources;
using template.net8.api.Settings.Attributes;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class ControllersInstaller : IServiceInstaller
{
    /// <inheritdoc cref="IServiceInstaller.LoadOrder" />
    public short LoadOrder => 12;

    /// <inheritdoc cref="IServiceInstaller.InstallServiceAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddControllers(x => ConfigureControllers(x, builder))
            .AddJsonOptions(ConfigureJsonOptions)
            .ConfigureApiBehaviorOptions(static x =>
            {
                x.InvalidModelStateResponseFactory = static context =>
                {
                    var localizer = context.HttpContext.RequestServices
                        .GetRequiredService<IStringLocalizer<ResourceMain>>();
                    var problemDetails =
                        ProblemDetailsFactoryCore.CreateProblemDetailsBadRequestValidationPayload(context.ModelState,
                            localizer);
                    return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
                };
            });
        return Task.CompletedTask;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureControllers(MvcOptions options, WebApplicationBuilder builder)
    {
        options.RespectBrowserAcceptHeader = true;
        options.ReturnHttpNotAcceptable = true;
        options.Conventions.Add(new ActionHidingConvention(builder.Environment.EnvironmentName));
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureJsonOptions(JsonOptions options)
    {
        options.JsonSerializerOptions.AddCoreOptions();
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        options.JsonSerializerOptions.Converters.Add(new NamingPolicyConverter(new HttpContextAccessor()));
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }
}