using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using template.net8.api.Persistence.Models;

namespace template.net8.api.Persistence.Context.Configuration;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigurePrimaryKeys(builder);
        ConfigureProperties(builder);
        ConfigureInsertUserRelationship(builder);
        ConfigureRoleRelationship(builder);
        ConfigureUpdateUserRelationship(builder);
        ConfigureClaimRelationship(builder);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigurePrimaryKeys(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(static e => e.Id).HasName("user_pkey");
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureProperties(EntityTypeBuilder<User> builder)
    {
        builder.Property(static e => e.InsertDatetime).HasDefaultValueSql("(now() AT TIME ZONE 'UTC'::text)");
        builder.Property(static e => e.UpdateDatetime).HasDefaultValueSql("(now() AT TIME ZONE 'UTC'::text)");
        builder.Property(static e => e.Uuid).HasDefaultValueSql("gen_random_uuid()");
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureInsertUserRelationship(EntityTypeBuilder<User> builder)
    {
        builder.HasOne(static d => d.InsertUser).WithMany(static p => p.InverseInsertUser)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("user_insert_user_id_fkey");
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureRoleRelationship(EntityTypeBuilder<User> builder)
    {
        builder.HasOne(static d => d.Role).WithMany(static p => p.Users)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("user_role_id_fkey");
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureUpdateUserRelationship(EntityTypeBuilder<User> builder)
    {
        builder.HasOne(static d => d.UpdateUser).WithMany(static p => p.InverseUpdateUser)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("user_update_user_id_fkey");
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void ConfigureClaimRelationship(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(static d => d.Claims).WithMany(static p => p.Users)
            .UsingEntity<Dictionary<string, object>>(
                "ClaimUser", static r => r.HasOne<Claim>().WithMany()
                    .HasForeignKey("ClaimId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("claim_user_claim_id_fkey"), static l => l.HasOne<User>().WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("claim_user_user_id_fkey"), static j =>
                {
                    j.HasKey("UserId", "ClaimId").HasName("claim_user_pkey");
                    j.ToTable("claim_user", "identity");
                    j.IndexerProperty<short>("UserId").HasColumnName("user_id");
                    j.IndexerProperty<short>("ClaimId").HasColumnName("claim_id");
                });
    }
}