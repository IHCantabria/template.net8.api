using System.Text;
using LanguageExt;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Extensions;
using template.net8.api.Domain.DTOs;
using template.net8.api.Domain.Password;

namespace template.net8.api.Persistence.Models.Extensions;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class UserExtensions
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void UpdateUser(this User entity, CommandUpdateUserParamsDto payload)
    {
        entity.Email = payload.Email ?? entity.Email;
        entity.FirstName = payload.FirstName ?? entity.FirstName;
        entity.LastName = payload.LastName ?? entity.LastName;
        entity.Username = payload.Username ?? entity.Username;
        entity.RoleId = payload.RoleId ?? entity.RoleId;
        entity.IsDisabled = payload.IsDisabled ?? entity.IsDisabled;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void DisableUser(this User entity)
    {
        entity.IsDisabled = true;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static void EnableUser(this User entity)
    {
        entity.IsDisabled = false;
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
    /// <exception cref="EncoderFallbackException">
    ///     A fallback occurred (for more information, see Character Encoding in .NET)
    ///     -and-
    ///     <see cref="EncoderFallback" /> is set to <see cref="EncoderExceptionFallback" />.
    /// </exception>
    internal static Try<bool> UpdateUserPassword(this User entity, CommandResetUserPasswordParamsDto payload,
        string pepper)
    {
        return () =>
        {
            var passwordPayload = new CreateUserPasswordDto
            {
                Password = payload.Password,
                Pepper = pepper
            };
            var result = PasswordHasher.HashPasword(passwordPayload).Try();
            if (result.IsFaulted)
                return new LanguageExt.Common.Result<bool>(result.ExtractException());

            var passwordInfo = result.ExtractData();
            entity.PasswordHash = passwordInfo.Item1;
            entity.PasswordSalt = passwordInfo.Item2;
            return true;
        };
    }
}