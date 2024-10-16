using System.Net;
using System.Numerics;
using FluentValidation;
using JetBrains.Annotations;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Localization;
using template.net8.api.Domain.DTOs;
using template.net8.api.Localize.Resources;

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
    /// <summary>
    ///     Get project role validator constructor to initialize the validation rules for the key in
    ///     the
    ///     Get Project Query DTO class.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public GetDummyKeyValidator(IStringLocalizer<ResourceMain> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);

        RuleFor(x => x.QueryParams.Key)
            .Must(ValidateDummyKey)
            .OverridePropertyName("project-key")
            .WithMessage(localizer["GetDummyValidatorNotFoundMsg"])
            .WithErrorCode(localizer["GetDummyValidatorNotFoundCode"])
            .WithState(_ => HttpStatusCode.NotFound);
    }

    private static bool ValidateDummyKey(string key)
    {
        return key == "1";
    }
}