using template.net8.api.Core.Attributes;

namespace template.net8.api.Logger;

[CoreLibrary]
internal static class LoggerMessageDefinitions
{
    internal const string ServiceInjected = "Service {ServiceType} injected successfully";

    internal const string ControllerInjected = "Controller {ControllerType} injected successfully";

    internal const string RepositoryInjected = "Repository {RepositoryType} injected successfully";

    internal const string WebClientInjected = "Web Client {WebClientType} injected successfully";

    internal const string ExceptionServer = "Server Exception occurred, trace:{Message}";

    internal const string ExceptionClient = "Client Exception occurred, trace:{Message}";

    internal const string HandlingRequest = "Handling Request {RequestType}";

    internal const string HandledRequestSuccess = "Handled Request {RequestType}, with state Success in {time}";

    internal const string HandledRequestEmpty = "Handled Request {RequestType}, finished in {time} with empty Result";

    internal const string HandledRequestError = "Handled Request {RequestType}, with state ERROR in {time}";

    internal const string HandlingPostProcess =
        "Handling Post Process {PostProcessType} for the Request {RequestType}";

    internal const string HandledPostProcess =
        "Handled Post Process {PostProcessType} for the {RequestType} in {time}";
}