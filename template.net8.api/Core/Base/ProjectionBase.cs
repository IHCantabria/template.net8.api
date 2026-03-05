using System.Linq.Expressions;
using template.net8.api.Core.Interfaces;
using template.net8.api.Persistence.Models.Interfaces;

namespace template.net8.api.Core.Base;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal class ProjectionBase<TEntity, TDto> : IProjection<TEntity, TDto>
    where TEntity : class, IEntity where TDto : class, IDto
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    protected ProjectionBase(Expression<Func<TEntity, TDto>> expression)
    {
        Expression = expression;
    }

    /// <inheritdoc cref="IProjection{TEntity, TDto}.Expression" />
    public Expression<Func<TEntity, TDto>> Expression { get; }
}