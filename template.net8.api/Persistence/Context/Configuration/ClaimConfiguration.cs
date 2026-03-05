using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using template.net8.api.Persistence.Models;

namespace template.net8.api.Persistence.Context.Configuration;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class ClaimConfiguration : IEntityTypeConfiguration<Claim>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public void Configure(EntityTypeBuilder<Claim> builder)
    {
        ConfigurePrimaryKeys(builder);
        ConfigureClaimRolesRelation(builder);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigurePrimaryKeys(EntityTypeBuilder<Claim> builder)
    {
        builder.HasKey(static e => e.Id).HasName("claim_pkey");
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureClaimRolesRelation(EntityTypeBuilder<Claim> builder)
    {
        builder.HasMany(static d => d.Roles).WithMany(static p => p.Claims)
            .UsingEntity<Dictionary<string, object>>(
                "ClaimRole", static r => r.HasOne<Role>().WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("claim_role_role_id_fkey"), static l => l.HasOne<Claim>().WithMany()
                    .HasForeignKey("ClaimId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("claim_role_claim_id_fkey"), static j =>
                {
                    j.HasKey("ClaimId", "RoleId").HasName("claim_role_pkey");
                    j.ToTable("claim_role", "identity");
                    j.IndexerProperty<short>("ClaimId").HasColumnName("claim_id");
                    j.IndexerProperty<short>("RoleId").HasColumnName("role_id");
                });
    }
}