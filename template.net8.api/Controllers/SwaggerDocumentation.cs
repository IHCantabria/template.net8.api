namespace template.net8.api.Controllers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class SwaggerDocumentation
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static class Identity
    {
        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string ControllerDescription = "Identity Controller";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class Login
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Summary = "Login User.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Description = "Log in the system with credentials.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Id = "LoginAsync";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Ok = "Return the id token.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string BadRequest =
                "Unable to log in the system due to a client query error.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Forbidden =
                "Unable to log in the system using the credentials provided, invalid password.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string NotFound =
                "Unable to log in the system using the credentials provided, the user is not present in the system.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string UnprocessableEntity =
                "Unable to log in the system due to a data incompatibility in the client payload. Please review the payload and fix the errors before retry the request.";
        }

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class Access
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Summary = "Access User.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Description = "Request access in the system with id Token.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Id = "AccessAsync";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Ok = "Return the access token.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string BadRequest =
                "Unable to get the required access token due to a client query error.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Forbidden =
                "Unable to get the required access token, you dont have the required authentication level.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string NotFound =
                "Unable to get the required access token, the scope is not present in the system.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string UnprocessableEntity =
                "Unable to get the required access token due to a data incompatibility in the client payload. Please review the payload and fix the errors before retry the request.";
        }
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static class Users
    {
        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string ControllerDescription = "Users Controller";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class CreateUser
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Summary = "Create User.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Description = "Create a new User in the system.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Id = "CreateUserAsync";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Created = "Return the user created in the system.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string BadRequest =
                "Unable to create the user due to a client payload error. Please review the payload and fix the errors before retry the request.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string NotFound =
                "Unable to create the user due to mistmaching data in the client payload. Please review the payload and fix the errors before retry the request.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string UnprocessableEntity =
                "Unable to create the user due to a data incompatibility in the client payload. Please review the payload and fix the errors before retry the request.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Conflict =
                "Unable to create the user due to a conflict with it's current state. Please review the payload and fix the errors before retry the request.";
        }

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class UpdateUser
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Summary = "Update User.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Description = "Update a User in the system.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Id = "UpdateUserAsync";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Ok = "Return the updated created in the system.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string BadRequest =
                "Unable to update the user due to a client payload error. Please review the payload and fix the errors before retry the request.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string NotFound =
                "Unable to update the user due to mistmaching data in the client payload. Please review the payload and fix the errors before retry the request.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string UnprocessableEntity =
                "Unable to update the user due to a data incompatibility in the client payload. Please review the payload and fix the errors before retry the request.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Conflict =
                "Unable to update the user due to a conflict with it's current state. Please review the payload and fix the errors before retry the request.";
        }

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class DeleteUser
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Summary = "Delete User.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Description = "Delete a User in the system.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Id = "DeleteUserAsync";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Ok = "Return the user deleted in the system.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string BadRequest =
                "Unable to delete the user due to a client payload error. Please review the payload and fix the errors before retry the request.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string NotFound =
                "Unable to delete the user due to mistmaching data in the client payload. Please review the payload and fix the errors before retry the request.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string UnprocessableEntity =
                "Unable to delete the user due to a data incompatibility in the client payload. Please review the payload and fix the errors before retry the request.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Conflict =
                "Unable to delete the user due to a conflict with it's current state. Please review the payload and fix the errors before retry the request.";
        }

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class DisableUser
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Summary = "Disable User.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Description = "Disable a User in the system.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Id = "DisableUserAsync";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Ok = "Return the user disabled in the system.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string BadRequest =
                "Unable to disable the user due to a client payload error. Please review the payload and fix the errors before retry the request.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string NotFound =
                "Unable to disable the user due to mistmaching data in the client payload. Please review the payload and fix the errors before retry the request.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string UnprocessableEntity =
                "Unable to disable the user due to a data incompatibility in the client payload. Please review the payload and fix the errors before retry the request.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Conflict =
                "Unable to disable the user due to a conflict with it's current state. Please review the payload and fix the errors before retry the request.";
        }

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class EnableUser
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Summary = "Enable User.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Description = "Enable a User in the system.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Id = "EnableUserAsync";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Ok = "Return the user enabled in the system.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string BadRequest =
                "Unable to enable the user due to a client payload error. Please review the payload and fix the errors before retry the request.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string NotFound =
                "Unable to enable the user due to mistmaching data in the client payload. Please review the payload and fix the errors before retry the request.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string UnprocessableEntity =
                "Unable to enable the user due to a data incompatibility in the client payload. Please review the payload and fix the errors before retry the request.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Conflict =
                "Unable to enable the user due to a conflict with it's current state. Please review the payload and fix the errors before retry the request.";
        }

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class ResetUserPassword
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Summary = "Reset User Password.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Description = "Reset a User password in the system.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Id = "ResetUserPasswordAsync";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Ok = "Return the new user credentials in the system.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string BadRequest =
                "Unable to reset the user password due to a client payload error. Please review the payload and fix the errors before retry the request.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string NotFound =
                "Unable to reset the user password due to mistmaching data in the client payload. Please review the payload and fix the errors before retry the request.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string UnprocessableEntity =
                "Unable to reset the user password due to a data incompatibility in the client payload. Please review the payload and fix the errors before retry the request.";
        }

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class GetUsers
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Summary = "Get Users.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Description = "Get the users availables in the system.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Id = "GetUsersAsync";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Ok = "Return the uses availables in the system.";
        }

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class GetUser
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Summary = "Get User.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Description = "Get data about a user based on its Key(uuid).";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Id = "GetUserInfoAsync";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Ok = "Return the user with the specified key in the system.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string BadRequest =
                "Unable to get the user due to a client payload error. Please review the payload and fix the errors before retry the request.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string NotFound =
                "Unable to get the user due to mistmaching data in the client query. The User key is not present in the system.";
        }

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class GetUserEvents
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Summary = "Get the User's Events.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Description = "Get the user's events in the system.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Id = "GetUserEventsAsync";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Ok = "Return the user's events in the system.";
        }
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static class System
    {
        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string ControllerDescription = "System Controller";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class GetErrorCodes
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Summary = "Get Error Codes.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Description = "Get the error codes and their description managed for the system.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Id = "GetErrorCodesAsync";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Ok = "Return the error codes and their description managed for the system.";
        }

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal static class GetVersion
        {
            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Summary = "Get Version";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Description = "Get the current system version.";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Id = "GetVersionAsync";

            /// <summary>
            ///     ADD DOCUMENTATION
            /// </summary>
            internal const string Ok = "Return the current system version.";
        }
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static class Filter
    {
        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string AuthorizationErrorDescription =
            "Unable to execute the requested operation due to a authorization error. Please, log in the system with your user and get a valid access_token before trying again. if the error persist contact with IH-IT.";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string ForbiddenErrorDescription =
            "Unable to execute the requested operation due to a forbidden error. Your current user don't have privileges to execute the requested operation.";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string InternalServerErrorDescription =
            "Unable to execute the requested operation due to a server error. Please, try again after a couple of mins. if the error persist contact with IH-IT.";

        /// <summary>
        ///     ADD DOCUMENTATION
        /// </summary>
        internal const string RequestTimeoutErrorDescription =
            "Unable to execute the requested operation due to a request timeout issue, please retry the request.";
    }
}