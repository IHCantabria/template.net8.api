using template.net8.api.Core.Attributes;

namespace template.net8.api.Controllers;

internal static class SwaggerDocumentation
{
    internal static class Dummies
    {
        internal const string ControllerDescription = "Dummy Controller";

        internal static class GetDummies
        {
            internal const string Summary = "Get the Dummies.";
            internal const string Description = "Get the dummies in the system.";
            internal const string Id = "GetDummiesAsync";
            internal const string Ok = "Return the dummies in the system.";
        }

        internal static class GetDummy
        {
            internal const string Summary = "Get Dummy.";
            internal const string Description = "Get data about a Dummy based on its Key(key).";
            internal const string Id = "GetDummyAsync";
            internal const string Ok = "Return the dummy with the specified key in the system.";

            internal const string BadRequest =
                "Unable to get the dummy due to a client payload error. Please review the payload and fix the errors before retry the request.";

            internal const string NotFound =
                "Unable to get the dummy due to mistmaching data in the client query. The Dummy key is not present in the system.";
        }

        internal static class CreateDummy
        {
            internal const string Summary = "Create Dummy.";
            internal const string Description = "Create a new Dummy in the system.";
            internal const string Id = "CreateDummyAsync";
            internal const string Ok = "Return the dummy created in the system.";

            internal const string UnprocessableEntity =
                "Unable to create the dummy due to a data incompatibility in the client payload. Please review the payload and fix the errors before retry the request.";
        }
    }

    [CoreLibrary]
    internal static class System
    {
        internal const string ControllerDescription = "System Controller";

        internal static class GetErrorCodes
        {
            internal const string Summary = "Get Error Codes.";
            internal const string Description = "Get the error codes and their description managed for the system.";
            internal const string Id = "GetErrorCodesAsync";
            internal const string Ok = "Return the error codes and their description managed for the system.";
        }

        internal static class GetVersion
        {
            internal const string Summary = "Get Version";
            internal const string Description = "Get the current system version.";
            internal const string Id = "GetVersionAsync";
            internal const string Ok = "Return the current system version.";
        }
    }

    [CoreLibrary]
    internal static class Filter
    {
        internal const string AuthorizationErrorDescription =
            "Unable to execute the requested operation due to a authorization error. Please, log in the system with your user and get a valid access_token before trying again. if the error persist contact with IH-IT.";

        internal const string ForbiddenErrorDescription =
            "Unable to execute the requested operation due to a forbidden error. Your current user don't have privileges to execute the requested operation.";

        internal const string InternalServerErrorDescription =
            "Unable to execute the requested operation due to a server error. Please, try again after a couple of mins. if the error persist contact with IH-IT.";

        internal const string RequestTimeoutErrorDescription =
            "Unable to execute the requested operation due to a request timeout issue, please retry the request.";
    }
}