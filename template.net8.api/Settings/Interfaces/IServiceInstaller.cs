namespace template.net8.api.Settings.Interfaces;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal interface IServiceInstaller
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    short LoadOrder { get; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Task InstallServiceAsync(WebApplicationBuilder builder);
}