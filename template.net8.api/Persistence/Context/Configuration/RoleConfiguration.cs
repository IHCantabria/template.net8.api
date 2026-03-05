using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using template.net8.api.Persistence.Models;

namespace template.net8.api.Persistence.Context.Configuration;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        ConfigurePrimaryKeys(builder);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigurePrimaryKeys(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(static e => e.Id).HasName("role_pkey");
    }
}