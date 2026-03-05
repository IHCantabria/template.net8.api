using template.net8.api.Domain.DTOs;

namespace template.net8.api.Contracts;

public sealed partial record QueryGetUserParamsResource
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static implicit operator QueryGetUserParamsDto(QueryGetUserParamsResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new QueryGetUserParamsDto
        {
            Key = resource.Key
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static QueryGetUserParamsDto ToQueryGetUserParamsDto(
        QueryGetUserParamsResource resource)
    {
        return resource;
    }
}

public sealed partial record QueryLoginUserParamsResource
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static implicit operator QueryLoginUserParamsDto(QueryLoginUserParamsResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new QueryLoginUserParamsDto
        {
            Email = resource.Email,
            Password = resource.Password
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static QueryLoginUserParamsDto ToQueryLoginUserParamsDto(
        QueryLoginUserParamsResource resource)
    {
        return resource;
    }
}