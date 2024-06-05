using JetBrains.Annotations;
using LanguageExt.Common;
using MediatR;
using template.net8.api.Domain.DTOs;

namespace template.net8.api.Features.Querys;

/// <summary>
///     Get Dummies CQRS Query
/// </summary>
[UsedImplicitly]
public sealed record GetDummiesQuery : IRequest<Result<IEnumerable<DummyDto>>>;

/// <summary>
///     Get Dummy Dummies Query Handler
/// </summary>
[UsedImplicitly]
public sealed class GetDummiesHandlerQuery : IRequestHandler<GetDummiesQuery, Result<IEnumerable<DummyDto>>>
{
    /// <summary>
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result<IEnumerable<DummyDto>>> Handle(GetDummiesQuery request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new Result<IEnumerable<DummyDto>>(new List<DummyDto>
        {
            new()
            {
                Text = "Dummy 1"
            }
        }));
    }
}