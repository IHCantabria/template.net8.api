using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MediatR;
using template.net8.api.Logger;

namespace template.net8.api.Behaviors;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private const string IsBottom = nameof(LanguageExt.Common.Result<bool>.IsBottom);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private const string IsSuccess = nameof(LanguageExt.Common.Result<bool>.IsSuccess);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private const string IsFaulted = nameof(LanguageExt.Common.Result<bool>.IsBottom);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private const string Exception = "exception";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        return BehaviorLogicAsync(next);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
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

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private void LogHandledRequest(TResponse? result, long startTimestamp)
    {
        var delta = Stopwatch.GetElapsedTime(startTimestamp);
        LogResponse(result, delta);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
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

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private void LogResponseEmpty(TimeSpan delta)
    {
        _logger.LogHandledRequestIsEmpty(typeof(TRequest).Name, $"{delta.TotalMilliseconds}ms");
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private void LogResponseSuccess(TimeSpan delta)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogHandledRequestSuccess(typeof(TRequest).Name, $"{delta.TotalMilliseconds}ms");
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private void LogResponseError(Exception ex, TimeSpan delta)
    {
        LogResponseError(delta);
        _logger.LogExceptionClient(ex);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private void LogResponseError(TimeSpan delta)
    {
        _logger.LogHandledRequestError(typeof(TRequest).Name, $"{delta.TotalMilliseconds}ms");
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
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

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static bool IsResultType(Type type)
    {
        return type.GetProperty(IsFaulted) != null
               && type.GetProperty(IsSuccess) != null
               && type.GetProperty(IsBottom) != null;
    }
}