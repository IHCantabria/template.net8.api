using System.Numerics;
using LanguageExt.Common;
using MediatR;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.DTOs;

namespace template.net8.api.Features.Querys;

/// <summary>
///     Query Get Version CQRS
/// </summary>
[CoreLibrary]
public sealed record QueryGetVersion : IRequest<Result<VersionDto>>,
    IEqualityOperators<QueryGetVersion, QueryGetVersion, bool>;

[CoreLibrary]
internal sealed class QueryGetVersionHandler : IRequestHandler<QueryGetVersion, Result<VersionDto>>
{
    /// <summary>
    ///     Handle the Get Version Query request
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result<VersionDto>> Handle(QueryGetVersion request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Result<VersionDto>(new VersionDto
        {
            Name = "Dummy",
            Tag = "0.0.0",
            Date = DateTimeOffset.UtcNow,
            Id = 0
        }));
    }
}