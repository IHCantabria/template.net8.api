using System.Linq.Expressions;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Base;
using template.net8.api.Core.Interfaces;
using template.net8.api.Domain.Persistence.Models.Interfaces;

namespace template.net8.api.Domain.DTOs;

/// <summary>
///     Projection Base
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TDto"></typeparam>
[CoreLibrary]
public sealed class Projection<TEntity, TDto> : ProjectionBase<TEntity, TDto>
    where TEntity : class, IEntity where TDto : class, IDto
{
    /// <summary>
    ///     Constructs a Projection instance with the specified expression.
    /// </summary>
    /// <param name="expression"></param>
    public Projection(Expression<Func<TEntity, TDto>> expression) : base(expression)
    {
    }
}