using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Base;
using template.net8.api.Core.Interfaces;
using template.net8.api.Persistence.Models.Interfaces;

namespace template.net8.api.Domain.Specifications.Generic;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "ReSharper",
    "UnusedType.Global",
    Justification =
        "Generic specification type intended for reuse in repository queries; usage may be indirect or consumer-dependent.")]
internal sealed class DtosOrderedByAliasSpecifications<TEntity> : SpecificationBase<TEntity>
    where TEntity : class, IEntityWithAlias
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal DtosOrderedByAliasSpecifications(bool trackData = false)
    {
        AddOrderBy(static e => e.AliasText, OrderByType.Asc);
        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
        SetQuerySplitStrategy(QuerySplittingBehavior.SingleQuery);
    }
}