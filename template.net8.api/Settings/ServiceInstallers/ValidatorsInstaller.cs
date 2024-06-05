using FluentValidation;
using template.net8.Api.Core.Attributes;
using template.net8.Api.Settings.Interfaces;

namespace template.net8.Api.Settings.ServiceInstallers;

/// <summary>
///     Validators Services Installer
/// </summary>
[CoreLibrary]
public sealed class ValidatorsInstaller : IServiceInstaller
{
    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 13;

    /// <summary>
    ///     Install Validators Services
    /// </summary>
    /// <param name="builder"></param>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        return Task.CompletedTask;
    }
}