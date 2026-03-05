using MediatR;
using template.net8.api.Behaviors;
using template.net8.api.Behaviors.Extensions;
using template.net8.api.Features.Extensions;

namespace template.net8.api.Settings.Extensions;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class MediatRServiceConfigurationExtensions
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void AddBehaviours(this MediatRServiceConfiguration config)
    {
        config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        config.ConfigureValidations();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void AddValidation<TRequest, TResponse>(this MediatRServiceConfiguration config)
        where TRequest : notnull
    {
        config
            .AddBehavior<IPipelineBehavior<TRequest, LanguageExt.Common.Result<TResponse>>,
                ValidationBehavior<TRequest, TResponse>>();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static void AddPostProcesses(this MediatRServiceConfiguration config)
    {
        config.ConfigurePostProcesses();
    }
}