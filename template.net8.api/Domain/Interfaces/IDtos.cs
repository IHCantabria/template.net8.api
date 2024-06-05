using template.net8.Api.Core.Attributes;

namespace template.net8.Api.Domain.Interfaces;

/// <summary>
///     This interfaces is intended to mark DTO records.
/// </summary>
[CoreLibrary]
public interface IDto
{
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public bool Check()
    {
        return true;
    }
}