using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Numerics;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Extensions;
using template.net8.api.Domain.DTOs;
using template.net8.api.Domain.Factory;
using template.net8.api.Domain.Specifications;
using template.net8.api.Domain.Specifications.Generic;
using template.net8.api.Localize.Resources;
using template.net8.api.Persistence.Context;
using template.net8.api.Persistence.Models;
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
public sealed record CommandCreateUser(CommandCreateUserParamsDto CommandParams)
    : IRequest<LanguageExt.Common.Result<User>>, IEqualityOperators<CommandCreateUser, CommandCreateUser, bool>;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class CommandCreateUserHandler(
    IGenericDbRepositoryWriteContext<AppDbContext, User> repository,
    IOptions<PasswordOptions> config,
    IUnitOfWork<AppDbContext> unitOfWork) : IRequestHandler<CommandCreateUser, LanguageExt.Common.Result<User>>
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
    public async Task<LanguageExt.Common.Result<User>> Handle(CommandCreateUser request,
        CancellationToken cancellationToken)
    {
        var createUserResult = CreateUser(request);
        if (createUserResult.IsFaulted)
            return new LanguageExt.Common.Result<User>(createUserResult.ExtractException());

        var userInsertResult = await _repository.InsertAsync(createUserResult.ExtractData(), cancellationToken)
            .ConfigureAwait(false);
        if (userInsertResult.IsFaulted)
            return new LanguageExt.Common.Result<User>(userInsertResult.ExtractException());

        var unitResult = await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return unitResult.IsSuccess
            ? userInsertResult
            : new LanguageExt.Common.Result<User>(unitResult.ExtractException());
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private LanguageExt.Common.Result<CreateUserDto> CreateUser(CommandCreateUser request)
    {
        return UserFactory.CreateUser(request.CommandParams, _config.Pepper).Try();
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class CreateUserPasswordValidator : AbstractValidator<CommandCreateUser>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    public CreateUserPasswordValidator(IStringLocalizer<ResourceMain> localizer)
    {
        var localizerService = localizer ?? throw new ArgumentNullException(nameof(localizer));

        RuleFor(static x => x.CommandParams.Password).Equal(static x => x.CommandParams.ConfirmPassword)
            .OverridePropertyName("confirm-password")
            .WithMessage(localizerService["CreateUserValidatorPasswordNotValidMsg"])
            .WithErrorCode(localizerService["CreateUserValidatorPasswordNotValidCode"])
            .WithState(static _ => HttpStatusCode.BadRequest);
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class CreateUserEmailValidator : AbstractValidator<CommandCreateUser>
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
    public CreateUserEmailValidator(
        IGenericDbRepositoryReadContext<AppDbContext, User> repository,
        IStringLocalizer<ResourceMain> localizer)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(static x => x.CommandParams.Email).EmailAddress()
            .WithMessage(_localizer["CreateUserValidatorEmailNotValidMsg"])
            .WithErrorCode(_localizer["CreateUserValidatorEmailNotValidCode"])
            .WithState(static _ => HttpStatusCode.BadRequest);

        RuleFor(static x => x.CommandParams.Email)
            .Must(ValidateEmail)
            .WithMessage(_localizer["CreateUserValidatorEmailUsedMsg"])
            .WithErrorCode(_localizer["CreateUserValidatorEmailUsedCode"])
            .WithState(static _ => HttpStatusCode.Conflict);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private bool ValidateEmail(string email)
    {
        var verification = new UserEmailVerification(email);
        var result = _repository.Verificate(verification).Try();
        if (result.IsFaulted)
            throw new InternalServerErrorException(_localizer["InternalServerErrorValidationData"],
                result.ExtractException());

        return !result.ExtractData();
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class CreateUserRoleValidator : AbstractValidator<CommandCreateUser>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IStringLocalizer<ResourceMain> _localizer;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IGenericDbRepositoryReadContext<AppDbContext, Role> _repository;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    public CreateUserRoleValidator(
        IGenericDbRepositoryReadContext<AppDbContext, Role> repository,
        IStringLocalizer<ResourceMain> localizer)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

        RuleFor(static x => x.CommandParams.RoleId ?? 0)
            .Must(ValidateRoleId)
            .OverridePropertyName("role-id")
            .WithMessage(_localizer["CreateUserValidatorRoleNotFoundCode"])
            .WithErrorCode(_localizer["CreateUserValidatorRoleNotFoundMsg"])
            .WithState(static _ => HttpStatusCode.NotFound)
            .When(static x => x.CommandParams.RoleId is not null);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private bool ValidateRoleId(short roleId)
    {
        var verification = new EntityVerificationById<Role, short>(roleId);
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
internal sealed class CreateUserIdentifierValidator : AbstractValidator<CommandCreateUser>
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
    public CreateUserIdentifierValidator(
        IGenericDbRepositoryReadContext<AppDbContext, User> repository,
        IStringLocalizer<ResourceMain> localizer)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

        RuleFor(static x => x.CommandParams.Identity.UserUuid ?? Guid.Empty)
            .Must(ValidateIdentifier)
            .OverridePropertyName("access_token")
            .WithMessage(_localizer["TokenValidatoUserExistMsg"])
            .WithErrorCode(_localizer["TokenValidatoUserExistCode"])
            .WithState(static _ => HttpStatusCode.Forbidden)
            .When(static x => x.CommandParams.Identity.UserUuid is not null);

        RuleFor(static x => x.CommandParams.Identity.UserUuid ?? Guid.Empty)
            .Must(ValidateUserActive)
            .OverridePropertyName("access_token")
            .WithMessage(_localizer["TokenValidatoUserDisabledMsg"])
            .WithErrorCode(_localizer["TokenValidatoUserDisabledCode"])
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