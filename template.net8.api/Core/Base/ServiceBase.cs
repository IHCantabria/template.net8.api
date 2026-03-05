using System.Diagnostics.CodeAnalysis;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Interfaces;
using template.net8.api.Logger;

namespace template.net8.api.Core.Base;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[ServiceLifetime(ServiceLifetime.Scoped)]
[SuppressMessage(
    "ReSharper",
    "MemberCanBePrivate.Global",
    Justification = "Members are intended to be accessed by derived classes.")]
internal class ServiceBase : IServiceImplementation
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal readonly ILogger Logger;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    protected ServiceBase(ILogger<ServiceBase> logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (Logger.IsEnabled(LogLevel.Debug)) Logger.LogServiceBaseInjected(logger.GetType().ToString());
    }
}