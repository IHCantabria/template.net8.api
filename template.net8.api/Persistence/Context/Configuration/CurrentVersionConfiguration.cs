using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using template.net8.api.Persistence.Models;

namespace template.net8.api.Persistence.Context.Configuration;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class CurrentVersionConfiguration : IEntityTypeConfiguration<CurrentVersion>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public void Configure(EntityTypeBuilder<CurrentVersion> builder)
    {
        ConfigurePrimaryKeys(builder);
        ConfigureVersionRelation(builder);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigurePrimaryKeys(EntityTypeBuilder<CurrentVersion> builder)
    {
        builder.HasKey(static e => e.VersionId).HasName("current_version_pkey");

        builder.Property(static e => e.VersionId).ValueGeneratedNever();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureVersionRelation(EntityTypeBuilder<CurrentVersion> builder)
    {
        builder.HasOne(static d => d.Version).WithOne(static p => p.CurrentVersion)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("current_version_version_id_fkey");
    }
}