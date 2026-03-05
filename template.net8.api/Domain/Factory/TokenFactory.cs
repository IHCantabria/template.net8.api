using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LanguageExt;
using Microsoft.IdentityModel.Tokens;
using template.net8.api.Core.Authorization;
using template.net8.api.Core.Authorization.DTOs.Base;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Extensions;
using template.net8.api.Domain.DTOs;
using template.net8.api.Settings;
using template.net8.api.Settings.Options;

namespace template.net8.api.Domain.Factory;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class TokenFactory
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static IdTokenDto GenerateIdTokenDto(UserIdTokenBaseDto user, JwtOptions jwtConfig, AppOptions appConfig)
    {
        return new IdTokenDto
        {
            IdToken = GenerateIdToken(user, jwtConfig, appConfig)
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static Try<AccessTokenDto> GenerateAccessTokenDto(UserAccessTokenBaseDto user, JwtOptions jwtConfig,
        AppOptions appConfig)
    {
        return () => new AccessTokenDto
        {
            AccessToken = GenerateAccessToken(user, jwtConfig, appConfig),
            RefreshToken = GenerateAccessToken(user, jwtConfig, appConfig)
        };
    }

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
    internal static Try<AccessTokenDto> GenerateGenieAccessTokenDto(JwtOptions jwtConfig, AppOptions appConfig)
    {
        return () =>
        {
            var result = GenerateGenieAccessToken(jwtConfig, appConfig).Try();
            if (result.IsFaulted)
                return new LanguageExt.Common.Result<AccessTokenDto>(result.ExtractException());

            var accessToken = result.ExtractData();
            return new AccessTokenDto
            {
                AccessToken = accessToken,
                RefreshToken = accessToken
            };
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static string GenerateIdToken(UserIdTokenBaseDto user, JwtOptions jwtConfig, AppOptions appConfig)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var exp = GetExpirationDateTime(jwtConfig.TokenLifetime, appConfig.Env);
        var claims = AddUserClaims(user);
        var token = new JwtSecurityToken(jwtConfig.Issuer, jwtConfig.Audience, claims, null, exp, credentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static string GenerateAccessToken(UserAccessTokenBaseDto user, JwtOptions jwtConfig, AppOptions appConfig)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = AddUserClaims(user);
        var exp = GetExpirationDateTime(jwtConfig.TokenLifetime, appConfig.Env);
        var token = new JwtSecurityToken(jwtConfig.Issuer, jwtConfig.Audience, claims, null, exp, credentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ReturnTypeCanBeEnumerable.Local",
        Justification =
            "Concrete return type is intentional for performance and to avoid interface-based enumeration overhead.")]
    private static List<Claim> AddUserClaims(UserIdTokenBaseDto user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Uuid.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Name, user.Username),
            new(ClaimCoreConstants.TokenTypeClaim, TokenTypesIdentityConstants.IdTokenType),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddClaimIfNotNull(ClaimCoreConstants.RoleClaim, user.RoleName);
        claims.AddClaimIfNotNull(JwtRegisteredClaimNames.FamilyName, user.LastName);
        claims.AddClaimIfNotNull(JwtRegisteredClaimNames.GivenName, user.FirstName);
        return claims;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ReturnTypeCanBeEnumerable.Local",
        Justification =
            "Concrete return type is intentional for performance and to avoid interface-based enumeration overhead.")]
    private static List<Claim> AddUserClaims(UserAccessTokenBaseDto user)
    {
        var claims = new Collection<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Uuid.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Name, user.Username),
            new(ClaimCoreConstants.TokenTypeClaim, TokenTypesIdentityConstants.AccessTokenType),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddClaimIfNotNull(ClaimCoreConstants.RoleClaim, user.RoleName);
        claims.AddClaimIfNotNull(JwtRegisteredClaimNames.FamilyName, user.LastName);
        claims.AddClaimIfNotNull(JwtRegisteredClaimNames.GivenName, user.FirstName);
        return (List<Claim>)claims.Append(AddApplicationPrivileges(user.RoleClaims.Select(static rc => rc.Name)
            .Concat(user.UserClaims.Select(static uc => uc.Name)).Distinct()));
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void AddClaimIfNotNull(this ICollection<Claim> claims, string claimType, string? value)
    {
        if (value is not null) claims.Add(new Claim(claimType, value));
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">if 'key' is null.</exception>
    /// <exception cref="ArgumentException">If 'expires' &lt;= 'notbefore'.</exception>
    /// <exception cref="EncoderFallbackException">
    ///     A fallback occurred (for more information, see Character Encoding in .NET)
    ///     -and-
    ///     <see cref="EncoderFallback" /> is set to <see cref="EncoderExceptionFallback" />.
    /// </exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static Try<string> GenerateGenieAccessToken(JwtOptions jwtConfig, AppOptions appConfig)
    {
        return () =>
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var exp = GetExpirationDateTime(jwtConfig.TokenLifetime, appConfig.Env);
            var token = new JwtSecurityToken(jwtConfig.Issuer, jwtConfig.Audience, GenerateGenieClaims(), null, exp,
                credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ReturnTypeCanBeEnumerable.Local",
        Justification =
            "Concrete return type is intentional for performance and to avoid interface-based enumeration overhead.")]
    private static List<Claim> GenerateGenieClaims()
    {
        return
        [
            new Claim(ClaimCoreConstants.RoleClaim, GenieIdentityConstants.RoleName),
            new Claim(JwtRegisteredClaimNames.Sub, GenieIdentityConstants.Identifier),
            new Claim(JwtRegisteredClaimNames.Email, GenieIdentityConstants.Email),
            new Claim(JwtRegisteredClaimNames.Name, GenieIdentityConstants.UserName),
            new Claim(JwtRegisteredClaimNames.FamilyName, GenieIdentityConstants.LastName),
            new Claim(JwtRegisteredClaimNames.GivenName, GenieIdentityConstants.FirsName),
            new Claim(ClaimCoreConstants.TokenTypeClaim, TokenTypesIdentityConstants.AccessTokenType),
            new Claim(ClaimCoreConstants.ScopeClaim, GenieIdentityConstants.Scope),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ReturnTypeCanBeEnumerable.Local",
        Justification =
            "Concrete return type is intentional for performance and to avoid interface-based enumeration overhead.")]
    private static List<Claim> AddApplicationPrivileges(IEnumerable<string> privileges)
    {
        return privileges
            .Select(static privilege => new Claim(ClaimCoreConstants.ApplicationPrivilegesClaim, privilege))
            .ToList();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static DateTime GetExpirationDateTime(TimeSpan? tokenLifetime, string env)
    {
        var isDev = env is not Envs.Production;
        if (isDev || tokenLifetime is null)
            // Max duration allowed for JWT token
            return new DateTimeOffset(2038, 1, 19, 3, 14, 7, TimeSpan.Zero).UtcDateTime;

        return DateTime.UtcNow + (TimeSpan)tokenLifetime;
    }
}