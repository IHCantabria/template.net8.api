using template.net8.api.Behaviors.Users;

namespace template.net8.api.Behaviors.Extensions;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class MediatRServiceConfigurationExtensions
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static MediatRServiceConfiguration ConfigurePostProcesses(this MediatRServiceConfiguration config)
    {
        config.AddRequestPostProcessor<CreateUserProcessor>();
        config.AddRequestPostProcessor<UpdateUserProcessor>();
        config.AddRequestPostProcessor<DeleteUserProcessor>();
        return config;
    }
}