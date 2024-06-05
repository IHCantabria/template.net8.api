using template.net8.Api.Domain.DTOs;

namespace template.net8.Api.Contracts;

/// <summary>
///     Command Dummy Create Params Resource
/// </summary>
public sealed partial record CommandDummyCreateParamsResource
{
    /// <summary>
    ///     Convert Resource to Dto
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public static implicit operator CommandDummyCreateParamsDto(CommandDummyCreateParamsResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new CommandDummyCreateParamsDto
        {
            Text = resource.Text
        };
    }

    /// <summary>
    ///     This method converts a CommandDummyCreateParamsResource to a CommandDummyCreateParamsDto
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    public static CommandDummyCreateParamsDto ToCommandDummyCreateParamsDto(
        CommandDummyCreateParamsResource dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return dto;
    }
}