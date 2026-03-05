using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using template.net8.api.Localize.Resources;
using template.net8.api.Logger;

namespace template.net8.api.Controllers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Controllers must remain public to allow OpenAPI discovery and correct API exposure.")]
[SuppressMessage(
    "ReSharper",
    "MemberCanBePrivate.Global",
    Justification = "Members are intended to be accessed by derived classes.")]
public abstract class MyControllerBase : ControllerBase
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal readonly IStringLocalizer<ResourceMain> Localizer;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal readonly ILogger Logger;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal readonly IMediator Mediator;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    protected MyControllerBase(IMediator mediator, IStringLocalizer<ResourceMain> localizer,
        ILogger<MyControllerBase> logger)
    {
        Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        Localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        if (Logger.IsEnabled(LogLevel.Debug)) Logger.LogControllerBaseInjected(logger.GetType().ToString());
    }
}