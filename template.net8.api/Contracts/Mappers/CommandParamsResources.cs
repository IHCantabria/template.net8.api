using template.net8.api.Domain.DTOs;

namespace template.net8.api.Contracts;

public sealed partial record CommandCreateUserParamsResource
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static implicit operator CommandCreateUserParamsDto(CommandCreateUserParamsResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new CommandCreateUserParamsDto
        {
            Username = resource.Username,
            Email = resource.Email,
            IsDisabled = resource.IsDisabled,
            FirstName = resource.FirstName,
            LastName = resource.LastName,
            Password = resource.Password,
            ConfirmPassword = resource.ConfirmPassword,
            RoleId = resource.RoleId,
            Identity = new IdentityDto()
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static CommandCreateUserParamsDto ToCommandCreateUserParamsDto(
        CommandCreateUserParamsResource resource)
    {
        return resource;
    }
}

public sealed partial record CommandUpdateUserParamsResource
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static implicit operator CommandUpdateUserParamsDto(CommandUpdateUserParamsResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new CommandUpdateUserParamsDto
        {
            Key = resource.Key,
            Username = resource.Body.Username,
            Email = resource.Body.Email,
            IsDisabled = resource.Body.IsDisabled,
            RoleId = resource.Body.RoleId,
            FirstName = resource.Body.FirstName,
            LastName = resource.Body.LastName,
            Identity = new IdentityDto()
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static CommandUpdateUserParamsDto ToCommandUpdateUserParamsDto(
        CommandUpdateUserParamsResource resource)
    {
        return resource;
    }
}

public sealed partial record CommandDisableUserParamsResource
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static implicit operator CommandDisableUserParamsDto(CommandDisableUserParamsResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new CommandDisableUserParamsDto
        {
            Key = resource.Key,
            Identity = new IdentityDto()
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static CommandDisableUserParamsDto ToCommandDisableUserParamsDto(
        CommandDisableUserParamsResource resource)
    {
        return resource;
    }
}

public sealed partial record CommandEnableUserParamsResource
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static implicit operator CommandEnableUserParamsDto(CommandEnableUserParamsResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new CommandEnableUserParamsDto
        {
            Key = resource.Key,
            Identity = new IdentityDto()
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static CommandEnableUserParamsDto ToCommandEnableUserParamsDto(
        CommandEnableUserParamsResource resource)
    {
        return resource;
    }
}

public sealed partial record CommandDeleteUserParamsResource
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <returns></returns>
    public static implicit operator CommandDeleteUserParamsDto(CommandDeleteUserParamsResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new CommandDeleteUserParamsDto
        {
            Key = resource.Key,
            Identity = new IdentityDto()
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static CommandDeleteUserParamsDto ToCommandDeleteUserParamsDto(
        CommandDeleteUserParamsResource resource)
    {
        return resource;
    }
}

public sealed partial record CommandResetUserPasswordParamsResource
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static implicit operator CommandResetUserPasswordParamsDto(CommandResetUserPasswordParamsResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new CommandResetUserPasswordParamsDto
        {
            Key = resource.Key,
            Password = resource.Body.Password,
            ConfirmPassword = resource.Body.ConfirmPassword,
            Identity = new IdentityDto()
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static CommandResetUserPasswordParamsDto ToCommandResetUserPasswordParamsDto(
        CommandResetUserPasswordParamsResource resource)
    {
        return resource;
    }
}