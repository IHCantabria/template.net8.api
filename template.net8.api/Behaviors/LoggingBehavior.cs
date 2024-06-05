using System.Diagnostics;
using LanguageExt.Common;
using MediatR;
using template.net8.api.Logger;

namespace template.net8.api.Behaviors;

internal sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        return BehaviorLogicAsync(next);
    }

    private async Task<TResponse> BehaviorLogicAsync(RequestHandlerDelegate<TResponse> next)
    {
        var start = Stopwatch.GetTimestamp();
        _logger.LogHandlingRequest(typeof(TRequest).Name);
        //PRE REQUEST
        var result = await next().ConfigureAwait(false);
        //POST REQUEST
        LogHandledRequest(result, start);
        return result;
    }

    private void LogHandledRequest(TResponse result, long startTimestamp)
    {
        var delta = Stopwatch.GetElapsedTime(startTimestamp);
        switch (result)
        {
            case Result<IEnumerable<object>> responseCollection:
                LogResult(responseCollection.IsSuccess, delta);
                break;
            case Result<object> responseObject:
                LogResult(responseObject.IsSuccess, delta);
                break;
            case not null:
                LogResult(true, delta);
                break;
            default:
                LogResult(false, delta);
                break;
        }
    }

    private void LogResult(bool isSuccess, TimeSpan delta)
    {
        var resultStatus = isSuccess ? "Succeded" : "Failed";
        _logger.LogHandledRequest(typeof(TRequest).Name, resultStatus, $"{delta}ms");
    }
}