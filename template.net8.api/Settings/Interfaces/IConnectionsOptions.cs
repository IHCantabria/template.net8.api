using template.net8.Api.Core.Attributes;

namespace template.net8.Api.Settings.Interfaces;

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