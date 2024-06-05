using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Attributes;
using template.net8.api.Domain.Persistence.Context;
using template.net8.api.Settings.Interfaces;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Db Context Installer
/// </summary>
[CoreLibrary]
public sealed class DbInstaller : IServiceInstaller
{
    private const short MaxRetryCount = 5;
    private static readonly TimeSpan MaxRetryDelay = TimeSpan.FromSeconds(5);

    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 7;

    /// <summary>
    ///     Install Db Services
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Configure strongly typed options objects
        var connectionOptions = builder.Configuration
            .GetSection(ProjectDbOptions.ProjectDb)
            .Get<ProjectDbOptions>();
        AddDbContextPool(builder, connectionOptions);
        return Task.CompletedTask;
    }

    private static void AddDbContextPool(IHostApplicationBuilder builder, ProjectDbOptions? connectionOptions)
    {
        // Register a pooling context factory as a Singleton service
        builder.Services.AddPooledDbContextFactory<ProjectDbContext>(options =>
        {
            if (options.IsConfigured) return;
            if (builder.Environment.EnvironmentName is Envs.Development or Envs.Local)
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }

            if (connectionOptions is not null)
                options.UseNpgsql(connectionOptions.DecodedConnectionString,
                    x => { x.EnableRetryOnFailure(MaxRetryCount, MaxRetryDelay, new Collection<string>()); });
        });
        // Register an additional context factory as a Transient service, which gets a pooled context from the Singleton factory we registered above
        builder.Services.AddTransient<ProjectDbContextFactory>();
        //Add a scoped service to create a new context instance, used for direct ProjectDbContextFactory access
        builder.Services.AddScoped(sp =>
            sp.GetRequiredService<IDbContextFactory<ProjectDbContext>>().CreateDbContext());
        if (builder.Environment.EnvironmentName is Envs.Development or Envs.Local)
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    }
}