using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using MediatR;
using Microsoft.Extensions.Options;
using Npgsql;
using template.net8.api.Core.DTOs;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Extensions;
using template.net8.api.Logger;
using template.net8.api.Persistence.Context;
using template.net8.api.Persistence.Models;
using template.net8.api.Persistence.Repositories.Interfaces;
using template.net8.api.Settings.Options;

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
public sealed record QueryCheckStatus : IRequest<LanguageExt.Common.Result<InfoDto>>,
    IEqualityOperators<QueryCheckStatus, QueryCheckStatus, bool>;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class QueryCheckStatusHandler(
    IGenericDbRepositoryReadContext<AppDbContext, CurrentVersion> repository,
    IOptions<ProjectOptions> options,
    ILogger<QueryCheckStatusHandler> logger)
    : IRequestHandler<QueryCheckStatus, LanguageExt.Common.Result<InfoDto>>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly ILogger _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly ProjectOptions _options =
        options.Value ?? throw new ArgumentNullException(nameof(options));

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
    public async Task<LanguageExt.Common.Result<InfoDto>> Handle(QueryCheckStatus request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _repository.VerificateAsync(null, cancellationToken).ConfigureAwait(false);
            return result.IsSuccess
                ? new InfoDto
                {
                    Status = StatusCodes.Status200OK,
                    Info = "API is running fine.",
                    Version = _options.Version
                }
                : new InfoDto
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Info = result.ExtractException().Message,
                    Version = _options.Version
                };
        }
        catch (NpgsqlException ex)
        {
            _logger.LogStatusDbFail(ex);
            return new InfoDto
            {
                Status = StatusCodes.Status500InternalServerError,
                Info = ex.Message,
                Version = _options.Version
            };
        }
    }
}