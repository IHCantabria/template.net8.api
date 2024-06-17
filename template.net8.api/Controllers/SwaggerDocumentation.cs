namespace template.net8.api.Controllers;

internal static class SwaggerDocumentation
{
    internal static class Dummy
    {
        internal const string Tag = "Dummy Controller";

        internal static class GetDummies
        {
            internal const string Summary = "Get the Dummies.";
            internal const string Description = "Get the dummies in the system.";
            internal const string Id = "GetDummies";
            internal const string Ok = "Return the dummies in the system.";
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