using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Extensions;
using template.net8.api.Core.Factory;
using template.net8.api.Domain.Factory;
using template.net8.api.Localize.Resources;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.Events;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class AppJwtBearerEvents(
    IOptions<JwtOptions> jwtConfig,
    IOptions<AppOptions> appConfig,
    IStringLocalizer<ResourceMain> localizer)
    : JwtBearerEvents
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
    private readonly IStringLocalizer<ResourceMain> _localizer =
        localizer ?? throw new ArgumentNullException(nameof(localizer));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    public override Task AuthenticationFailed(AuthenticationFailedContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var clientProblemDetails =
            ProblemDetailsFactoryCore.CreateProblemDetailsUnauthorizedProcessFail(context.Exception, _localizer);
        context.HttpContext.Features.Set(clientProblemDetails);
        return base.AuthenticationFailed(context);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public override Task Challenge(JwtBearerChallengeContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var httpContextProblemDetails =
            context.HttpContext.Features.Get<ProblemDetails>();
        var bearerError = httpContextProblemDetails != null ? "invalid_token" : "invalid_request";
        var bearerErrorDescription = httpContextProblemDetails != null
            ? "The access token is not valid"
            : "The access token is missing";
        if (httpContextProblemDetails == null)
        {
            httpContextProblemDetails =
                ProblemDetailsFactoryCore.CreateProblemDetailsUnauthorizedMissingToken(_localizer);
            context.HttpContext.Features.Set(httpContextProblemDetails);
        }

        context.HttpContext.Items["BearerError"] = bearerError;
        context.HttpContext.Items["BearerErrorDescription"] = bearerErrorDescription;

        return base.Challenge(context);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    public override Task TokenValidated(TokenValidatedContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return base.TokenValidated(context);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    public override Task Forbidden(ForbiddenContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var clientProblemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsForbiddenAccess(_localizer);
        context.HttpContext.Features.Set(clientProblemDetails);
        return base.Forbidden(context);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    public override Task MessageReceived(MessageReceivedContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (TryExtractSignalRAccessToken(context, out var token) ||
            TryExtractDevToken(context, out token))
            context.Token = token;

        return Task.CompletedTask;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static bool TryExtractSignalRAccessToken(MessageReceivedContext context, out string? token)
    {
        token = null;
        var accessToken = context.Request.Query["access_token"];
        var path = context.HttpContext.Request.Path;

        if (string.IsNullOrEmpty(accessToken)) return false;
        if (!path.StartsWithSegments(ApiRoutes.HubsAccess,
                StringComparison.InvariantCultureIgnoreCase)) return false;

        token = accessToken;
        return true;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static bool HasAuthorizationHeader(MessageReceivedContext context)
    {
        return !string.IsNullOrEmpty(context.HttpContext.Request.Headers.Authorization);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private bool IsDevEnvironment()
    {
        return _appConfig.Env is Envs.Local or Envs.Test;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private bool TryExtractDevToken(MessageReceivedContext context, out string? token)
    {
        token = null;
        if (!IsDevEnvironment()) return false;
        if (HasAuthorizationHeader(context)) return false;

        var result = TokenFactory.GenerateGenieAccessToken(_jwtConfig, _appConfig).Try();
        if (result.IsFaulted)
            throw new InternalServerErrorException(
                _localizer["GenieAccessTokenServerError"],
                result.ExtractException());

        token = result.ExtractData();
        return true;
    }
}