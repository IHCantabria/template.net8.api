using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using template.net8.api.Logger;

namespace template.net8.api.Core.Base;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal class StatefulRepositoryBase : RepositoryBase
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal readonly DbContext Context;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    protected StatefulRepositoryBase(DbContext context, ILogger<StatefulRepositoryBase> logger) :
        base(logger)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal class StatelessRepositoryBase : RepositoryBase
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    protected StatelessRepositoryBase(ILogger<StatelessRepositoryBase> logger) : base(logger)
    {
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "ReSharper",
    "MemberCanBePrivate.Global",
    Justification = "Members are intended to be accessed by derived classes.")]
internal class RepositoryBase
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal readonly ILogger Logger;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    protected RepositoryBase(ILogger<RepositoryBase> logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (Logger.IsEnabled(LogLevel.Debug)) Logger.LogRepositoryBaseInjected(logger.GetType().ToString());
    }
}