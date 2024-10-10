﻿using template.net8.api.Core.Attributes;

namespace template.net8.api.Logger;

[CoreLibrary]
internal static partial class LoggerMessageMethods
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Debug,
        Message = LoggerMessageDefinitions.ControllerInjected)]
    internal static partial void LogControllerBaseInjected(this ILogger logger, string controllerType);

    [LoggerMessage(EventId = 2, Level = LogLevel.Debug,
        Message = LoggerMessageDefinitions.ServiceInjected)]
    internal static partial void LogServiceBaseInjected(this ILogger logger, string serviceType);

    [LoggerMessage(EventId = 3, Level = LogLevel.Debug,
        Message = LoggerMessageDefinitions.RepositoryInjected)]
    internal static partial void LogRepositoryBaseInjected(this ILogger logger, string repositoryType);

    [LoggerMessage(EventId = 4, Level = LogLevel.Debug,
        Message = LoggerMessageDefinitions.WebClientInjected)]
    internal static partial void LogWebClientBaseInjected(this ILogger logger, string webClientType);

    [LoggerMessage(EventId = 5, Level = LogLevel.Error, Message = LoggerMessageDefinitions.ExceptionServer)]
    internal static partial void LogExceptionServer(this ILogger logger, string message);

    [LoggerMessage(EventId = 6, Level = LogLevel.Information,
        Message = LoggerMessageDefinitions.HandlingRequest)]
    internal static partial void LogHandlingRequest(this ILogger logger, string requestType);

    [LoggerMessage(EventId = 7, Level = LogLevel.Information, Message = LoggerMessageDefinitions.HandledRequestSuccess)]
    internal static partial void LogHandledRequestSuccess(this ILogger logger, string requestType, string time);

    [LoggerMessage(EventId = 8, Level = LogLevel.Warning, Message = LoggerMessageDefinitions.HandledRequestEmpty)]
    internal static partial void LogHandledRequestIsEmpty(this ILogger logger, string requestType, string time);

    [LoggerMessage(EventId = 9, Level = LogLevel.Warning, Message = LoggerMessageDefinitions.HandledRequestError)]
    internal static partial void LogHandledRequestError(this ILogger logger, string requestType, string time);

    [LoggerMessage(EventId = 10, Level = LogLevel.Warning, Message = LoggerMessageDefinitions.ExceptionClient)]
    internal static partial void LogExceptionClient(this ILogger logger, string message);

    [LoggerMessage(EventId = 11, Level = LogLevel.Information, Message = LoggerMessageDefinitions.HandlingPostProcess)]
    internal static partial void
        LogHandlingPostProcess(this ILogger logger, string postProcessType, string requestType);

    [LoggerMessage(EventId = 12, Level = LogLevel.Information, Message = LoggerMessageDefinitions.HandledPostProcess)]
    internal static partial void LogHandledPostProcess(this ILogger logger, string postProcessType, string requestType,
        string time);
}