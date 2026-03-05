using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using template.net8.api.Persistence.Models.Interfaces;

namespace template.net8.api.Persistence.Models;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[Table("version")]
[Microsoft.EntityFrameworkCore.Index("Name", Name = "versiob_key", IsUnique = true)]
internal class Version : IEntityWithNameKey, IEntityWithId<short>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Column("tag")]
    [MaxLength(100)]
    public required string Tag { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [Column("date", TypeName = "timestamp without time zone")]
    public required DateTime Date { get; set; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [InverseProperty("Version")]
    public virtual CurrentVersion? CurrentVersion { get; set; }

    /// <inheritdoc cref="IEntityWithId{short}.Id" />
    [Key]
    [Column("id")]
    public required short Id { get; init; }

    /// <inheritdoc cref="IEntityWithNameKey.Name" />
    [Column("name")]
    [MaxLength(100)]
    public required string Name { get; init; }
}