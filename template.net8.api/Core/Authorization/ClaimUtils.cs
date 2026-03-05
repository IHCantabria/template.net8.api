using System.Security.Claims;
using LanguageExt;
using template.net8.api.Core.Exceptions;

namespace template.net8.api.Core.Authorization;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class ClaimUtils
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static Try<Guid> GetUserGuid(ClaimsPrincipal claimsPrincipal)
    {
        return () =>
        {
            var stringUuid = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = Guid.TryParse(stringUuid, out var uuid);
            return result
                ? uuid
                : new LanguageExt.Common.Result<Guid>(new UnauthorizedException(
                    "You dont have a valid uuid in your Access Token"));
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static string? GetRoleName(ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(ClaimCoreConstants.RoleClaim);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static string? GetUserUuid(ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}