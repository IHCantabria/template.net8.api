using System.Text.Json;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     SignalR Services Installer
/// </summary>
[CoreLibrary]
public sealed class SignalRInstaller : IServiceInstaller
{
    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 26;

    /// <summary>
    ///     Install SignalR Services
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
        builder.Services
            .AddSignalR(hubOptions => hubOptions.EnableDetailedErrors =
                builder.Environment.EnvironmentName is Envs.Development or Envs.Local).AddJsonProtocol(options =>
                options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
        return Task.CompletedTask;
    }
}