namespace template.net8.api.Logger;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class LoggerMessageDefinitions
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string ServiceInjected = "Service {serviceType} injected successfully";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string ControllerInjected = "Controller {controllerType} injected successfully";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string RepositoryInjected = "Repository {repositoryType} injected successfully";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string WebClientInjected = "Web Client {webClientType} injected successfully";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string ExceptionServer = "Server Exception occurred.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string StatusDbFail = "DB is not running.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string ExceptionClient = "Client Exception occurred.";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string ActionRequestReceived = "Action Request Received: {methodName} at path {requestPath}";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string ActionRequestResponsed = "Action Request Responsed: {methodName} at path {requestPath}";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string ActionRequestResponsedError =
        "Action Request Responsed: {methodName} at path {requestPath} with ERROR code: {statusCode}";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string HandlingRequest = "Handling Request {requestType}";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string HandledRequestSuccess = "Handled Request {requestType}, with state Success in {time}";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string HandledRequestEmpty =
        "Handled Request {requestType}, finished in {time} with empty Result";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string HandledRequestError = "Handled Request {requestType}, with state ERROR in {time}";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string HandlingPostProcess =
        "Handling Post Process {postProcessType} for the Request {requestType}";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal const string HandledPostProcess =
        "Handled Post Process {postProcessType} for the {requestType} in {time}";
}