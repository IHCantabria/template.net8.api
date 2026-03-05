using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Numerics;
using FluentValidation;
using JetBrains.Annotations;
using LanguageExt;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
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
using template.net8.api.Settings.Options;

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
public sealed record CommandResetUserPassword(CommandResetUserPasswordParamsDto CommandParams)
    : IRequest<LanguageExt.Common.Result<UserResetedPasswordDto>>,
        IEqualityOperators<CommandResetUserPassword, CommandResetUserPassword, bool>;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class CommandResetUserPasswordHandler(
    IGenericDbRepositoryWriteContext<AppDbContext, User> repository,
    IOptions<PasswordOptions> config,
    IUnitOfWork<AppDbContext> unitOfWork)
    : IRequestHandler<CommandResetUserPassword, LanguageExt.Common.Result<UserResetedPasswordDto>>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly PasswordOptions _config = config.Value ?? throw new ArgumentNullException(nameof(config));

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
    /// <exception cref="ResultFaultedInvalidOperationException">
    ///     Result is not a failure! Use ExtractData method instead and
    ///     Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractData method
    /// </exception>
    /// <exception cref="ResultSuccessInvalidOperationException">
    ///     Result is not a success! Use ExtractException method instead
    ///     and Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractException method
    /// </exception>
    public async Task<LanguageExt.Common.Result<UserResetedPasswordDto>> Handle(CommandResetUserPassword request,
        CancellationToken cancellationToken)
    {
        var specification = new UserWriteByKeySpecification(request.CommandParams.Key);
        var userResult = await _repository.GetFirstAsync(specification, cancellationToken).ConfigureAwait(false);
        if (userResult.IsFaulted)
            return new LanguageExt.Common.Result<UserResetedPasswordDto>(userResult.ExtractException());

        var userUpdateResult = UpdateUser(request, userResult.ExtractData()).Try();
        if (userUpdateResult.IsFaulted)
            return new LanguageExt.Common.Result<UserResetedPasswordDto>(userResult.ExtractException());

        var unitResult = await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        UserResetedPasswordDto userDto = userUpdateResult.ExtractData();
        userDto.Password = request.CommandParams.Password;
        return unitResult.IsSuccess
            ? userDto
            : new LanguageExt.Common.Result<UserResetedPasswordDto>(unitResult.ExtractException());
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private Try<User> UpdateUser(CommandResetUserPassword request, User user)
    {
        return () =>
        {
            var userUpdatePasswordResult =
                user.UpdateUserPassword(request.CommandParams, _config.Pepper).Try();
            return userUpdatePasswordResult.IsFaulted
                ? new LanguageExt.Common.Result<User>(userUpdatePasswordResult.ExtractException())
                : _repository.Update(user).Try();
        };
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class ResetUserPasswordPasswordValidator : AbstractValidator<CommandResetUserPassword>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    public ResetUserPasswordPasswordValidator(IStringLocalizer<ResourceMain> localizer)
    {
        var localizer1 = localizer ?? throw new ArgumentNullException(nameof(localizer));

        RuleFor(static x => x.CommandParams.Password).Equal(static x => x.CommandParams.ConfirmPassword)
            .OverridePropertyName("confirm_password")
            .WithMessage(localizer1["ResetUserPasswordValidatorPasswordNotValidMasg"])
            .WithErrorCode(localizer1["ResetUserPasswordValidatorPasswordNotValidCode"])
            .WithErrorCode(StatusCodes.Status400BadRequest.ToString(CultureInfo.InvariantCulture))
            .WithState(static _ => HttpStatusCode.BadRequest);
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class ResetUserPasswordIdentifierValidator : AbstractValidator<CommandResetUserPassword>
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
    public ResetUserPasswordIdentifierValidator(
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
internal sealed class ResetUserPasswordKeyValidator : AbstractValidator<CommandResetUserPassword>
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
    public ResetUserPasswordKeyValidator(
        IGenericDbRepositoryReadContext<AppDbContext, User> repository,
        IStringLocalizer<ResourceMain> localizer)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(static x => x.CommandParams.Key)
            .Must(ValidateUserUuid)
            .OverridePropertyName("user-key")
            .WithMessage(_localizer["ResetUserPasswordValidatorUuidNotFoundMsg"])
            .WithErrorCode(_localizer["ResetUserPasswordValidatorUuidNotFoundCode"])
            .WithState(static _ => HttpStatusCode.NotFound);

        RuleFor(static x => x.CommandParams.Key)
            .Must(ValidateUserActive)
            .OverridePropertyName("user-key")
            .WithMessage(_localizer["ResetUserPasswordValidatorUserDisabledMsg"])
            .WithErrorCode(_localizer["ResetUserPasswordValidatorUserDisabledCode"])
            .WithState(static _ => HttpStatusCode.Forbidden);
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

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private bool ValidateUserActive(Guid key)
    {
        var verification = new UserEnabledVerification(key);
        var result = _repository.Verificate(verification).Try();
        if (result.IsFaulted)
            throw new InternalServerErrorException(_localizer["InternalServerErrorValidationData"],
                result.ExtractException());

        return result.ExtractData();
    }
}