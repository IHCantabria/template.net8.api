using System.Globalization;
using System.Net;
using System.Numerics;
using FluentValidation;
using JetBrains.Annotations;
using LanguageExt.Common;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using template.net8.api.Domain.DTOs;
using template.net8.api.Domain.Persistence.Models;

namespace template.net8.api.Features.Commands;

/// <summary>
///     Command Create Dummy CQRS
/// </summary>
public sealed record CommandCreateDummy(CommandCreateDummyParamsDto CommandParams) : IRequest<Result<Dummy>>,
    IEqualityOperators<CommandCreateDummy, CommandCreateDummy, bool>;

[UsedImplicitly]
internal sealed class CreateDummyHandlerCommand : IRequestHandler<CommandCreateDummy, Result<Dummy>>
{
    /// <summary>
    ///     Handle the Create Dummy Command request
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result<Dummy>> Handle(CommandCreateDummy request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new Result<Dummy>(new Dummy
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
    private const string Msg =
        "You must specify a valid text";

    /// <summary>
    ///     Create Dummy Handler Validator Constructor to initialize the validation rules for the Create Dummy Command DTO
    ///     class.
    /// </summary>
    public CreateDummyTextValidator()
    {
        RuleFor(x => x.CommandParams.Text).Must(x => !x.IsNullOrEmpty())
            .OverridePropertyName("text")
            .WithMessage(Msg.ToString(CultureInfo.InvariantCulture))
            .WithErrorCode(StatusCodes.Status422UnprocessableEntity.ToString(CultureInfo.InvariantCulture))
            .WithState(_ => HttpStatusCode.UnprocessableEntity);
    }
}