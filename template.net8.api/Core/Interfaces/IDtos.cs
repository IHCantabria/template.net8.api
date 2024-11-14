using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Interfaces;

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