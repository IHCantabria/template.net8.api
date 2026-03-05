using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using template.net8.api.Persistence.Models.Interfaces;

namespace template.net8.api.Persistence.Models;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[Table("claim", Schema = "identity")]
[Microsoft.EntityFrameworkCore.Index("Name", Name = "claim_key", IsUnique = true)]
internal class Claim : IEntityWithNameKey, IEntityWithId<short>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [ForeignKey("ClaimId")]
    [InverseProperty("Claims")]
    public virtual ICollection<Role> Roles { get; } = [];

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [ForeignKey("ClaimId")]
    [InverseProperty("Claims")]
    public virtual ICollection<User> Users { get; } = [];

    /// <inheritdoc cref="IEntityWithId{short}.Id" />
    [Key]
    [Column("id")]
    public required short Id { get; init; }

    /// <inheritdoc cref="IEntityWithNameKey.Name" />
    [Column("name")]
    [MaxLength(100)]
    public required string Name { get; init; }
}