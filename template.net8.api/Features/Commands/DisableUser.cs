using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
using template.net8.api.Persistence.Models.Extensions;
using template.net8.api.Persistence.Repositories.Interfaces;

namespace template.net8.api.Features.Commands;

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
public sealed record CommandDisableUser(CommandDisableUserParamsDto CommandParams)
    : IRequest<LanguageExt.Common.Result<User>>,
        IEqualityOperators<CommandDisableUser, CommandDisableUser, bool>;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class CommandDisableUserHandler(
    IGenericDbRepositoryWriteContext<AppDbContext, User> repository,
    IUnitOfWork<AppDbContext> unitOfWork) : IRequestHandler<CommandDisableUser, LanguageExt.Common.Result<User>>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IGenericDbRepositoryWriteContext<AppDbContext, User> _repository =
        repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IUnitOfWork<AppDbContext> _unitOfWork =
        unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ResultSuccessInvalidOperationException">
    ///     Result is not a success! Use ExtractException method instead
    ///     and Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractException method
    /// </exception>
    /// <exception cref="ResultFaultedInvalidOperationException">
    ///     Result is not a failure! Use ExtractData method instead and
    ///     Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractData method
    /// </exception>
    /// <exception cref="ResultFaultedInvalidOperationException">
    ///     Result is not a failure! Use ExtractData method instead and
    ///     Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractData method
    /// </exception>
    public async Task<LanguageExt.Common.Result<User>> Handle(CommandDisableUser request,
        CancellationToken cancellationToken)
    {
        var specification = new UserWriteByKeySpecification(request.CommandParams.Key);
        var userResult = await _repository.GetFirstAsync(specification, cancellationToken)
            .ConfigureAwait(false);
        if (userResult.IsFaulted)
            return new LanguageExt.Common.Result<User>(userResult.ExtractException());

        var userToDisable = userResult.ExtractData();
        userToDisable.DisableUser();
        var updateResult = _repository.Update(userToDisable).Try();
        if (updateResult.IsFaulted)
            return new LanguageExt.Common.Result<User>(updateResult.ExtractException());

        var unitResult = await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return unitResult.IsSuccess
            ? updateResult
            : new LanguageExt.Common.Result<User>(updateResult.ExtractException());
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class DisableUserIdentifierValidator : AbstractValidator<CommandDisableUser>
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
    public DisableUserIdentifierValidator(
        IGenericDbRepositoryReadContext<AppDbContext, User> repository,
        IStringLocalizer<ResourceMain> localizer)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

        RuleFor(static x => x.CommandParams.Identity.UserUuid ?? Guid.Empty)
            .Must(ValidateIdentifier)
            .WithMessage(_localizer["TokenValidatoUserExistMsg"])
            .WithErrorCode(_localizer["TokenValidatoUserExistCode"])
            .WithErrorCode(StatusCodes.Status403Forbidden.ToString(CultureInfo.InvariantCulture))
            .WithState(static _ => HttpStatusCode.Forbidden)
            .When(static x => x.CommandParams.Identity.UserUuid is not null);

        RuleFor(static x => x.CommandParams.Identity.UserUuid ?? Guid.Empty)
            .Must(ValidateUserActive)
            .OverridePropertyName("access_token")
            .WithMessage(_localizer["TokenValidatoUserDisabledMsg"])
            .WithErrorCode(_localizer["TokenValidatoUserDisabledCode"])
            .WithErrorCode(StatusCodes.Status403Forbidden.ToString(CultureInfo.InvariantCulture))
            .WithState(static _ => HttpStatusCode.Forbidden)
            .When(static x => x.CommandParams.Identity.UserUuid is not null);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private bool ValidateIdentifier(Guid uuid)
    {
        var verification = new EntityVerificationByUuid<User>(uuid);
        var result = _repository.Verificate(verification).Try();
        if (result.IsFaulted)
            throw new InternalServerErrorException(_localizer["InternalServerErrorValidationData"],
                result.ExtractException());

        return result.ExtractData();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private bool ValidateUserActive(Guid uuid)
    {
        var verification = new UserEnabledVerification(uuid);
        var result = _repository.Verificate(verification).Try();
        if (result.IsFaulted)
            throw new InternalServerErrorException(_localizer["InternalServerErrorValidationData"],
                result.ExtractException());

        return result.ExtractData();
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class DisableUserUserValidator : AbstractValidator<CommandDisableUser>
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
    public DisableUserUserValidator(
        IGenericDbRepositoryReadContext<AppDbContext, User> repository,
        IStringLocalizer<ResourceMain> localizer)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(static x => x.CommandParams.Key)
            .Must(ValidateUserUuid)
            .WithMessage(_localizer["DisableUserValidatorUuidNotFoundMsg"])
            .WithErrorCode(_localizer["DisableUserValidatorUuidNotFoundCode"])
            .WithErrorCode(StatusCodes.Status404NotFound.ToString(CultureInfo.InvariantCulture))
            .WithState(static _ => HttpStatusCode.NotFound);

        RuleFor(static x => x.CommandParams.Key)
            .Must(ValidateUserDisabled)
            .OverridePropertyName("user-key")
            .WithMessage(_localizer["DisableUserValidatorUserDisabledMsg"])
            .WithErrorCode(_localizer["DisableUserValidatorUserDisabledCode"])
            .WithState(static _ => HttpStatusCode.Conflict);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private bool ValidateUserUuid(Guid userUuid)
    {
        var verification = new EntityVerificationByUuid<User>(userUuid);
        var result = _repository.Verificate(verification).Try();
        if (result.IsFaulted)
            throw new InternalServerErrorException(_localizer["InternalServerErrorValidationData"],
                result.ExtractException());

        return result.ExtractData();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private bool ValidateUserDisabled(Guid userUuid)
    {
        var verification = new UserDisabledVerification(userUuid);
        var result = _repository.Verificate(verification).Try();
        if (result.IsFaulted)
            throw new InternalServerErrorException(_localizer["InternalServerErrorValidationData"],
                result.ExtractException());

        return !result.ExtractData();
    }
}