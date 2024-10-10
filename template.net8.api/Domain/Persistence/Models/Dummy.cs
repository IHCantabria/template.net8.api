using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using template.net8.api.Domain.Persistence.Models.Interfaces;

namespace template.net8.api.Domain.Persistence.Models;

/// <summary>
///     Dummy Model
/// </summary>
public partial class Dummy : IEntity
{
    /// <summary>
    ///     Text
    /// </summary>
    [Column("text")]
    [MaxLength(50)]
    public string Text { get; init; } = null!;

    /// <summary>
    ///     Key (Primary Key)
    /// </summary>
    [Column("key")]
    [MaxLength(50)]
    public string Key { get; init; } = null!;
}