using MediatR;
using Microsoft.AspNetCore.Mvc;
using template.net8.api.Core.Attributes;
using template.net8.api.Logger;

namespace template.net8.api.Controllers;

/// <summary>
///     My Controller Base
/// </summary>
[CoreLibrary]
public class MyControllerBase : ControllerBase
{
    internal readonly ILogger Logger;

    internal readonly IMediator Mediator;

    /// <summary>
    ///     My Controller Base Constructor
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"></exception>
    protected MyControllerBase(IMediator mediator,
        ILogger<MyControllerBase> logger)
    {
        Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        Logger.LogControllerBaseInjected(logger.GetType().ToString());
    }
}