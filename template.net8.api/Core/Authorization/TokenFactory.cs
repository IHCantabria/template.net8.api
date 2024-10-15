using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.IdentityModel.Tokens;
using template.net8.api.Business.Exceptions;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Authorization.Dtos;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Extensions;
using template.net8.api.Settings;
using template.net8.api.Settings.Options;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace template.net8.api.Core.Authorization;

[CoreLibrary]
internal static class TokenFactory
{
    internal static IdTokenDto GenerateIdTokenDto(UserIdTokenWithScopesBaseDto user, JwtOptions config)
    {
        return new IdTokenDto
        {
            IdToken = GenerateIdToken(user, config),
            Scopes = user.Scopes
        };
    }

    internal static Try<AccessTokenDto> GenerateAccessTokenDto(UserAccessTokenWithScopeBaseDto user, JwtOptions config)
    {
        return () =>
        {
            if (user.Scope is null)
                return new Result<AccessTokenDto>(
                    new UnauthorizedException(
                        "You dont have a valid scope assigned to your user so you dont access to the system."));

            return new AccessTokenDto
            {
                AccessToken = GenerateAccessToken(user, config),
                RefreshToken = GenerateAccessToken(user, config)
            };
        };
    }

    /// <exception cref="ResultFaultedInvalidOperationException">
    ///     Result is not a failure! Use ExtractData method instead and
    ///     Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractData method
    /// </exception>
    /// <exception cref="ResultSuccessInvalidOperationException">
    ///     Result is not a success! Use ExtractException method instead
    ///     and Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractException method
    /// </exception>
    /// <exception cref="EncoderFallbackException">
    ///     A fallback occurred (for more information, see Character Encoding in .NET)
    ///     -and-
    ///     <see cref="EncoderFallback" /> is set to <see cref="EncoderExceptionFallback" />.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>s</name>
    ///     </paramref>
    ///     is <see langword="null" />.
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
    internal static Try<AccessTokenDto> GenerateGenieAccessTokenDto(JwtOptions config)
    {
        return () =>
        {
            var result = GenerateGenieAccessToken(config).Try();
            if (result.IsFaulted)
                return new Result<AccessTokenDto>(result.ExtractException());

            var accessToken = result.ExtractData();
            return new AccessTokenDto
            {
                AccessToken = accessToken,
                RefreshToken = accessToken
            };
        };
    }

    private static string GenerateIdToken(UserIdTokenWithScopesBaseDto user, JwtOptions config)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var exp = GetExpirationDateTime(config.TokenLifetime);
        var claims = AddUserClaims(user);
        var token = new JwtSecurityToken(config.Issuer, config.Audience, claims, null, exp, credentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private static string GenerateAccessToken(UserAccessTokenWithScopeBaseDto user, JwtOptions config)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = AddUserClaims(user);
        var exp = GetExpirationDateTime(config.TokenLifetime);
        var token = new JwtSecurityToken(config.Issuer, config.Audience, claims, null, exp, credentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private static List<Claim> AddUserClaims(UserIdTokenWithScopesBaseDto user)
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
        //claims.AddClaimIfNotNull(JwtRegisteredClaimNames.FamilyName, user.LastName);
        //claims.AddClaimIfNotNull(JwtRegisteredClaimNames.GivenName, user.FirstName);
        return claims;
    }

    private static List<Claim> AddUserClaims(UserAccessTokenWithScopeBaseDto user)
    {
        var claims = new Collection<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Uuid.ToString()),
            new(ClaimCoreConstants.ScopeClaim, user.Scope!.Name),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Name, user.Username),
            new(ClaimCoreConstants.TokenTypeClaim, TokenTypesIdentityConstants.AccessTokenType),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddClaimIfNotNull(ClaimCoreConstants.RoleClaim, user.RoleName);
        //claims.AddClaimIfNotNull(JwtRegisteredClaimNames.FamilyName, user.LastName);
        //claims.AddClaimIfNotNull(JwtRegisteredClaimNames.GivenName, user.FirstName);
        return claims.Append(AddClimportPrivileges(user.Scope.Claims.Select(c => c.Name))).ToList();
    }

    private static void AddClaimIfNotNull(this ICollection<Claim> claims, string claimType, string? value)
    {
        if (value is not null) claims.Add(new Claim(claimType, value));
    }

    //TODO: TODO
    //private static string GenerateRefreshToken(UserAccessTokenWithScopeDto user, JwtOptions config)
    //{
    //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret));
    //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    //    var claims = AddUserClaims(user);
    //    var exp = GetExpirationDateTime(config.TokenLifetime);
    //    var token = new JwtSecurityToken(config.Issuer, config.Audience, claims, null, exp, credentials);
    //    var tokenHandler = new JwtSecurityTokenHandler();
    //    return tokenHandler.WriteToken(token);
    //}

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
    internal static Try<string> GenerateGenieAccessToken(JwtOptions config)
    {
        return () =>
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var exp = GetExpirationDateTime(config.TokenLifetime);
            var token = new JwtSecurityToken(config.Issuer, config.Audience, GenerateGenieClaims(), null, exp,
                credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        };
    }

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

    private static List<Claim> AddClimportPrivileges(IEnumerable<string> privileges)
    {
        return privileges.Select(privilege => new Claim(ClaimCoreConstants.ApplicationPrivilegesClaim, privilege))
            .ToList();
    }

    private static DateTime GetExpirationDateTime(TimeSpan? tokenLifetime)
    {
        var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") is not Envs.Production;
        if (isDev || !tokenLifetime.HasValue)
            // Max duration allowed for JWT token
            return new DateTimeOffset(2038, 1, 19, 3, 14, 7, TimeSpan.Zero).DateTime;

        return DateTime.UtcNow.Add(tokenLifetime.Value);
    }
}