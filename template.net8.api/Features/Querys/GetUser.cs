using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Numerics;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Localization;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Extensions;
using template.net8.api.Domain.DTOs;
using template.net8.api.Domain.Specifications;
using template.net8.api.Domain.Specifications.Generic;
using template.net8.api.Localize.Resources;
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
public sealed record QueryGetUser(QueryGetUserParamsDto QueryParams)
    : IRequest<LanguageExt.Common.Result<UserDto>>, IEqualityOperators<QueryGetUser, QueryGetUser, bool>;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class QueryGetUserHandler(
    IGenericDbRepositoryReadContext<AppDbContext, User> repository)
    : IRequestHandler<QueryGetUser, LanguageExt.Common.Result<UserDto>>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IGenericDbRepositoryReadContext<AppDbContext, User> _repository =
        repository ?? throw new ArgumentNullException(nameof(repository));


    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ResultFaultedInvalidOperationException">
    ///     Result is not a failure! Use ExtractData method instead and
    ///     Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractData method
    /// </exception>
    public async Task<LanguageExt.Common.Result<UserDto>> Handle(QueryGetUser request,
        CancellationToken cancellationToken)
    {
        var specification = new UserReadByKeySpecification(request.QueryParams.Key);
        var result = await _repository.GetFirstAsync(specification, UserProjections.UserProjection, cancellationToken)
            .ConfigureAwait(false);
        return result.IsSuccess
            ? result
            : new LanguageExt.Common.Result<UserDto>(result.ExtractException());
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class GetUserKeyValidator : AbstractValidator<QueryGetUser>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IStringLocalizer<ResourceMain> _localizer;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IGenericDbRepositoryReadContext<AppDbContext, User> _repository;


    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    public GetUserKeyValidator(
        IGenericDbRepositoryReadContext<AppDbContext, User> repository,
        IStringLocalizer<ResourceMain> localizer)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(static x => x.QueryParams.Key)
            .Must(ValidateUserUuid)
            .OverridePropertyName("user-key")
            .WithMessage(_localizer["GetUserValidatorUuidNotFoundMsg"])
            .WithErrorCode(_localizer["GetUserValidatorUuidNotFoundCode"])
            .WithState(static _ => HttpStatusCode.NotFound);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private bool ValidateUserUuid(Guid key)
    {
        var verification = new EntityVerificationByUuid<User>(key);
        var result = _repository.Verificate(verification).Try();
        if (result.IsFaulted)
            throw new InternalServerErrorException(_localizer["InternalServerErrorValidationData"],
                result.ExtractException());

        return result.ExtractData();
    }
}