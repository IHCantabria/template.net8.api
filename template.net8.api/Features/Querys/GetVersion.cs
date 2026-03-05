using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using MediatR;
using template.net8.api.Core.DTOs;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Extensions;
using template.net8.api.Domain.Specifications;
using template.net8.api.Persistence.Context;
using template.net8.api.Persistence.Models;
using template.net8.api.Persistence.Repositories.Interfaces;

namespace template.net8.api.Features.Querys;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Public visibility is required because this request is part of the application messaging contract (MediatR).")]
[SuppressMessage(
    "Design",
    "MemberCanBeInternal",
    Justification =
        "Public visibility is required because this request is part of the application messaging contract (MediatR).")]
public sealed record QueryGetVersion : IRequest<LanguageExt.Common.Result<VersionDto>>,
    IEqualityOperators<QueryGetVersion, QueryGetVersion, bool>;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class QueryGetVersionHandler(
    IGenericDbRepositoryReadContext<AppDbContext, CurrentVersion> repository)
    : IRequestHandler<QueryGetVersion, LanguageExt.Common.Result<VersionDto>>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IGenericDbRepositoryReadContext<AppDbContext, CurrentVersion> _repository =
        repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ResultFaultedInvalidOperationException">
    ///     Result is not a failure! Use ExtractData method instead and
    ///     Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractData method
    /// </exception>
    public async Task<LanguageExt.Common.Result<VersionDto>> Handle(QueryGetVersion request,
        CancellationToken cancellationToken)
    {
        var specification = new CurrentVersionReadSpecification();
        var result = await _repository
            .GetFirstAsync(specification, CurrentVersionProjections.VersionProjection, cancellationToken)
            .ConfigureAwait(false);
        return result.IsSuccess
            ? result
            : new LanguageExt.Common.Result<VersionDto>(result.ExtractException());
    }
}