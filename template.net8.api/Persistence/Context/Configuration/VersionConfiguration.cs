using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Version = template.net8.api.Persistence.Models.Version;

namespace template.net8.api.Persistence.Context.Configuration;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class VersionConfiguration : IEntityTypeConfiguration<Version>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public void Configure(EntityTypeBuilder<Version> builder)
    {
        ConfigurePrimaryKeys(builder);
        ConfigureProperties(builder);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigurePrimaryKeys(EntityTypeBuilder<Version> builder)
    {
        builder.HasKey(static e => e.Id).HasName("version_pkey");
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureProperties(EntityTypeBuilder<Version> builder)
    {
        builder.Property(static e => e.Date).HasDefaultValueSql("(now() AT TIME ZONE 'UTC'::text)");
    }
}