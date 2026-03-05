using LanguageExt;
using template.net8.api.Domain.DTOs;

namespace template.net8.api.Domain.Password;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class PasswordUtils
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static Try<bool> VerifyUserCredentials(UserCredentialsDto payload, string password, string pepper)
    {
        return () =>
        {
            var verifyPayload = new VerifyUserPasswordDto
            {
                Password = password,
                Pepper = pepper,
                Salt = payload.PasswordSalt,
                Hash = payload.PasswordHash
            };
            return PasswordHasher.VerifyPassword(verifyPayload).Try();
        };
    }
}