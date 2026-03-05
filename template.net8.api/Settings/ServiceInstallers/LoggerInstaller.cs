using System.Text.Json;
using JetBrains.Annotations;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Protocols.Configuration;
using Serilog;
using template.net8.api.Core;
using template.net8.api.Core.Extensions;
using template.net8.api.Core.Logger;
using template.net8.api.Settings.Interfaces;
using Path = System.IO.Path;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class LoggerInstaller : IServiceInstaller
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions().AddCoreOptions();

    /// <inheritdoc cref="IServiceInstaller.LoadOrder" />
    public short LoadOrder => 1;

    /// <inheritdoc cref="IServiceInstaller.InstallServiceAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidConfigurationException">Condition.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Given depth must be positive.</exception>
    /// <exception cref="InvalidOperationException">When the logger is already created</exception>
    public async Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var config = builder.Configuration;
        //Remove microsoft logger instances
        builder.Logging.ClearProviders();
        //Define log min level, this is the fallback value if this value is not defined in the appsettings file.

        builder.Logging.SetMinimumLevel(LogLevel.Trace);
        //Define Serilog like default logger.

        builder.Services.AddSerilog();

        var version = await ReadPackageJsonVersionAsync().ConfigureAwait(false);

        SerilogLoggersFactory.RealApplicationLogFactory(config, builder.Environment.EnvironmentName, version);

        builder.Services.AddHttpLogging(static options =>
            options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders |
                                    HttpLoggingFields.ResponsePropertiesAndHeaders);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static async Task<string> ReadPackageJsonVersionAsync()
    {
        var ct = CancellationToken.None;
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), CoreConstants.PackageJsonFile);
        if (!File.Exists(filePath)) return string.Empty;

        using var reader = new StreamReader(filePath);
        var jsonContent = await reader.ReadToEndAsync(ct).ConfigureAwait(false);

        var jsonObject = JsonSerializer.Deserialize<JsonElement>(jsonContent, Options);

        return (jsonObject.TryGetProperty("version", out var version) ? version.GetString() : string.Empty) ??
               string.Empty;
    }
}