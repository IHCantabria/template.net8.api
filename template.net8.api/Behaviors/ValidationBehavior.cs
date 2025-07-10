using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Localization;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Parallel;
using template.net8.api.Localize.Resources;

namespace template.net8.api.Behaviors;

[CoreLibrary]
internal sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    IStringLocalizer<ResourceMain> localizer)
    : IPipelineBehavior<TRequest, LanguageExt.Common.Result<TResponse>>
    where TRequest : notnull
{
    private readonly IStringLocalizer<ResourceMain> _localizer =
        localizer ?? throw new ArgumentNullException(nameof(localizer));

    private readonly IEnumerable<IValidator<TRequest>> _validators =
        validators ?? throw new ArgumentNullException(nameof(validators));

    public Task<LanguageExt.Common.Result<TResponse>> Handle(TRequest request,
        RequestHandlerDelegate<LanguageExt.Common.Result<TResponse>> next,
        CancellationToken cancellationToken)
    {
        return BehaviorLogicAsync(request, next, cancellationToken);
    }

    private async Task<LanguageExt.Common.Result<TResponse>> BehaviorLogicAsync(TRequest request,
        RequestHandlerDelegate<LanguageExt.Common.Result<TResponse>> next,
        CancellationToken cancellationToken = default)
    {
        var failures = await ValidateRequestAsync(request, cancellationToken).ConfigureAwait(false);
        return failures.Count > 0
            ? new LanguageExt.Common.Result<TResponse>(new ValidationException(failures))
            : await next(cancellationToken).ConfigureAwait(false);
    }

    private async Task<ICollection<ValidationFailure>> ValidateRequestAsync(TRequest request,
        CancellationToken cancellationToken = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var results = await ValidateValidatorsAsync(request, cts).ConfigureAwait(false);
        return AggregateValidationResults(results).ToList();
    }

    private async Task<IEnumerable<ValidationResult>> ValidateValidatorsAsync(TRequest request,
        CancellationTokenSource cts)
    {
        var tasks = _validators.Select(validator =>
        {
            return Task.Run(() => validator.ValidateAsync(new ValidationContext<TRequest>(request), cts.Token),
                cts.Token);
        });
        var result = await ParallelUtils.ExecuteDependentInParallelAsync(tasks, cts).ConfigureAwait(false);
        var validateValidatorsAsync = result.ToList();
        if (validateValidatorsAsync.Length() != _validators.Length())
            throw new ValidationException(_localizer["GenericValidatorError"]);

        return validateValidatorsAsync;
    }

    private static IEnumerable<ValidationFailure> AggregateValidationResults(IEnumerable<ValidationResult> results)
    {
        return results
            .SelectMany(r => r.Errors).Distinct()
            .Where(f => f is not null);
    }
}