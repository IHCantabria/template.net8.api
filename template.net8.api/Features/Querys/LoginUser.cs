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
using template.net8.api.Domain.Password;
using template.net8.api.Domain.Specifications;
using template.net8.api.Localize.Resources;
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
public sealed record QueryLoginUser(QueryLoginUserParamsDto QueryParams)
    : IRequest<LanguageExt.Common.Result<IdTokenDto>>, IEqualityOperators<QueryLoginUser, QueryLoginUser, bool>;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class QueryLoginUserHandler(
    IGenericDbRepositoryReadContext<AppDbContext, User> repository,
    IOptions<JwtOptions> jwtConfig,
    IOptions<AppOptions> appConfig)
    : IRequestHandler<QueryLoginUser, LanguageExt.Common.Result<IdTokenDto>>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly AppOptions _appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly JwtOptions _jwtConfig = jwtConfig.Value ?? throw new ArgumentNullException(nameof(jwtConfig));

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
    /// <exception cref="ResultSuccessInvalidOperationException">
    ///     Result is not a success! Use ExtractException method instead
    ///     and Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractException method
    /// </exception>
    public async Task<LanguageExt.Common.Result<IdTokenDto>> Handle(QueryLoginUser request,
        CancellationToken cancellationToken)
    {
        var specification = new UserReadByEmailSpecification(request.QueryParams.Email);
        var resultUser = await _repository
            .GetFirstAsync(specification, UserProjections.UserIdTokenProjection, cancellationToken)
            .ConfigureAwait(false);
        return resultUser.IsSuccess
            ? TokenFactory.GenerateIdTokenDto(resultUser.ExtractData(), _jwtConfig, _appConfig)
            : new LanguageExt.Common.Result<IdTokenDto>(resultUser.ExtractException());
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class LoginUserEmailValidator : AbstractValidator<QueryLoginUser>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly PasswordOptions _config;

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
    /// <exception cref="ArgumentNullException"><paramref name="config" /> is <see langword="null" />.</exception>
    public LoginUserEmailValidator(
        IGenericDbRepositoryReadContext<AppDbContext, User> repository, IOptions<PasswordOptions> config,
        IStringLocalizer<ResourceMain> localizer)
    {
        ArgumentNullException.ThrowIfNull(config);

        _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        _config = config.Value ?? throw new ArgumentNullException(nameof(config));

        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(static x => x.QueryParams.Email).EmailAddress()
            .OverridePropertyName("email")
            .WithMessage(_localizer["LoginUserValidatorEmailNotValidMsg"])
            .WithErrorCode(_localizer["LoginUserValidatorEmailNotValidCode"])
            .WithState(static _ => HttpStatusCode.BadRequest);

        RuleFor(static y => y.QueryParams.Email)
            .Must(ValidateEmail)
            .OverridePropertyName("email")
            .WithMessage(_localizer["LoginUserValidatorEmailNotFoundMsg"])
            .WithErrorCode(_localizer["LoginUserValidatorEmailNotFoundCode"])
            .WithState(static _ => HttpStatusCode.NotFound);

        RuleFor(static x => x.QueryParams.Email)
            .Must(ValidateUserActive)
            .OverridePropertyName("email")
            .WithMessage(_localizer["LoginUserValidatorUserDisabledMsg"])
            .WithErrorCode(_localizer["LoginUserValidatorUserDisabledCode"])
            .WithState(static _ => HttpStatusCode.Forbidden);

        RuleFor(static x => x.QueryParams)
            .Must(ValidatePassword)
            .OverridePropertyName("password")
            .WithMessage(_localizer["LoginUserValidatorPasswordInvalidMsg"])
            .WithErrorCode(_localizer["LoginUserValidatorPasswordInvalidCode"])
            .WithState(static _ => HttpStatusCode.Forbidden);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private bool ValidateUserActive(string email)
    {
        var verification = new UserEnabledVerification(email);
        var result = _repository.Verificate(verification).Try();
        if (result.IsFaulted)
            throw new InternalServerErrorException(_localizer["InternalServerErrorValidationData"],
                result.ExtractException());

        return result.ExtractData();
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

        return result.ExtractData();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private bool ValidatePassword(QueryLoginUserParamsDto queryParams)
    {
        var specification = new UserReadByEmailSpecification(queryParams.Email);
        var resultUser = _repository.GetFirst(specification, UserProjections.UserCredentialsProjection).Try();
        if (resultUser.IsFaulted)
            throw new InternalServerErrorException(_localizer["InternalServerErrorValidationData"],
                resultUser.ExtractException());

        var resultValidation = PasswordUtils
            .VerifyUserCredentials(resultUser.ExtractData(), queryParams.Password, _config.Pepper).Try();
        if (resultValidation.IsFaulted)
            throw new InternalServerErrorException(_localizer["InternalServerErrorValidationData"],
                resultValidation.ExtractException());

        return resultValidation.ExtractData();
    }
}