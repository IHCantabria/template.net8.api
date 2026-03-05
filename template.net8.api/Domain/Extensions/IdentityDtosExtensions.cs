using System.Security.Claims;
using template.net8.api.Core.Authorization;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Extensions;
using template.net8.api.Core.Interfaces;
using template.net8.api.Domain.DTOs;
using template.net8.api.Domain.Interfaces;

namespace template.net8.api.Domain.Extensions;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class IdentityExtensions
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ResultSuccessInvalidOperationException">
    ///     Result is not a success! Use ExtractException method instead
    ///     and Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractException method
    /// </exception>
    internal static T AddIdentifier<T>(this T dto, ClaimsPrincipal claimsPrincipal) where T : IIdentity, IDto
    {
        var userGuidResult = ClaimUtils.GetUserGuid(claimsPrincipal).Try();
        var userRoleName = ClaimUtils.GetRoleName(claimsPrincipal);
        var userIdentifier = ClaimUtils.GetUserUuid(claimsPrincipal);
        dto.Identity = new IdentityDto(userGuidResult.IsSuccess ? userGuidResult.ExtractData() : null,
            userRoleName, userIdentifier);
        return dto;
    }
}