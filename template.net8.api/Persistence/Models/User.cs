using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using template.net8.api.Persistence.Models.Interfaces;

namespace template.net8.api.Persistence.Models;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[Table("user", Schema = "identity")]
[Microsoft.EntityFrameworkCore.Index("Email", Name = "user_email_key", IsUnique = true)]
[Microsoft.EntityFrameworkCore.Index("Uuid", Name = "user_uuid_key", IsUnique = true)]
internal partial class User : IEntityWithUuid, IEntityWithId<short>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Column("username")]
    [MaxLength(100)]
    public required string Username { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Column("email")]
    [MaxLength(100)]
    public required string Email { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Column("password_hash")]
    [MaxLength(128)]
    public required string PasswordHash { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Column("password_salt")]
    [MaxLength(128)]
    public required string PasswordSalt { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Column("role_id")]
    public required short? RoleId { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Column("first_name")]
    [MaxLength(100)]
    public required string? FirstName { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Column("last_name")]
    [MaxLength(100)]
    public required string? LastName { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Column("insert_datetime", TypeName = "timestamp without time zone")]
    public required DateTime? InsertDatetime { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Column("is_disabled")]
    public required bool IsDisabled { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Column("insert_user_id")]
    public required short? InsertUserId { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Column("update_datetime", TypeName = "timestamp without time zone")]
    public required DateTime UpdateDatetime { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Column("update_user_id")]
    public required short? UpdateUserId { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [ForeignKey("InsertUserId")]
    [InverseProperty("InverseInsertUser")]
    public virtual User? InsertUser { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [InverseProperty("InsertUser")]
    public virtual ICollection<User> InverseInsertUser { get; } = [];

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [InverseProperty("UpdateUser")]
    public virtual ICollection<User> InverseUpdateUser { get; } = [];

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role? Role { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [ForeignKey("UpdateUserId")]
    [InverseProperty("InverseUpdateUser")]
    public virtual User? UpdateUser { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<Claim> Claims { get; } = [];

    /// <inheritdoc cref="IEntityWithId{short}.Id" />
    [Key]
    [Column("id")]
    public required short Id { get; init; }

    /// <inheritdoc cref="IEntityWithUuid.Uuid" />
    [Column("uuid")]
    public required Guid Uuid { get; init; }
}