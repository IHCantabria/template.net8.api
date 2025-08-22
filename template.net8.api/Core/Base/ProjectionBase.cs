using System.Linq.Expressions;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Interfaces;
using template.net8.api.Domain.Persistence.Models.Interfaces;

namespace template.net8.api.Core.Base;

// Generic Projection
/// <summary>
///     Projection Base
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TDto"></typeparam>
[CoreLibrary]
public class ProjectionBase<TEntity, TDto> : IProjection<TEntity, TDto>
    where TEntity : class, IEntity where TDto : class, IDto
{
    /// <summary>
    ///     Constructs a ProjectionBase instance.
    /// </summary>
    protected ProjectionBase(Expression<Func<TEntity, TDto>> expression)
    {
        Expression = expression;
    }

    /// <summary>
    ///     Expression to project the entity to a DTO for the Query Specification Pattern Implementation for Querying Data
    /// </summary>
    public Expression<Func<TEntity, TDto>> Expression { get; }
}