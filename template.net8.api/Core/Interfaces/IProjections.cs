using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using template.net8.api.Persistence.Models.Interfaces;

namespace template.net8.api.Core.Interfaces;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "The interface is part of the public API contract and must remain publicly accessible.")]
internal interface IProjection<TEntity, TDto> where TEntity : class, IEntity where TDto : class, IDto
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Expression<Func<TEntity, TDto>> Expression { get; }
}