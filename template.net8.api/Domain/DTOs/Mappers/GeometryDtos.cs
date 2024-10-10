using JetBrains.Annotations;
using template.net8.api.Contracts;

namespace template.net8.api.Domain.DTOs;

/// <summary>
///     Extent DTO
/// </summary>
public sealed partial record ExtentDto
{
    /// <summary>
    ///     Convert dto to Resource
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static implicit operator ExtentResource(ExtentDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return new ExtentResource
        {
            LonMax = dto.LonMax,
            LatMax = dto.LatMax,
            LonMin = dto.LonMin,
            LatMin = dto.LatMin
        };
    }

    /// <summary>
    ///     This method converts a ExtentDto to a ExtentResource
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [UsedImplicitly]
    public static ExtentResource ToExtentResource(
        ExtentDto dto)
    {
        return dto;
    }
}

/// <summary>
///     Point DTO
/// </summary>
public sealed partial record PointDto
{
    /// <summary>
    ///     Convert dto to Resource
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static implicit operator PointResource(PointDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return new PointResource
        {
            Uuid = dto.Uuid,
            Lon = dto.Lon,
            Lat = dto.Lat
        };
    }

    /// <summary>
    ///     This method converts a PointDto to a PointResource
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [UsedImplicitly]
    public static PointResource ToPointResource(
        PointDto dto)
    {
        return dto;
    }

    internal static IEnumerable<PointResource> ToCollection(
        IReadOnlyList<PointDto> dtos)
    {
        var resources = new PointResource[dtos.Count];
        for (var i = 0; i < dtos.Count; i++) resources[i] = dtos[i];
        return resources;
    }
}