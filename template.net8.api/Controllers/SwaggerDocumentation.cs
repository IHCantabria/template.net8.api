using template.net8.api.Core.Attributes;

namespace template.net8.api.Controllers;

[CoreLibrary]
internal static class SwaggerDocumentation
{
    internal static class Dummies
    {
        internal const string ControllerDescription = "Dummy Controller";

        internal static class GetDummies
        {
            internal const string Summary = "Get the Dummies.";
            internal const string Description = "Get the dummies in the system.";
            internal const string Id = "GetDummies";
            internal const string Ok = "Return the dummies in the system.";
        }

        internal static class GetDummy
        {
            internal const string Summary = "Get Dummy.";
            internal const string Description = "Get data about a Dummy based on its Key(key).";
            internal const string Id = "GetDummy";
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
            internal const string Id = "CreateDummy";
            internal const string Ok = "Return the dummy created in the system.";

            internal const string UnprocessableEntity =
                "Unable to create the dummy due to a data incompatibility in the client payload. Please review the payload and fix the errors before retry the request.";
        }
    }
}