using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Base;
using template.net8.api.Core.Interfaces;
using template.net8.api.Domain.Persistence.Models.Interfaces;

namespace template.net8.api.Domain.Specifications.Generic;

[CoreLibrary]
internal sealed class DtosOrderedByAliasSpecifications<TEntity> : SpecificationBase<TEntity>
    where TEntity : class, IEntityWithAlias
{
    /// <summary>
    ///     Constructs a specification to order entities by their alias text.
    /// </summary>
    /// <param name="trackData"></param>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>ICollection`1</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    internal DtosOrderedByAliasSpecifications(bool trackData = false)
    {
        AddOrderBy(e => e.AliasText, OrderByType.Asc);
        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
    }
}