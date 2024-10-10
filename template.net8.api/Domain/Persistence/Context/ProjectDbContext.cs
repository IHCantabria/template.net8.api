using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using template.net8.api.Settings.Options;

namespace template.net8.api.Domain.Persistence.Context;

/// <summary>
///     ProjectDbContext class to hold the Db Context
/// </summary>
[MustDisposeResource]
public class ProjectDbContext(DbContextOptions<ProjectDbContext> options, IOptions<ProjectDbOptions> config)
    : DbContext(options)
{
    private readonly ProjectDbOptions _config = config.Value ?? throw new ArgumentNullException(nameof(config));

    /// <summary>
    ///     OnModelCreating method to configure the models
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);

        if (_config.Schema is null) return;
        //Must be Serial
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.RemoveAnnotation("Relational:Schema");

            entity.AddAnnotation("Relational:Schema", _config.Schema);
        }
    }
}