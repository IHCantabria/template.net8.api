using System.Globalization;
using MediatR.Pipeline;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Extensions;
using template.net8.api.Features.Commands;
using template.net8.api.Hubs.User;
using template.net8.api.Hubs.User.Contracts;
using template.net8.api.Hubs.User.Interfaces;
using template.net8.api.Localize.Resources;
using template.net8.api.Persistence.Models;

namespace template.net8.api.Behaviors.Users;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class CreateUserProcessor(
    IHubContext<UserHub, IUserHub> hubContext,
    IStringLocalizer<ResourceMain> localizer)
    : IRequestPostProcessor<CommandCreateUser, LanguageExt.Common.Result<User>>

{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IHubContext<UserHub, IUserHub> _hubContext =
        hubContext ?? throw new ArgumentNullException(nameof(hubContext));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IStringLocalizer<ResourceMain> _localizer =
        localizer ?? throw new ArgumentNullException(nameof(localizer));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ResultSuccessInvalidOperationException">
    ///     Result is not a success! Use ExtractException method instead and Check the
    ///     state of Result with IsSuccess or IsFaulted before use this method or ExtractException method
    /// </exception>
    public async Task Process(CommandCreateUser request, LanguageExt.Common.Result<User> response,
        CancellationToken cancellationToken)
    {
        if (response.IsFaulted) return;

        await SendEventNotificationAsync(response.ExtractData()).ConfigureAwait(false);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private Task SendEventNotificationAsync(User data)
    {
        return _hubContext.Clients.All.CreatedUser(new UserHubCreatedUserMessageResource
        {
            Message = _localizer["UserHubCreatedUserMsg"],
            Uuid = data.Uuid.ToString(CultureInfo.InvariantCulture.ToString())
        });
    }
}