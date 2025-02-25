﻿using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
            ProblemDetailsFactoryCore.CreateProblemDetailsUnauthorizedProcessFail(context.Exception, localizer);
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
    public override Task Challenge(JwtBearerChallengeContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
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
        var clientProblemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsForbiddenAccess(localizer);
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
    /// <exception cref="EncoderFallbackException">
    ///     A fallback occurred (for more information, see Character Encoding in .NET)
    ///     -and-
    ///     <see cref="EncoderFallback" /> is set to <see cref="EncoderExceptionFallback" />.
    /// </exception>
    /// <exception cref="ArgumentException">If 'expires' &lt;= 'notbefore'.</exception>
    /// <exception cref="SecurityTokenEncryptionFailedException">
    ///     both
    ///     <see>
    ///         <cref>P:System.IdentityModel.Tokens.Jwt.JwtSecurityToken.SigningCredentials</cref>
    ///     </see>
    ///     and
    ///     <see>
    ///         <cref>P:System.IdentityModel.Tokens.Jwt.JwtSecurityToken.InnerToken</cref>
    ///     </see>
    ///     are set.
    /// </exception>
    /// <exception cref="InternalServerErrorException">Condition.</exception>
    /// <exception cref="ResultFaultedInvalidOperationException">
    ///     Result is not a failure! Use ExtractData method instead and
    ///     Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractData method
    /// </exception>
    /// <exception cref="ResultSuccessInvalidOperationException">
    ///     Result is not a success! Use ExtractException method instead
    ///     and Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractException method
    /// </exception>
    public override Task MessageReceived(MessageReceivedContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        //Make authentication optional if Authorization header is missing for DEV environment
        if (ShouldAuthenticate(context, _config.Value)) return Task.CompletedTask;

        var result = TokenFactory.GenerateGenieAccessToken(_config.Value).Try();
        if (result.IsFaulted)
            throw new InternalServerErrorException(_localizer["GenieAccessTokenServerError"],
                result.ExtractException());

        context.Token = result.ExtractData(); // Create Valid Token

        return Task.CompletedTask;
    }

    private static bool ShouldAuthenticate(MessageReceivedContext? context, JwtOptions? config)
    {
        if (context is null) return true;
        if (config is null) return true;

        var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") is Envs.Local or Envs.Test;

        return !string.IsNullOrEmpty(context.HttpContext.Request.Headers.Authorization) || !isDev;
    }
}