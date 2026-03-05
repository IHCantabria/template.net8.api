using template.net8.api.Domain.DTOs;
using template.net8.api.Features.Commands;
using template.net8.api.Features.Querys;
using template.net8.api.Persistence.Models;
using template.net8.api.Settings.Extensions;

namespace template.net8.api.Features.Extensions;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class MediatRServiceConfigurationExtensions
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void ConfigureValidations(this MediatRServiceConfiguration config)
    {
        config.AddValidation<CommandCreateUser, User>();
        config.AddValidation<CommandDeleteUser, User>();
        config.AddValidation<CommandUpdateUser, User>();
        config.AddValidation<CommandDisableUser, User>();
        config.AddValidation<CommandEnableUser, User>();
        config.AddValidation<CommandResetUserPassword, UserResetedPasswordDto>();

        config.AddValidation<QueryAccessUser, AccessTokenDto>();
        config.AddValidation<QueryGetUser, UserDto>();
        config.AddValidation<QueryGetUsers, IEnumerable<UserDto>>();
        config.AddValidation<QueryLoginUser, IdTokenDto>();
    }
}