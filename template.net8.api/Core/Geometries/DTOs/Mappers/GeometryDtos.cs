using template.net8.api.Core.Geometries.Contracts;

namespace template.net8.api.Core.Geometries.DTOs;

internal sealed partial record ExtentDto
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
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
}

internal sealed partial record PointDto
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
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
}