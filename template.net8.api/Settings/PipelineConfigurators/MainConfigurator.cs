using System.Globalization;
using Microsoft.Extensions.Options;
using Serilog;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Extensions;
using template.net8.api.Settings.Interfaces;
using template.net8.api.Settings.Middlewares;
using template.net8.api.Settings.Options;

// ReSharper disable MethodTooLong (justification: The method is the main pipeline configuration for the application)

namespace template.net8.api.Settings.PipelineConfigurators;

/// <summary>
///     Main Configurator
/// </summary>
[CoreLibrary]
public sealed class MainConfigurator : IPipelineConfigurator
{
    /// <summary>
    ///     Load order of the pipeline configurator
    /// </summary>
    public short LoadOrder => 1;

    /// <summary>
    ///     Configure the main Pipeline for the application.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
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
    /// <exception cref="CultureNotFoundException">
    ///     <paramref>
    ///         <name>name</name>
    ///     </paramref>
    ///     is not a valid culture name. For more information,
    ///     see the Notes to Callers section.
    /// </exception>
    public Task ConfigurePipelineAsync(WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        // Configure specific not Production behaviour exception page.
        if (app.Environment.EnvironmentName is Envs.Development or Envs.Local or Envs.Test)
            app.UseDeveloperExceptionPage();

        // Enable middleware to log the request ID.
        app.UseMiddleware<RequestIdLoggingMiddleware>();

        // Enable middleware to log requests.
        app.UseMiddleware<RequestLoggingMiddleware>();

        //Enable Serilog Request Logging.
        app.UseSerilogRequestLogging();

        // Enable middleware Block Http Request.
        app.UseMiddleware<HttpRejectMiddleware>();

        // Enable middleware to control exceptions.
        app.UseExceptionHandler();

        // Add the Request Localization middleware
        app.ConfigureLocalizationMiddleware();

        // Enable middleware to allow Error response outside the scope of the API.
        app.UseStatusCodePages();

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
        var configuration = app.Services.GetRequiredService<IOptions<ApiOptions>>().Value;

        // Enable cors middleware to allow cross domain requests.
        app.UseCors(configuration.CorsPolicy);

        //Enable Authentication Header middleware. Commented out because it is not used in this template.
        //app.UseMiddleware<AuthenticationHeaderMiddleware>();

        //Enable authentication middleware. Commented out because it is not used in this template.
        //app.UseAuthentication();

        //Enable Authorization middleware. Commented out because it is not used in this template.
        //app.UseAuthorization();

        //Enable Security Headers middleware.
        app.UseMiddleware<SecurityHeadersMiddleware>();

        //Enable response compression middleware.
        app.UseResponseCompression();

        //Configure default controller endpoints (specific routes must be defined in the Controller classes).
        app.MapControllers();
        return Task.CompletedTask;
    }
}