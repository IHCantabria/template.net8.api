using template.net8.Api.Core.Attributes;
using template.net8.Api.Core.Interfaces;
using template.net8.Api.Logger;

namespace template.net8.api.Services.Base;

/// <summary>
///     Service Base
/// </summary>
[ServiceLifetime(ServiceLifetime.Scoped)]
[CoreLibrary]
public class ServiceBase : IServiceImplementation
{
    internal readonly ILogger Logger;

    /// <summary>
    ///     Service Base Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"></exception>
    protected ServiceBase(ILogger<ServiceBase> logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        Logger.LogServiceBaseInjected(logger.GetType().ToString());
    }
}