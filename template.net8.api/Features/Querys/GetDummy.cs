using System.Globalization;
using System.Net;
using System.Numerics;
using FluentValidation;
using JetBrains.Annotations;
using LanguageExt.Common;
using MediatR;
using template.net8.api.Domain.DTOs;

namespace template.net8.api.Features.Querys;

/// <summary>
///     Query Get Dummy CQRS
/// </summary>
public sealed record QueryGetDummy(QueryGetDummyParamsDto QueryParams) : IRequest<Result<DummyDto>>,
    IEqualityOperators<QueryGetDummy, QueryGetDummy, bool>;

[UsedImplicitly]
internal sealed class GetDummyHandlerQuery : IRequestHandler<QueryGetDummy, Result<DummyDto>>
{
    /// <summary>
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result<DummyDto>> Handle(QueryGetDummy request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new Result<DummyDto>(new DummyDto { Key = request.QueryParams.Key, Text = "Dummy 1" }));
    }
}

/// <summary>
///     Get dummy key validator class to validate the key declared in the Query Get Dummy DTO before
///     processing the request.
/// </summary>
[UsedImplicitly]
public sealed class GetDummyKeyValidator : AbstractValidator<QueryGetDummy>
{
    private const string DummyNotFoundMsg =
        "The dummy key declared in your request doesn't match any dummy in the system.";

    /// <summary>
    ///     Get project role validator constructor to initialize the validation rules for the key in
    ///     the
    ///     Get Project Query DTO class.
    /// </summary>
    public GetDummyKeyValidator()
    {
        RuleFor(x => x.QueryParams.Key)
            .Must(ValidateDummyKey)
            .OverridePropertyName("project-key")
            .WithMessage(DummyNotFoundMsg)
            .WithErrorCode(StatusCodes.Status404NotFound.ToString(CultureInfo.InvariantCulture))
            .WithState(_ => HttpStatusCode.NotFound);
    }

    private static bool ValidateDummyKey(string key)
    {
        return key == "1";
    }
}