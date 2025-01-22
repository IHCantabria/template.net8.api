using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Timeout;
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
    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 8;

    /// <summary>
    ///     Install Db Services
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
        if (connectionOptions is null) return;

        builder.Services.AddPooledDbContextFactory<ProjectDbContext>(options =>
        {
            ConfigureDbContext(options, builder, connectionOptions);
        });
        // Register an additional context factory as a Transient service, which gets a pooled context from the Singleton factory we registered above
        builder.Services.AddTransient<ProjectDbContextFactory>();
        //Add a scoped service to create a new context instance, used for direct ProjectDBContext access
        builder.Services.AddScoped(sp =>
            sp.GetRequiredService<IDbContextFactory<ProjectDbContext>>().CreateDbContext());
        if (builder.Environment.EnvironmentName is Envs.Development or Envs.Local or Envs.Test)
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    }

    private static void ConfigureDbContext(DbContextOptionsBuilder options, IHostApplicationBuilder builder,
        ProjectDbOptions connectionOptions)
    {
        if (options.IsConfigured) return;

        if (builder.Environment.EnvironmentName is Envs.Development or Envs.Local or Envs.Test)
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }

        options.UseNpgsql(GetNpgsqlDataSource(builder.Environment, connectionOptions),
            x =>
            {
                x.UseNetTopologySuite();
                x.CommandTimeout(DbContextConstants.CommandTimeout);
                x.EnableRetryOnFailure(DbContextConstants.MaxRetryCount, DbContextConstants.MaxRetryDelay, []);
            });
    }

    [MustDisposeResource]
    private static NpgsqlDataSource GetNpgsqlDataSource(IHostEnvironment env,
        ProjectDbOptions connectionOptions)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionOptions.DecodedConnectionString);
        if (env.EnvironmentName is Envs.Development or Envs.Local or Envs.Test)
            dataSourceBuilder.EnableParameterLogging();
        dataSourceBuilder.UseNetTopologySuite();

        return dataSourceBuilder.Build();
    }
}