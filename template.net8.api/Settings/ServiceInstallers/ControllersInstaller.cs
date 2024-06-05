using System.Text.Json;
using template.net8.Api.Core.Attributes;
using template.net8.Api.Core.Extensions;
using template.net8.Api.Core.Json;
using template.net8.Api.Settings.Interfaces;

namespace template.net8.Api.Settings.ServiceInstallers;

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
    /// <exception cref="InvalidOperationException">This property is set after serialization or deserialization has occurred.</exception>
    /// <exception cref="NotSupportedException">The <see /> is read-only.</exception>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = true;
            })
            .AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.AddCoreOptions();
                x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                x.JsonSerializerOptions.Converters.Add(new NamingPolicyConverter(new HttpContextAccessor()));
            });
        //TODO: fix Body params Json Serializer error with missing properties
        //TODO: Add OutputFormatter default with msg Header negotiation fail
        //.AddMvcOptions(options => options.OutputFormatters.Add(new FallBackOutputFormatter()));
        return Task.CompletedTask;
    }
}