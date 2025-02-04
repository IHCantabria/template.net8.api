using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MediatR;
using template.net8.api.Core.Attributes;
using template.net8.api.Logger;

namespace template.net8.api.Behaviors;

[CoreLibrary]
internal sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private const string IsBottom = nameof(LanguageExt.Common.Result<bool>.IsBottom);

    private const string IsSuccess = nameof(LanguageExt.Common.Result<bool>.IsSuccess);

    private const string IsFaulted = nameof(LanguageExt.Common.Result<bool>.IsBottom);

    private const string Exception = "exception";

    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    ///     Handle the request
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

    private void LogHandledRequest(TResponse? result, long startTimestamp)
    {
        var delta = Stopwatch.GetElapsedTime(startTimestamp);
        LogResponse(result, delta);
    }

    private void LogResponse(TResponse? result, TimeSpan delta)
    {
        if (result is null)
        {
            LogResponseEmpty(delta);
            return;
        }

        if (IsResultType(result.GetType()))
        {
            LogResult(result, delta);
            return;
        }

        LogResponseSuccess(delta);
    }

    private void LogResponseEmpty(TimeSpan delta)
    {
        _logger.LogHandledRequestIsEmpty(typeof(TRequest).Name, $"{delta.TotalMilliseconds}ms");
    }

    private void LogResponseSuccess(TimeSpan delta)
    {
        _logger.LogHandledRequestSuccess(typeof(TRequest).Name, $"{delta.TotalMilliseconds}ms");
    }

    private void LogResponseError(Exception ex, TimeSpan delta)
    {
        LogResponseError(delta);
        _logger.LogExceptionClient(ex.ToString());
    }

    private void LogResponseError(TimeSpan delta)
    {
        _logger.LogHandledRequestError(typeof(TRequest).Name, $"{delta.TotalMilliseconds}ms");
    }

    [SuppressMessage("Security",
        "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields",
        Justification =
            "Access to the non-public field is necessary because the logic in this file is used to solve an integration problem between the Automapper and HotChocolate libraries.")]
    private void LogResult(TResponse result, TimeSpan delta)
    {
        var type = result?.GetType();
        var exceptionInfo = type?.GetField(Exception, BindingFlags.NonPublic | BindingFlags.Instance);

        // Get the value of the exception field
        if (exceptionInfo?.GetValue(result) is Exception exception)
            LogResponseError(exception, delta);
        else
            LogResponseSuccess(delta);
    }

    private static bool IsResultType(Type type)
    {
        return type.GetProperty(IsFaulted) != null
               && type.GetProperty(IsSuccess) != null
               && type.GetProperty(IsBottom) != null;
    }
}