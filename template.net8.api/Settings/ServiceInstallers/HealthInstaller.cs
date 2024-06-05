using HealthChecks.ApplicationStatus.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.HealthChecks;
using template.net8.api.Settings.Interfaces;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Health Checks Service Installer
/// </summary>
[CoreLibrary]
public sealed class HealthInstaller : IServiceInstaller
{
    private const string Feedback = "Feedback";

    private static readonly string[] ApplicationTags = [Feedback, "Api"];

    private static readonly string[] DbTags = [Feedback, "External", "Database"];

    private static readonly string[] ServiceMemoryTags = [Feedback, "Memory"];

    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 23;


    /// <summary>
    ///     Install Health Checks Service
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(builder);
        var connectionOptions = builder.Configuration
            .GetSection(ProjectDbOptions.ProjectDb)
            .Get<ProjectDbOptions>();
        AddHealthChecks(builder, connectionOptions);

        return Task.CompletedTask;
    }

    private static void AddHealthChecks(IHostApplicationBuilder builder, ProjectDbOptions? connectionOptions)
    {
        var healthChecksBuilder = builder.Services.AddHealthChecks()
            .AddApplicationStatus("API Status", tags: ApplicationTags)
            .AddCheck<MemoryHealthCheck>("Feedback Service Memory Check", HealthStatus.Unhealthy,
                ServiceMemoryTags);
        if (connectionOptions is not null)
            healthChecksBuilder.AddNpgSql(connectionOptions.DecodedConnectionString, "select 1",
                name: "PostgreSql Server CLIMPORT DB", failureStatus: HealthStatus.Unhealthy, tags: DbTags);

        //TODO: FIX, ONLY WORK IN LOCAL
        builder.Services.AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(120); //time in seconds between check    
                opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks    
                opt.SetApiMaxActiveRequests(1); //api requests concurrency    
                opt.AddHealthCheckEndpoint("Feedback Api", "/healthchecks"); //map health check api    
            })
            .AddInMemoryStorage();
    }
}