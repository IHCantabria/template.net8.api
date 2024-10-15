using System.Text.Json;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Extensions;
using template.net8.api.Core.Json;
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
    /// <exception cref="InvalidOperationException">This property is set after serialization or deserialization has occurred.</exception>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
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