using template.net8.api.Core.Attributes;

namespace template.net8.api.Logger;

//Adds the log messages of the dedicated log methods of the business logic.
internal static class LoggerMessageDefinitions
{
    [CoreLibrary] internal const string ServiceInjected = "Service {serviceType} injected successfully";

    [CoreLibrary] internal const string ControllerInjected = "Controller {controllerType} injected successfully";

    [CoreLibrary] internal const string RepositoryInjected = "Repository {repositoryType} injected successfully";

    [CoreLibrary] internal const string WebClientInjected = "Web Client {webClientType} injected successfully";

    [CoreLibrary] internal const string ExceptionServer = "Server Exception occurred.";

    [CoreLibrary] internal const string ExceptionClient = "Client Exception occurred.";

    [CoreLibrary]
    internal const string ActionRequestReceived = "Action Request Received: {methodName} at path {requestPath}";

    [CoreLibrary] internal const string ActionRequestParameter = "Action Request Parameter: {key} = {value}";

    [CoreLibrary] internal const string ActionRequestResponseError = "Action Request Response Error: {jsonError}";

    [CoreLibrary]
    internal const string ActionRequestResponsed = "Action Request Responsed: {methodName} at path {requestPath}";

    [CoreLibrary] internal const string HandlingRequest = "Handling Request {requestType}";

    [CoreLibrary]
    internal const string HandledRequestSuccess = "Handled Request {requestType}, with state Success in {time}";

    [CoreLibrary] internal const string HandledRequestEmpty =
        "Handled Request {requestType}, finished in {time} with empty Result";

    [CoreLibrary]
    internal const string HandledRequestError = "Handled Request {requestType}, with state ERROR in {time}";

    [CoreLibrary] internal const string HandlingPostProcess =
        "Handling Post Process {postProcessType} for the Request {requestType}";

    [CoreLibrary] internal const string HandledPostProcess =
        "Handled Post Process {postProcessType} for the {requestType} in {time}";
}