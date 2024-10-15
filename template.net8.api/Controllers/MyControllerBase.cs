using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using template.net8.api.Core.Attributes;
using template.net8.api.Localize.Resources;
using template.net8.api.Logger;

namespace template.net8.api.Controllers;

/// <summary>
///     My Controller Base
/// </summary>
[CoreLibrary]
public class MyControllerBase : ControllerBase
{
    internal readonly IStringLocalizer<Resource> Localizer;
    internal readonly ILogger Logger;

    internal readonly IMediator Mediator;

    /// <summary>
    ///     My Controller Base Constructor
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="localizer"></param>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"></exception>
    protected MyControllerBase(IMediator mediator, IStringLocalizer<Resource> localizer,
        ILogger<MyControllerBase> logger)
    {
        Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        Localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        Logger.LogControllerBaseInjected(logger.GetType().ToString());
    }
}