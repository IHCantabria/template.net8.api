using System.Numerics;
using JetBrains.Annotations;
using MediatR;
using template.net8.api.Domain.DTOs;

namespace template.net8.api.Features.Querys;

/// <summary>
///     Query Get Dummies CQRS
/// </summary>
public sealed record QueryGetDummies : IRequest<LanguageExt.Common.Result<IEnumerable<DummyDto>>>,
    IEqualityOperators<QueryGetDummies, QueryGetDummies, bool>;

[UsedImplicitly]
internal sealed class
    GetDummiesHandlerQuery : IRequestHandler<QueryGetDummies, LanguageExt.Common.Result<IEnumerable<DummyDto>>>
{
    /// <summary>
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<LanguageExt.Common.Result<IEnumerable<DummyDto>>> Handle(QueryGetDummies request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(
            new LanguageExt.Common.Result<IEnumerable<DummyDto>>([new DummyDto { Key = "1", Text = "Dummy 1" }]));
    }
}