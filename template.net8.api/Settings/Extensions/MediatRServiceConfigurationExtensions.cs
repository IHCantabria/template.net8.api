using MediatR;
using template.net8.api.Behaviors;
using template.net8.api.Behaviors.Extensions;
using template.net8.api.Core.Attributes;
using template.net8.api.Features.Extensions;

namespace template.net8.api.Settings.Extensions;

[CoreLibrary]
internal static class MediatRServiceConfigurationExtensions
{
    internal static MediatRServiceConfiguration AddBehaviours(this MediatRServiceConfiguration config)
    {
        config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        config.ConfigureValidations();

        return config;
    }

    internal static MediatRServiceConfiguration AddValidation<TRequest, TResponse>(
        this MediatRServiceConfiguration config) where TRequest : notnull
    {
        return config
            .AddBehavior<IPipelineBehavior<TRequest, LanguageExt.Common.Result<TResponse>>,
                ValidationBehavior<TRequest, TResponse>>();
    }

    internal static MediatRServiceConfiguration AddPostProcesses(this MediatRServiceConfiguration config)
    {
        return config.ConfigurePostProcesses();
    }
}