using template.net8.api.Hubs.User;

namespace template.net8.api.Hubs.Extensions;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class WebApplicationExtensions
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void ConfigureHubs(this WebApplication app)
    {
        app.MapHub<UserHub>(ApiRoutes.UsersHub.PathHub, static options => options.AllowStatefulReconnects = true);
    }
}