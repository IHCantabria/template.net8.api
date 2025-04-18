﻿using HealthChecks.ApplicationStatus.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Tokens;
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
    public short LoadOrder => 25;

    /// <summary>
    ///     Install Health Checks Service
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var connectionOptions = builder.Configuration
            .GetSection(ProjectDbOptions.ProjectDb)
            .Get<ProjectDbOptions>();
        ValidateProjectDbOptions(connectionOptions);
        AddHealthChecks(builder, connectionOptions);

        return Task.CompletedTask;
    }

    private static void ValidateProjectDbOptions(ProjectDbOptions? config)
    {
        var optionsValidator = new ProjectDbOptionsValidator();
        if (config is null)
            throw new InvalidConfigurationException(
                "The Project Db configuration in the appsettings file is incorrect");

        var validation = optionsValidator.Validate(null, config);
        if (validation.Failed)
            throw new InvalidConfigurationException(validation.FailureMessage);
    }

    private static void AddHealthChecks(WebApplicationBuilder builder, ProjectDbOptions? connectionOptions)
    {
        var healthChecksBuilder = builder.Services.AddHealthChecks();

        AddChecks(healthChecksBuilder, connectionOptions);

        //TODO: FIX, ONLY WORK IN LOCAL
        builder.Services.AddHealthChecksUI(opts =>
            {
                opts.SetEvaluationTimeInSeconds(120); //time in seconds between check    
                opts.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks    
                opts.SetApiMaxActiveRequests(1); //api requests concurrency    
                opts.AddHealthCheckEndpoint("Feedback Api", "/healthchecks"); //map health check api    
            })
            .AddInMemoryStorage();
    }

    private static void AddChecks(IHealthChecksBuilder builder, ProjectDbOptions? connectionOptions)
    {
        builder.AddApplicationStatus("API Status", tags: ApplicationTags);

        builder.AddCheck<MemoryHealthCheck>("Feedback Service Memory Check", HealthStatus.Unhealthy,
            ServiceMemoryTags);
        if (connectionOptions is not null && !connectionOptions.ConnectionString.IsNullOrEmpty())
            builder.AddNpgSql(connectionOptions.DecodedConnectionString, "select 1",
                name: "PostgreSql Server Project DB", failureStatus: HealthStatus.Unhealthy, tags: DbTags);
    }
}