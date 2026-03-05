using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using template.net8.api.Persistence.Models.Interfaces;

namespace template.net8.api.Persistence.Models;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[Table("role", Schema = "identity")]
[Microsoft.EntityFrameworkCore.Index("Name", Name = "rol_key", IsUnique = true)]
internal class Role : IEntityWithNameKey, IEntityWithAlias, IEntityWithId<short>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [InverseProperty("Role")]
    public virtual ICollection<User> Users { get; } = [];

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [ForeignKey("RoleId")]
    [InverseProperty("Roles")]
    public virtual ICollection<Claim> Claims { get; } = [];

    /// <inheritdoc cref="IEntityWithAlias.AliasText" />
    [Column("alias")]
    [MaxLength(100)]
    public required string AliasText { get; set; }

    /// <inheritdoc cref="IEntityWithId{short}.Id" />
    [Key]
    [Column("id")]
    public required short Id { get; init; }

    /// <inheritdoc cref="IEntityWithNameKey.Name" />
    [Column("name")]
    [MaxLength(100)]
    public required string Name { get; init; }
}