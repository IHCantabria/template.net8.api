using template.net8.api.Core.Attributes;

namespace template.net8.api.Logger;

//Add in this class dedicated methods to log specific events in the business logic if needed.
internal static partial class LoggerMessageMethods
{
    [CoreLibrary]
    [LoggerMessage(EventId = 1, Level = LogLevel.Debug,
        Message = LoggerMessageDefinitions.ControllerInjected)]
    internal static partial void LogControllerBaseInjected(this ILogger logger, string controllerType);

    [CoreLibrary]
    [LoggerMessage(EventId = 2, Level = LogLevel.Debug,
        Message = LoggerMessageDefinitions.ServiceInjected)]
    internal static partial void LogServiceBaseInjected(this ILogger logger, string serviceType);

    [CoreLibrary]
    [LoggerMessage(EventId = 3, Level = LogLevel.Debug,
        Message = LoggerMessageDefinitions.RepositoryInjected)]
    internal static partial void LogRepositoryBaseInjected(this ILogger logger, string repositoryType);

    [CoreLibrary]
    [LoggerMessage(EventId = 4, Level = LogLevel.Debug,
        Message = LoggerMessageDefinitions.WebClientInjected)]
    internal static partial void LogWebClientBaseInjected(this ILogger logger, string webClientType);

    [CoreLibrary]
    [LoggerMessage(EventId = 5, Level = LogLevel.Error, Message = LoggerMessageDefinitions.ExceptionServer)]
    internal static partial void LogExceptionServer(this ILogger logger, string message);

    [CoreLibrary]
    [LoggerMessage(EventId = 6, Level = LogLevel.Information,
        Message = LoggerMessageDefinitions.ActionRequestReceived)]
    internal static partial void LogActionRequestReceived(this ILogger logger, string? methodName, string? requestPath);

    [CoreLibrary]
    [LoggerMessage(EventId = 7, Level = LogLevel.Information,
        Message = LoggerMessageDefinitions.ActionRequestParameter)]
    internal static partial void LogActionRequestParameter(this ILogger logger, string key, string value);

    [CoreLibrary]
    [LoggerMessage(EventId = 8, Level = LogLevel.Error,
        Message = LoggerMessageDefinitions.ActionRequestResponseError)]
    internal static partial void LogActionRequestResponseError(this ILogger logger, string jsonError);

    [CoreLibrary]
    [LoggerMessage(EventId = 9, Level = LogLevel.Information,
        Message = LoggerMessageDefinitions.ActionRequestResponsed)]
    internal static partial void
        LogActionRequestResponsed(this ILogger logger, string? methodName, string? requestPath);

    [CoreLibrary]
    [LoggerMessage(EventId = 10, Level = LogLevel.Information,
        Message = LoggerMessageDefinitions.HandlingRequest)]
    internal static partial void LogHandlingRequest(this ILogger logger, string requestType);

    [CoreLibrary]
    [LoggerMessage(EventId = 11, Level = LogLevel.Information,
        Message = LoggerMessageDefinitions.HandledRequestSuccess)]
    internal static partial void LogHandledRequestSuccess(this ILogger logger, string requestType, string time);

    [CoreLibrary]
    [LoggerMessage(EventId = 12, Level = LogLevel.Warning, Message = LoggerMessageDefinitions.HandledRequestEmpty)]
    internal static partial void LogHandledRequestIsEmpty(this ILogger logger, string requestType, string time);

    [CoreLibrary]
    [LoggerMessage(EventId = 13, Level = LogLevel.Warning, Message = LoggerMessageDefinitions.HandledRequestError)]
    internal static partial void LogHandledRequestError(this ILogger logger, string requestType, string time);

    [CoreLibrary]
    [LoggerMessage(EventId = 14, Level = LogLevel.Warning, Message = LoggerMessageDefinitions.ExceptionClient)]
    internal static partial void LogExceptionClient(this ILogger logger, string message);

    [CoreLibrary]
    [LoggerMessage(EventId = 15, Level = LogLevel.Information, Message = LoggerMessageDefinitions.HandlingPostProcess)]
    internal static partial void
        LogHandlingPostProcess(this ILogger logger, string postProcessType, string requestType);

    [CoreLibrary]
    [LoggerMessage(EventId = 16, Level = LogLevel.Information, Message = LoggerMessageDefinitions.HandledPostProcess)]
    internal static partial void LogHandledPostProcess(this ILogger logger, string postProcessType, string requestType,
        string time);
}