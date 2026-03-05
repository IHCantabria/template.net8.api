namespace template.net8.api.Logger;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static partial class LoggerMessageMethods
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [LoggerMessage(Level = LogLevel.Debug,
        Message = LoggerMessageDefinitions.ControllerInjected)]
    internal static partial void LogControllerBaseInjected(this ILogger logger, string controllerType);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [LoggerMessage(Level = LogLevel.Debug,
        Message = LoggerMessageDefinitions.ServiceInjected)]
    internal static partial void LogServiceBaseInjected(this ILogger logger, string serviceType);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [LoggerMessage(Level = LogLevel.Debug,
        Message = LoggerMessageDefinitions.RepositoryInjected)]
    internal static partial void LogRepositoryBaseInjected(this ILogger logger, string repositoryType);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [LoggerMessage(Level = LogLevel.Debug,
        Message = LoggerMessageDefinitions.WebClientInjected)]
    internal static partial void LogWebClientBaseInjected(this ILogger logger, string webClientType);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [LoggerMessage(Level = LogLevel.Error, Message = LoggerMessageDefinitions.ExceptionServer)]
    internal static partial void LogExceptionServer(this ILogger logger, Exception ex);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [LoggerMessage(Level = LogLevel.Information,
        Message = LoggerMessageDefinitions.ActionRequestReceived)]
    internal static partial void LogActionRequestReceived(this ILogger logger, string? methodName, string? requestPath);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [LoggerMessage(Level = LogLevel.Information,
        Message = LoggerMessageDefinitions.ActionRequestResponsed)]
    internal static partial void
        LogActionRequestResponsedSuccess(this ILogger logger, string? methodName, string? requestPath);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [LoggerMessage(Level = LogLevel.Error,
        Message = LoggerMessageDefinitions.ActionRequestResponsedError)]
    internal static partial void
        LogActionRequestResponsedError(this ILogger logger, string? methodName, string? requestPath,
            string? statusCode);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [LoggerMessage(Level = LogLevel.Information,
        Message = LoggerMessageDefinitions.HandlingRequest)]
    internal static partial void LogHandlingRequest(this ILogger logger, string requestType);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [LoggerMessage(Level = LogLevel.Information,
        Message = LoggerMessageDefinitions.HandledRequestSuccess)]
    internal static partial void LogHandledRequestSuccess(this ILogger logger, string requestType, string time);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [LoggerMessage(Level = LogLevel.Warning, Message = LoggerMessageDefinitions.HandledRequestEmpty)]
    internal static partial void LogHandledRequestIsEmpty(this ILogger logger, string requestType, string time);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [LoggerMessage(Level = LogLevel.Warning, Message = LoggerMessageDefinitions.HandledRequestError)]
    internal static partial void LogHandledRequestError(this ILogger logger, string requestType, string time);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [LoggerMessage(Level = LogLevel.Warning, Message = LoggerMessageDefinitions.ExceptionClient)]
    internal static partial void LogExceptionClient(this ILogger logger, Exception ex);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [LoggerMessage(Level = LogLevel.Information, Message = LoggerMessageDefinitions.HandlingPostProcess)]
    internal static partial void
        LogHandlingPostProcess(this ILogger logger, string postProcessType, string requestType);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [LoggerMessage(Level = LogLevel.Information, Message = LoggerMessageDefinitions.HandledPostProcess)]
    internal static partial void LogHandledPostProcess(this ILogger logger, string postProcessType, string requestType,
        string time);

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [LoggerMessage(Level = LogLevel.Error, Message = LoggerMessageDefinitions.StatusDbFail)]
    internal static partial void LogStatusDbFail(this ILogger logger, Exception ex);
}