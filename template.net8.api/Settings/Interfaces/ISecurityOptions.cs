using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Interfaces;

/// <summary>
///     Interface for Security Options
/// </summary>
[CoreLibrary]
public interface ISecurityOptions
{
    /// <summary>
    ///     AppSettings key for the Security Options
    /// </summary>
    static readonly string Security = nameof(Security);
}