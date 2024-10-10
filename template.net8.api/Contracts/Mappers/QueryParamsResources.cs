using template.net8.api.Domain.DTOs;

namespace template.net8.api.Contracts;

/// <summary>
///     Query Get Dummy Params Resource
/// </summary>
public sealed partial record QueryGetDummyParamsResource
{
    /// <summary>
    ///     Convert Resource to Dto
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public static implicit operator QueryGetDummyParamsDto(QueryGetDummyParamsResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new QueryGetDummyParamsDto
        {
            Key = resource.Key
        };
    }

    /// <summary>
    ///     This method converts a QueryGetDummyParamsResource to a QueryGetDummyParamsDto
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    public static QueryGetDummyParamsDto ToQueryGetDummyParamsDto(
        QueryGetDummyParamsResource dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return dto;
    }
}