using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Serilog;
using template.net8.api.Settings.Extensions;
using template.net8.api.Settings.Interfaces;
using template.net8.api.Settings.Middlewares;
using template.net8.api.Settings.Options;

// ReSharper disable MethodTooLong (justification: The method is the main pipeline configuration for the application)

namespace template.net8.api.Settings.PipelineConfigurators;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class MainConfigurator : IPipelineConfigurator
{
    /// <inheritdoc cref="IPipelineConfigurator.LoadOrder" />
    public short LoadOrder => 1;


    /// <inheritdoc cref="IPipelineConfigurator.ConfigurePipelineAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="app" /> is <see langword="null" />.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public Task ConfigurePipelineAsync(WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        // Configure specific not Production behaviour exception page.
        if (app.Environment.EnvironmentName is Envs.Development or Envs.Local or Envs.Test)
            app.UseDeveloperExceptionPage();

        // Enable middleware to allow Error response outside the scope of the API.
        app.UseStatusCodePages();

        // Enable middleware to log the request ID.
        app.UseMiddleware<RequestIdLoggingMiddleware>();

        // Enable Middleware native
        app.UseHttpLogging();

        // Enable middleware custom to log requests.
        app.UseMiddleware<HttpRequestResponseLoggingMiddleware>();

        //Enable Serilog Request Logging.
        app.UseSerilogRequestLogging();

        // Enable middleware to collect telemetry data.
        app.UseMiddleware<TelemetryMiddleware>();

        // Enable middleware Block Http Request.
        app.UseMiddleware<HttpRejectMiddleware>();

        // Enable middleware to control exceptions.
        app.UseExceptionHandler();

        // Add the Request Localization middleware
        app.ConfigureLocalizationMiddleware();

        // Enable middleware to redirect http request to https for the swagger page.
        app.UseHttpsRedirection();

        //Configure enforce Https
        if (app.Environment.EnvironmentName is Envs.PreProduction or Envs.Production) app.UseHsts();

        //Host Filtering Middleware
        app.UseHostFiltering();

        //Enable static files middleware.
        app.UseStaticFiles();

        // Enable routing middleware.
        app.UseRouting();

        // Enable request timeout middleware.
        app.UseRequestTimeouts();

        //Get swagger configuration from service with strongly typed options object.
        var configuration = app.Services.GetRequiredService<IOptions<CorsOptions>>().Value;

        // Enable cors middleware to allow cross domain requests.
        app.UseCors(configuration.CorsPolicy);

        //Enable Authentication Header middleware.
        app.UseMiddleware<AuthenticationHeaderMiddleware>();

        //Enable authentication middleware.
        app.UseAuthentication();

        //Enable Authorization middleware.
        app.UseAuthorization();

        //Enable Security Headers middleware.
        app.UseMiddleware<SecurityHeadersMiddleware>();

        //Enable response compression middleware.
        app.UseResponseCompression();

        //Configure default controller endpoints (specific routes must be defined in the Controller classes).
        app.MapControllers();
        return Task.CompletedTask;
    }
}