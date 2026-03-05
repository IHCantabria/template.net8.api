using template.net8.api.Core.Geometries.DTOs;

namespace template.net8.api.Core.Geometries.Contracts;

public sealed partial record CreateExtentResource
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
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
    ///     ADD DOCUMENTATION
    /// </summary>
    public static CreateExtentDto ToCreateExtentDto(
        CreateExtentResource resource)
    {
        return resource;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static IEnumerable<CreateExtentDto> ToCollection(
        IReadOnlyList<CreateExtentResource> resources)
    {
        var dtos = new CreateExtentDto[resources.Count];
        for (var i = 0; i < resources.Count; i++) dtos[i] = resources[i];
        return dtos;
    }
}

public sealed partial record CreatePointResource
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static implicit operator CreatePointDto(CreatePointResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new CreatePointDto
        {
            Lat = resource.Lat, Lon = resource.Lon
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static CreatePointDto ToCreatePointDto(
        CreatePointResource resource)
    {
        return resource;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static IEnumerable<CreatePointDto> ToCollection(
        IReadOnlyList<CreatePointResource> resources)
    {
        var dtos = new CreatePointDto[resources.Count];
        for (var i = 0; i < resources.Count; i++) dtos[i] = resources[i];
        return dtos;
    }
}