using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using template.net8.api.Persistence.Models;
using template.net8.api.Settings.Options;
using Version = template.net8.api.Persistence.Models.Version;

namespace template.net8.api.Persistence.Context;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[MustDisposeResource]
internal class AppDbContext(DbContextOptions<AppDbContext> options, IOptions<AppDbOptions> config)
    : DbContext(options)
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly AppDbOptions _config = config.Value ?? throw new ArgumentNullException(nameof(config));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public virtual DbSet<Claim> Claims { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public virtual DbSet<CurrentVersion> CurrentVersions { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public virtual DbSet<Role> Roles { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public virtual DbSet<User> Users { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public virtual DbSet<Version> Versions { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="modelBuilder" /> is <see langword="null" />.</exception>
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