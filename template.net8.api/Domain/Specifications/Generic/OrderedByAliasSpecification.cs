using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Attributes;
using template.net8.api.Domain.Base;
using template.net8.api.Domain.Interfaces;
using template.net8.api.Domain.Persistence.Models.Interfaces;
using template.net8.api.Domain.Specifications.Interfaces;

namespace template.net8.api.Domain.Specifications.Generic;

[CoreLibrary]
internal sealed class DtosOrderedByAliasSpecifications<TEntity, TDto> : SpecificationBase<TEntity, TDto>
    where TEntity : class, IAliasEntity where TDto : class, IDto
{
    /// <exception cref="NotSupportedException">The <see /> is read-only.</exception>
    internal DtosOrderedByAliasSpecifications(bool trackData = false)
    {
        AddOrderBy(e => e.AliasText, OrderByType.Asc);
        SetQueryTrackStrategy(trackData ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
    }
}