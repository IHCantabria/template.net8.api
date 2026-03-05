using EntityFramework.Exceptions.PostgreSQL;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Protocols.Configuration;
using Npgsql;
using template.net8.api.Core;
using template.net8.api.Core.Timeout;
using template.net8.api.Persistence.Context;
using template.net8.api.Settings.Interfaces;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class DbInstaller : IServiceInstaller
{
    /// <inheritdoc cref="IServiceInstaller.LoadOrder" />
    public short LoadOrder => 8;

    /// <inheritdoc cref="IServiceInstaller.InstallServiceAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidConfigurationException">
    ///     The App Db configuration in the appsettings file is incorrect.
    /// </exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Configure strongly typed options objects
        var connectionOptions = builder.Configuration
            .GetSection(AppDbOptions.AppDb)
            .Get<AppDbOptions>();

        OptionsValidator.ValidateAppDbOptions(connectionOptions);
        AddDbContextPool(builder, connectionOptions);
        return Task.CompletedTask;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void AddDbContextPool(IHostApplicationBuilder builder, AppDbOptions? connectionOptions)
    {
        // Register a pooling context factory as a Singleton service
        if (connectionOptions is null || string.IsNullOrEmpty(connectionOptions.ConnectionString)) return;

        builder.Services.AddPooledDbContextFactory<AppDbContext>(options =>
            ConfigureDbContext(options, builder, connectionOptions));
        // Register an additional context factory as a Transient service, which gets a pooled context from the Singleton factory we registered above
        builder.Services.AddTransient<AppDbContextFactory>();
        //Add a scoped service to create a new context instance, used for direct AppDBContext access
        builder.Services.AddScoped(static sp =>
            sp.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());
        if (builder.Environment.EnvironmentName is Envs.Development or Envs.Local or Envs.Test)
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureDbContext(DbContextOptionsBuilder options, IHostApplicationBuilder builder,
        AppDbOptions connectionOptions)
    {
        if (options.IsConfigured) return;

        ConfigureDevelopmentSettings(options, builder);
        ConfigureWarnings(options);
        ConfigureDatabaseProvider(options, builder, connectionOptions);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureDevelopmentSettings(DbContextOptionsBuilder options, IHostApplicationBuilder builder)
    {
        if (builder.Environment.EnvironmentName is not (Envs.Development or Envs.Local or Envs.Test)) return;

        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
        options.ConfigureWarnings(static w =>
            w.Ignore(CoreEventId.SensitiveDataLoggingEnabledWarning));
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureWarnings(DbContextOptionsBuilder options)
    {
        options.ConfigureWarnings(static w =>
            w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureDatabaseProvider(
        DbContextOptionsBuilder options,
        IHostApplicationBuilder builder,
        AppDbOptions connectionOptions)
    {
        options.UseNpgsql(
                GetNpgsqlDataSource(builder.Environment, connectionOptions), static npgsqlOptions =>
                {
                    npgsqlOptions.UseNetTopologySuite();
                    npgsqlOptions.CommandTimeout(DbContextConstants.CommandTimeout);
                    npgsqlOptions.EnableRetryOnFailure(
                        DbContextConstants.MaxRetryCount,
                        DbContextConstants.MaxRetryDelay,
                        []);
                })
            .UseExceptionProcessor();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [MustDisposeResource]
    private static NpgsqlDataSource GetNpgsqlDataSource(IHostEnvironment env,
        AppDbOptions connectionOptions)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionOptions.DecodedConnectionString);
        if (env.EnvironmentName is Envs.Development or Envs.Local or Envs.Test)
            dataSourceBuilder.EnableParameterLogging();
        dataSourceBuilder.UseNetTopologySuite();

        return dataSourceBuilder.Build();
    }
}