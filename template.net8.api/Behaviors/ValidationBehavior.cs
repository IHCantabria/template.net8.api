using FluentValidation;
using FluentValidation.Results;
using JetBrains.Annotations;
using LanguageExt.Common;
using MediatR;

namespace template.net8.api.Behaviors;

[UsedImplicitly]
internal sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, Result<TResponse>>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators =
        validators ?? throw new ArgumentNullException(nameof(validators));

    public Task<Result<TResponse>> Handle(TRequest request, RequestHandlerDelegate<Result<TResponse>> next,
        CancellationToken cancellationToken)
    {
        return BehaviorLogicAsync(request, next, cancellationToken);
    }

    private async Task<Result<TResponse>> BehaviorLogicAsync(TRequest request,
        RequestHandlerDelegate<Result<TResponse>> next,
        CancellationToken cancellationToken = default)
    {
        var failures = await ValidateRequestAsync(request, cancellationToken).ConfigureAwait(false);
        return failures.Count > 0
            ? new Result<TResponse>(new ValidationException(failures))
            : await next().ConfigureAwait(false);
    }

    private async Task<ICollection<ValidationFailure>> ValidateRequestAsync(TRequest request,
        CancellationToken cancellationToken = default)
    {
        var context = new ValidationContext<TRequest>(request);
        var results = await ValidateValidatorsAsync(context, cancellationToken).ConfigureAwait(false);
        return AggregateValidationResults(results).ToList();
    }

    private async Task<IEnumerable<ValidationResult>> ValidateValidatorsAsync(IValidationContext context,
        CancellationToken cancellationToken = default)
    {
        //TODO: paralelized validation?
        var validationTasks = _validators.Select(v => v.ValidateAsync(context, cancellationToken));
        return await Task.WhenAll(validationTasks).ConfigureAwait(false);
    }

    private static IEnumerable<ValidationFailure> AggregateValidationResults(IEnumerable<ValidationResult> results)
    {
        return results
            .SelectMany(r => r.Errors).Distinct()
            .Where(f => f is not null);
    }
}