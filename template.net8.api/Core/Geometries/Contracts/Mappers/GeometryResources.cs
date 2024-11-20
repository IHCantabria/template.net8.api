using template.net8.api.Core.Geometries.DTOs;

namespace template.net8.api.Core.Geometries.Contracts;

/// <summary>
///     Create Extent Resource
/// </summary>
public sealed partial record CreateExtentResource
{
    /// <summary>
    ///     Convert Resource to Dto
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public static implicit operator CreateExtentDto(CreateExtentResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new CreateExtentDto
        {
            LatMin = resource.LatMin,
            LatMax = resource.LatMax,
            LonMin = resource.LonMin,
            LonMax = resource.LonMax
        };
    }

    /// <summary>
    ///     This method converts a CreateExtentResource to a CreateExtentDto
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public static CreateExtentDto ToCreateExtentDto(
        CreateExtentResource resource)
    {
        return resource;
    }

    internal static IEnumerable<CreateExtentDto> ToCollection(
        IReadOnlyList<CreateExtentResource> resources)
    {
        var dtos = new CreateExtentDto[resources.Count];
        for (var i = 0; i < resources.Count; i++) dtos[i] = resources[i];
        return dtos;
    }
}

/// <summary>
///     Create Point Resource
/// </summary>
public sealed partial record CreatePointResource
{
    /// <summary>
    ///     Convert Resource to Dto
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public static implicit operator CreatePointDto(CreatePointResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new CreatePointDto
        {
            Lat = resource.Lat, Lon = resource.Lon
        };
    }

    /// <summary>
    ///     This method converts a CreatePointResource to a CreatePointDto
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public static CreatePointDto ToCreatePointDto(
        CreatePointResource resource)
    {
        return resource;
    }

    internal static IEnumerable<CreatePointDto> ToCollection(
        IReadOnlyList<CreatePointResource> resources)
    {
        var dtos = new CreatePointDto[resources.Count];
        for (var i = 0; i < resources.Count; i++) dtos[i] = resources[i];
        return dtos;
    }
}