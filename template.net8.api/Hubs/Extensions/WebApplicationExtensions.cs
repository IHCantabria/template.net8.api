using template.net8.api.Hubs.Dummy;

namespace template.net8.api.Hubs.Extensions;

internal static class WebApplicationExtensions
{
    /// <summary>
    ///     Configures the SignalR hubs for the application.
    /// </summary>
    internal static void ConfigureHubs(this WebApplication app)
    {
        app.MapHub<DummyHub>(ApiRoutes.DummiesHub.PathHub,
            options => options.AllowStatefulReconnects = true);
    }
}