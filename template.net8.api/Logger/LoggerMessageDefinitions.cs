using template.net8.api.Core.Attributes;

namespace template.net8.api.Logger;

[CoreLibrary]
internal static class LoggerMessageDefinitions
{
    internal const string ServiceInjected = "Service {serviceType} injected successfully";

    internal const string ControllerInjected = "Controller {controllerType} injected successfully";

    internal const string RepositoryInjected = "Repository {repositoryType} injected successfully";

    internal const string WebClientInjected = "Web Client {webClientType} injected successfully";

    internal const string ExceptionServer = "Server Exception occurred, trace:{message}";

    internal const string ExceptionClient = "Client Exception occurred, trace:{message}";

    internal const string ActionRequestReceived = "Action Request Received: {methodName} at path {requestPath}";

    internal const string ActionRequestParameter = "Action Request Parameter: {key} = {value}";

    internal const string ActionRequestResponseError = "Action Request Response Error: {jsonError}";

    internal const string ActionRequestResponsed = "Action Request Responsed: {methodName} at path {requestPath}";

    internal const string HandlingRequest = "Handling Request {requestType}";

    internal const string HandledRequestSuccess = "Handled Request {requestType}, with state Success in {time}";

    internal const string HandledRequestEmpty = "Handled Request {requestType}, finished in {time} with empty Result";

    internal const string HandledRequestError = "Handled Request {requestType}, with state ERROR in {time}";

    internal const string HandlingPostProcess =
        "Handling Post Process {postProcessType} for the Request {requestType}";

    internal const string HandledPostProcess =
        "Handled Post Process {postProcessType} for the {requestType} in {time}";
}