using template.net8.api.Core.Attributes;

namespace template.net8.api.Logger;

[CoreLibrary]
internal static partial class LoggerMessageMethods
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Information,
        Message = LoggerMessageDefinitions.ControllerInjected)]
    internal static partial void LogControllerBaseInjected(this ILogger logger, string controllerType);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information,
        Message = LoggerMessageDefinitions.ServiceInjected)]
    internal static partial void LogServiceBaseInjected(this ILogger logger, string serviceType);

    [LoggerMessage(EventId = 3, Level = LogLevel.Information,
        Message = LoggerMessageDefinitions.RepositoryInjected)]
    internal static partial void LogRepositoryBaseInjected(this ILogger logger, string repositoryType);

    [LoggerMessage(EventId = 4, Level = LogLevel.Information,
        Message = LoggerMessageDefinitions.WebClientInjected)]
    internal static partial void LogWebClientBaseInjected(this ILogger logger, string webClientType);

    [LoggerMessage(EventId = 5, Level = LogLevel.Information,
        Message = LoggerMessageDefinitions.ExceptionGeneral)]
    internal static partial void LogExceptionGeneral(this ILogger logger, string message);

    [LoggerMessage(EventId = 6, Level = LogLevel.Information,
        Message = LoggerMessageDefinitions.HandlingRequest)]
    internal static partial void LogHandlingRequest(this ILogger logger, string requestType);

    [LoggerMessage(EventId = 7, Level = LogLevel.Information,
        Message = LoggerMessageDefinitions.HandledRequest)]
    internal static partial void LogHandledRequest(this ILogger logger, string requestType, string status, string time);
}