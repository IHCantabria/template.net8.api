using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Interfaces;

/// <summary>
///     Interface for Connections Options
/// </summary>
[CoreLibrary]
public interface IConnectionsOptions
{
    /// <summary>
    ///     AppSettings key for the Connections Options
    /// </summary>
    static readonly string Connections = nameof(Connections);
}