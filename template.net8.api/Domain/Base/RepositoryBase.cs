using template.net8.api.Core.Attributes;
using template.net8.api.Logger;

namespace template.net8.api.Domain.Base;

/// <summary>
///     Repository Base
/// </summary>
[CoreLibrary]
public class RepositoryBase
{
    internal readonly ILogger Logger;

    /// <summary>
    ///     Repository Base Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    protected RepositoryBase(ILogger<RepositoryBase> logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        Logger.LogRepositoryBaseInjected(logger.GetType().ToString());
    }
}