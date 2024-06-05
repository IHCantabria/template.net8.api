using System.Globalization;
using System.Net;
using FluentValidation;
using JetBrains.Annotations;
using LanguageExt.Common;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using template.net8.Api.Domain.DTOs;

namespace template.net8.Api.Features.Commands;

/// <summary>
///     Create Dummy CQRS Command
/// </summary>
[UsedImplicitly]
public sealed record CreateDummyCommand(CommandDummyCreateParamsDto CommandParams) : IRequest<Result<DummyDto>>;

/// <summary>
///     Create Dummy CQRS Command Handler
/// </summary>
[UsedImplicitly]
public sealed class CreateDummyHandlerCommand : IRequestHandler<CreateDummyCommand, Result<DummyDto>>
{
    /// <summary>
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref /> is <see langword="null" />.</exception>
    public Task<Result<DummyDto>> Handle(CreateDummyCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return Task.FromResult(new Result<DummyDto>(new DummyDto
        {
            Text = request.CommandParams.Text
        }));
    }
}

/// <summary>
///     Create Dummy Handler Validator class to validate the  Create Dummy Command DTO before processing the request.
///     This class is used to validate the Create Dummy Command DTO before processing the request. It implements the
///     AbstractValidator class from the FluentValidation library.
/// </summary>
[UsedImplicitly]
public sealed class CreateDummyHandlerValidator : AbstractValidator<CreateDummyCommand>
{
    private const string MsgText =
        "You must specify a valid text";

    /// <summary>
    ///     Create Dummy Handler Validator Constructor to initialize the validation rules for the Create Dummy Command DTO
    ///     class.
    /// </summary>
    public CreateDummyHandlerValidator()
    {
        RuleFor(x => x.CommandParams.Text).Must(x => !x.IsNullOrEmpty())
            .OverridePropertyName("text")
            .WithMessage(MsgText.ToString(CultureInfo.InvariantCulture))
            .WithErrorCode(StatusCodes.Status422UnprocessableEntity.ToString(CultureInfo.InvariantCulture))
            .WithState(_ => HttpStatusCode.UnprocessableEntity);
    }
}