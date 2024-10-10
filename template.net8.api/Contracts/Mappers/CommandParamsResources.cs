using template.net8.api.Domain.DTOs;

namespace template.net8.api.Contracts;

/// <summary>
///     Command Dummy Create Params Resource
/// </summary>
public sealed partial record CommandCreateDummyParamsResource
{
    /// <summary>
    ///     Convert Resource to Dto
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public static implicit operator CommandCreateDummyParamsDto(CommandCreateDummyParamsResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new CommandCreateDummyParamsDto
        {
            Text = resource.Text
        };
    }

    /// <summary>
    ///     This method converts a CommandCreateDummyParamsResource to a CommandCreateDummyParamsDto
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    public static CommandCreateDummyParamsDto ToCommandCreateDummyParamsDto(
        CommandCreateDummyParamsResource dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return dto;
    }
}