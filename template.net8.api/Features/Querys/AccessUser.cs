using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Numerics;
using System.Text;
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
public sealed record QueryAccessUser(QueryAccessUserParamsDto QueryParams)
    : IRequest<LanguageExt.Common.Result<AccessTokenDto>>,
        IEqualityOperators<QueryAccessUser, QueryAccessUser, bool>;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class QueryAccessUserHandler(
    IGenericDbRepositoryReadContext<AppDbContext, User> repository,
    IOptions<JwtOptions> jwtConfig,
    IOptions<AppOptions> appConfig)
    : IRequestHandler<QueryAccessUser, LanguageExt.Common.Result<AccessTokenDto>>
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
    /// <exception cref="ArgumentNullException">if 'key' is null.</exception>
    /// <exception cref="ArgumentException">If 'expires' &lt;= 'notbefore'.</exception>
    /// <exception cref="EncoderFallbackException">
    ///     A fallback occurred (for more information, see Character Encoding in .NET)
    ///     -and-
    ///     <see cref="EncoderFallback" /> is set to <see cref="EncoderExceptionFallback" />.
    /// </exception>
    public async Task<LanguageExt.Common.Result<AccessTokenDto>> Handle(QueryAccessUser request,
        CancellationToken cancellationToken)
    {
        if (request.QueryParams.Identity.IsGenie())
        {
            var resultGenieToken = TokenFactory.GenerateGenieAccessTokenDto(_jwtConfig, _appConfig).Try();
            return resultGenieToken.IsSuccess
                ? resultGenieToken
                : new LanguageExt.Common.Result<AccessTokenDto>(resultGenieToken.ExtractException());
        }

        var specification = new UserReadByKeySpecification(request.QueryParams.Identity.UserUuid ?? Guid.Empty);
        var resultUser = await _repository
            .GetFirstAsync(specification, UserProjections.UserAccessTokenProjection, cancellationToken)
            .ConfigureAwait(false);
        if (resultUser.IsFaulted)
            return new LanguageExt.Common.Result<AccessTokenDto>(resultUser.ExtractException());

        var resultToken = TokenFactory.GenerateAccessTokenDto(resultUser.ExtractData(), _jwtConfig, _appConfig).Try();
        return resultToken.IsSuccess
            ? resultToken
            : new LanguageExt.Common.Result<AccessTokenDto>(resultToken.ExtractException());
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class AccessUserUuidValidator : AbstractValidator<QueryAccessUser>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IStringLocalizer<ResourceMain> _localizer;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IGenericDbRepositoryReadContext<AppDbContext, Role> _roleRepository;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IGenericDbRepositoryReadContext<AppDbContext, User> _userRepository;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    public AccessUserUuidValidator(
        IGenericDbRepositoryReadContext<AppDbContext, Role> roleRepository,
        IGenericDbRepositoryReadContext<AppDbContext, User> userRepository,
        IStringLocalizer<ResourceMain> localizer)
    {
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));

        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

        ClassLevelCascadeMode = CascadeMode.Stop;
        When(static x => !x.QueryParams.Identity.IsGenie(), () =>
        {
            RuleFor(static x => x.QueryParams.Identity.UserUuid)
                .NotNull()
                .OverridePropertyName("id_token")
                .WithMessage(_localizer["AccessUserValidatorUuidMissingMsg"])
                .WithErrorCode(_localizer["AccessUserValidatorUuidMissingCode"])
                .WithState(static _ => HttpStatusCode.Unauthorized);

            RuleFor(static x => x.QueryParams.Identity.UserUuid)
                .NotEmpty()
                .OverridePropertyName("id_token")
                .WithMessage(_localizer["AccessUserValidatorUuidEmptyMsg"])
                .WithErrorCode(_localizer["AccessUserValidatorUuidEmptyCode"])
                .WithState(static _ => HttpStatusCode.Unauthorized);

            RuleFor(static x => x.QueryParams.Identity.UserUuid ?? Guid.Empty)
                .Must(ValidateTokenUserActive)
                .OverridePropertyName("id_token")
                .WithMessage(_localizer["AccessUserValidatorUserDisabledMsg"])
                .WithErrorCode(_localizer["AccessUserValidatorUserDisabledCode"])
                .WithState(static _ => HttpStatusCode.Forbidden);

            RuleFor(static x => x.QueryParams.Identity.UserRoleName ?? "")
                .Must(ValidateTokenUserRole)
                .OverridePropertyName("id_token")
                .WithMessage(_localizer["AccessUserValidatorRoleInvalidMsg"])
                .WithErrorCode(_localizer["AccessUserValidatorRoleInvalidCode"])
                .WithState(static _ => HttpStatusCode.Unauthorized)
                .When(static x => x.QueryParams.Identity.HasRole());
        });
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private bool ValidateTokenUserActive(Guid key)
    {
        var verification = new UserEnabledVerification(key);
        var result = _userRepository.Verificate(verification).Try();
        if (result.IsFaulted)
            throw new InternalServerErrorException(_localizer["InternalServerErrorValidationData"],
                result.ExtractException());

        return result.ExtractData();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private bool ValidateTokenUserRole(string roleName)
    {
        var verification = new EntityVerificationByNameKey<Role>(roleName);
        var result = _roleRepository.Verificate(verification).Try();
        if (result.IsFaulted)
            throw new InternalServerErrorException(_localizer["InternalServerErrorValidationData"],
                result.ExtractException());

        return result.ExtractData();
    }
}