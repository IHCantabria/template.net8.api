using template.net8.Api.Core.Attributes;

namespace template.net8.Api.Logger;

[CoreLibrary]
internal static class LoggerMessageDefinitions
{
    internal const string ServiceInjected = "Service {ServiceType} injected successfully.";

    internal const string ControllerInjected = "Controller {ControllerType} injected successfully.";

    internal const string RepositoryInjected = "Repository {RepositoryType} injected successfully.";

    internal const string WebClientInjected = "Web Client {WebClientType} injected successfully.";

    internal const string ExceptionGeneral = "Exception occurred, trace:{Message}.";

    internal const string HandlingRequest = "Handling Request {RequestType}.";

    internal const string HandledRequest =
        "Handled Request {RequestType}, {status} in {time})";
}