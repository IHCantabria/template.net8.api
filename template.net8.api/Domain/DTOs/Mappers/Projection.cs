using System.Linq.Expressions;
using template.net8.api.Core.Base;
using template.net8.api.Core.Interfaces;
using template.net8.api.Persistence.Models.Interfaces;

namespace template.net8.api.Domain.DTOs;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class Projection<TEntity, TDto> : ProjectionBase<TEntity, TDto>
    where TEntity : class, IEntity where TDto : class, IDto
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public Projection(Expression<Func<TEntity, TDto>> expression) : base(expression)
    {
    }
}