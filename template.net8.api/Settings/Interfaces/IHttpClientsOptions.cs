using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Interfaces;

/// <summary>
///     Interface for Http Clients Options
/// </summary>
[CoreLibrary]
public interface IHttpClientsOptions : IConnectionsOptions
{
    /// <summary>
    ///     Uri Base for the Http Client
    /// </summary>
    Uri UriBase { get; init; }

    /// <summary>
    ///     Timeout for the Http Client
    /// </summary>
    TimeSpan Timeout { get; init; }
}