using System.Diagnostics.CodeAnalysis;
using template.net8.api.Domain.DTOs;

namespace template.net8.api.Domain.Interfaces;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "ReSharper",
    "UnusedMemberInSuper.Global",
    Justification = "Member is part of the abstraction contract and may be consumed polymorphically.")]
internal interface IIdentity
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    IdentityDto Identity { get; set; }
}