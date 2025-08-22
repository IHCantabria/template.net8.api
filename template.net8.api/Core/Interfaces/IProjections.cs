using System.Linq.Expressions;
using template.net8.api.Core.Attributes;
using template.net8.api.Domain.Persistence.Models.Interfaces;

namespace template.net8.api.Core.Interfaces;

/// <summary>
///     Interface for Projection Implementation for Querying Data with EF Core Queryable Extensions and EF Core
///     Queryable Extensions for Dto Projection.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TDto"></typeparam>
[CoreLibrary]
public interface IProjection<TEntity, TDto> where TEntity : class, IEntity where TDto : class, IDto
{
    /// <summary>
    ///     Expression to project the entity to a DTO for the Query Specification Pattern Implementation for Querying Data
    /// </summary>
    Expression<Func<TEntity, TDto>> Expression { get; }
}