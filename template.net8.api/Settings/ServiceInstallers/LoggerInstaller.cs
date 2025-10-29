using System.Text.Json;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Protocols.Configuration;
using Serilog;
using template.net8.api.Core;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Extensions;
using template.net8.api.Core.Logger;
using template.net8.api.Settings.Interfaces;
using Path = System.IO.Path;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Logger Service Installer
/// </summary>
[CoreLibrary]
public sealed class LoggerInstaller : IServiceInstaller
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions().AddCoreOptions();

    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 1;

    /// <summary>
    ///     Install Logger Service
    /// </summary>
    /// <param name="builder"></param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidConfigurationException">
    ///     The OpenTelemetry configuration in the appsettings file is incorrect.
    ///     There was a problem trying to connecte to the OpenTelemetry endpoint
    /// </exception>
    /// <exception cref="IOException">Condition.</exception>
    /// <exception cref="InvalidOperationException">Condition.</exception>
    /// <exception cref="NotSupportedException">Condition.</exception>
    /// <exception cref="PathTooLongException">
    ///     When
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    ///     is too long
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    ///     The caller does not have the required permission to access the
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid
    ///     <paramref>
    ///         <name>path</name>
    ///     </paramref>
    /// </exception>
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

        builder.Services.AddHttpLogging(options =>
            options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders |
                                    HttpLoggingFields.ResponsePropertiesAndHeaders);
    }

    private static async Task<string> ReadPackageJsonVersionAsync()
    {
        var ct = CancellationToken.None;
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), CoreConstants.PackageJsonFile);
        if (!File.Exists(filePath)) return string.Empty;

        using var reader = new StreamReader(filePath);
        var jsonContent = await reader.ReadToEndAsync(ct).ConfigureAwait(false);

        var jsonObject = JsonSerializer.Deserialize<JsonElement>(jsonContent, Options);

        return jsonObject.TryGetProperty("version", out var version) ? version.GetString()! : string.Empty;
    }
}