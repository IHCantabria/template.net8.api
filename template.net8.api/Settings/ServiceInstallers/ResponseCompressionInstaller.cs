using System.IO.Compression;
using System.Net.Mime;
using Microsoft.AspNetCore.ResponseCompression;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Response Compression Service Installer
/// </summary>
[CoreLibrary]
public sealed class ResponseCompressionInstaller : IServiceInstaller
{
    private static readonly string[] MimeTypes =
    [
        MediaTypeNames.Application.Json,
        "application/json; charset=utf-8",
        "application/json; charset=utf-16",
        "application/json; charset=utf-16le",
        "application/json; charset=utf-16be",
        MediaTypeNames.Application.ProblemJson,
        "application/problem+json; charset=utf-8",
        "application/problem+json; charset=utf-16",
        "application/problem+json; charset=utf-16le",
        "application/problem+json; charset=utf-16be",
        MediaTypeNames.Application.ProblemXml,
        "application/problem+xml; charset=utf-8",
        "application/problem+xml; charset=utf-16",
        "application/problem+xml; charset=utf-16le",
        "application/problem+xml; charset=utf-16be",
        MediaTypeNames.Application.Xml,
        "application/xml; charset=utf-8",
        "application/xml; charset=utf-16",
        "application/xml; charset=utf-16le",
        "application/xml; charset=utf-16be",
        MediaTypeNames.Text.Plain,
        "text/json; charset=utf-8",
        "text/json; charset=utf-16",
        "text/json; charset=utf-16le",
        "text/json; charset=utf-16be",
        MediaTypeNames.Text.Xml,
        "text/xml; charset=utf-8",
        "text/xml; charset=utf-16",
        "text/xml; charset=utf-16le",
        "text/xml; charset=utf-16be"
    ];

    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 18;

    /// <summary>
    ///     Install Compression Respònse Service
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
        // Install response compression middleware services.
        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(MimeTypes);
        });

        // Configure compression providers
        builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });

        builder.Services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.SmallestSize;
        });
        return Task.CompletedTask;
    }
}