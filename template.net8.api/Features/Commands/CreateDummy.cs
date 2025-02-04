using System.Net;
using System.Numerics;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using template.net8.api.Domain.DTOs;
using template.net8.api.Domain.Persistence.Models;
using template.net8.api.Localize.Resources;

namespace template.net8.api.Features.Commands;

/// <summary>
///     Command Create Dummy CQRS
/// </summary>
public sealed record CommandCreateDummy(CommandCreateDummyParamsDto CommandParams)
    : IRequest<LanguageExt.Common.Result<Dummy>>,
        IEqualityOperators<CommandCreateDummy, CommandCreateDummy, bool>;

[UsedImplicitly]
internal sealed class CreateDummyHandlerCommand : IRequestHandler<CommandCreateDummy, LanguageExt.Common.Result<Dummy>>
{
    /// <summary>
    ///     Handle the Create Dummy Command request
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<LanguageExt.Common.Result<Dummy>> Handle(CommandCreateDummy request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new LanguageExt.Common.Result<Dummy>(new Dummy
        {
            Key = "1",
            Text = request.CommandParams.Text
        }));
    }
}

/// <summary>
///     Create dummy text validator class to validate the text declared in the Command Create Dummy DTO
///     before
///     processing the request.
/// </summary>
[UsedImplicitly]
public sealed class CreateDummyTextValidator : AbstractValidator<CommandCreateDummy>
{
    /// <summary>
    ///     Create Dummy Handler Validator Constructor to initialize the validation rules for the Create Dummy Command DTO
    ///     class.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public CreateDummyTextValidator(IStringLocalizer<ResourceMain> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);

        RuleFor(x => x.CommandParams.Text).Must(x => !x.IsNullOrEmpty())
            .OverridePropertyName("text")
            .WithMessage(localizer["CreateDummyValidatorTextInvalidMsg"])
            .WithErrorCode(localizer["CreateDummyValidatorTextInvalidCode"])
            .WithState(_ => HttpStatusCode.UnprocessableEntity);
    }
}