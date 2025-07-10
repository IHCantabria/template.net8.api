using System.Globalization;
using MediatR.Pipeline;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Extensions;
using template.net8.api.Domain.Persistence.Models;
using template.net8.api.Features.Commands;
using template.net8.api.Hubs.Dummy;
using template.net8.api.Hubs.Dummy.Contracts;
using template.net8.api.Hubs.Dummy.Interfaces;
using template.net8.api.Localize.Resources;

namespace template.net8.api.Behaviors;

internal sealed class NewDummyProcessors(
    IHubContext<DummyHub, IDummyHub> hubContext,
    IStringLocalizer<ResourceMain> localizer)
    : IRequestPostProcessor<CommandCreateDummy, LanguageExt.Common.Result<Dummy>>

{
    private readonly IHubContext<DummyHub, IDummyHub> _hubContext =
        hubContext ?? throw new ArgumentNullException(nameof(hubContext));

    private readonly IStringLocalizer<ResourceMain> _localizer =
        localizer ?? throw new ArgumentNullException(nameof(localizer));

    /// <summary>
    ///     Process the request and send a notification to all clients connected to the hub
    /// </summary>
    /// <exception cref="ResultSuccessInvalidOperationException">
    ///     Result is not a success! Use ExtractException method instead and Check the
    ///     state of Result with IsSuccess or IsFaulted before use this method or ExtractException method
    /// </exception>
    public async Task Process(CommandCreateDummy request, LanguageExt.Common.Result<Dummy> response,
        CancellationToken cancellationToken)
    {
        if (response.IsFaulted) return;

        await SendEventNotificationAsync(response.ExtractData()).ConfigureAwait(false);
    }

    private Task SendEventNotificationAsync(Dummy data)
    {
        return _hubContext.Clients.All.NewDummy(new DummyHubNewDummyMessageResource
        {
            Message = _localizer["CreateDummyValidatorTextInvalidMsg"], // TODO:FIX
            DummyKey = data.Key.ToString(CultureInfo.InvariantCulture)
        });
    }
}