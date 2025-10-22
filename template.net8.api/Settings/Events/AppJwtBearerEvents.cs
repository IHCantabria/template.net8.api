using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Authorization;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Extensions;
using template.net8.api.Core.Factory;
using template.net8.api.Localize.Resources;
using template.net8.api.Settings.Options;

namespace template.net8.api.Settings.Events;

/// <summary>
///     App Jwt Bearer Events
/// </summary>
[CoreLibrary]
public sealed class AppJwtBearerEvents(IOptions<JwtOptions> config, IStringLocalizer<ResourceMain> localizer)
    : JwtBearerEvents
{
    private readonly IOptions<JwtOptions> _config = config ?? throw new ArgumentNullException(nameof(config));

    private readonly IStringLocalizer<ResourceMain> _localizer =
        localizer ?? throw new ArgumentNullException(nameof(localizer));

    /// <summary>
    ///     AuthenticationFailed event handler
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public override Task AuthenticationFailed(AuthenticationFailedContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var clientProblemDetails =
            ProblemDetailsFactoryCore.CreateProblemDetailsUnauthorizedProcessFail(context.Exception, _localizer);
        context.HttpContext.Features.Set(clientProblemDetails);
        return base.AuthenticationFailed(context);
    }

    /// <summary>
    ///     AuthenticationFailed event handler
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>IDictionary`2</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    /// <exception cref="System.Collections.Generic.KeyNotFoundException">
    ///     The property is retrieved and
    ///     <paramref>
    ///         <name>key</name>
    ///     </paramref>
    ///     is not found.
    /// </exception>
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
    ///     AuthenticationFailed event handler
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public override Task TokenValidated(TokenValidatedContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return base.TokenValidated(context);
    }

    /// <summary>
    ///     AuthenticationFailed event handler
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>dictionary</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public override Task Forbidden(ForbiddenContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var clientProblemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsForbiddenAccess(_localizer);
        context.HttpContext.Features.Set(clientProblemDetails);
        return base.Forbidden(context);
    }

    /// <summary>
    ///     MessageReceived event handler
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>s</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public override Task MessageReceived(MessageReceivedContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (TryExtractSignalRAccessToken(context, out var token) ||
            TryExtractDevToken(context, out token))
            context.Token = token;

        return Task.CompletedTask;
    }

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

    private static bool HasAuthorizationHeader(MessageReceivedContext context)
    {
        return !string.IsNullOrEmpty(context.HttpContext.Request.Headers.Authorization);
    }

    private static bool IsDevEnvironment()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        return env is Envs.Local or Envs.Test;
    }

    private bool TryExtractDevToken(MessageReceivedContext context, out string? token)
    {
        token = null;
        if (!IsDevEnvironment()) return false;
        if (HasAuthorizationHeader(context)) return false;

        var result = TokenFactory.GenerateGenieAccessToken(_config.Value).Try();
        if (result.IsFaulted)
            throw new InternalServerErrorException(
                _localizer["GenieAccessTokenServerError"],
                result.ExtractException());

        token = result.ExtractData();
        return true;
    }
}