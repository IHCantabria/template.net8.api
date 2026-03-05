using System.Text;
using LanguageExt;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Extensions;
using template.net8.api.Domain.DTOs;
using template.net8.api.Domain.Password;

namespace template.net8.api.Domain.Factory;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class UserFactory
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ResultSuccessInvalidOperationException">
    ///     Result is not a success! Use ExtractException method instead
    ///     and Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractException method
    /// </exception>
    /// <exception cref="ResultFaultedInvalidOperationException">
    ///     Result is not a failure! Use ExtractData method instead and
    ///     Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractData method
    /// </exception>
    /// <exception cref="EncoderFallbackException">
    ///     A fallback occurred (for more information, see Character Encoding in .NET)
    ///     -and-
    ///     <see cref="EncoderFallback" /> is set to <see cref="EncoderExceptionFallback" />.
    /// </exception>
    internal static Try<CreateUserDto> CreateUser(CommandCreateUserParamsDto payload,
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
                return new LanguageExt.Common.Result<CreateUserDto>(result.ExtractException());

            var passwordInfo = result.ExtractData();
            return new CreateUserDto
            {
                Uuid = Guid.NewGuid(),
                Username = payload.Username,
                Email = payload.Email,
                IsDisabled = payload.IsDisabled,
                RoleId = payload.RoleId,
                FirstName = payload.FirstName,
                LastName = payload.LastName,
                PasswordHash = passwordInfo.Item1,
                PasswordSalt = passwordInfo.Item2
            };
        };
    }
}