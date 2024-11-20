using JetBrains.Annotations;
using template.net8.api.Core.Authorization.Contracts;

namespace template.net8.api.Core.Authorization.DTOs;

/// <summary>
///     Access Token DTO User DTO
/// </summary>
public sealed partial record AccessTokenDto
{
    /// <summary>
    ///     Convert dto to Entity
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static implicit operator AccessTokenResource(AccessTokenDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return new AccessTokenResource
        {
            AccessToken = dto.AccessToken,
            AccessTokenType = AccessTokenType,
            RefreshToken = dto.RefreshToken,
            RefreshTokenType = RefreshTokenType
        };
    }

    /// <summary>
    ///     This method converts a AccessTokenDto to a AccessTokenResource
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [UsedImplicitly]
    public static AccessTokenResource ToAccessTokenResource(
        AccessTokenDto dto)
    {
        return dto;
    }
}

/// <summary>
///     Id Token DTO User DTO
/// </summary>
public sealed partial record IdTokenDto
{
    /// <summary>
    ///     Convert dto to Entity
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static implicit operator IdTokenResource(IdTokenDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return new IdTokenResource
        {
            IdToken = dto.IdToken,
            IdTokenType = TokenType,
            Scopes = dto.Scopes
        };
    }

    /// <summary>
    ///     This method converts a IdTokenDto to a IdTokenResource
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [UsedImplicitly]
    public static IdTokenResource ToIdTokenResource(
        IdTokenDto dto)
    {
        return dto;
    }
}