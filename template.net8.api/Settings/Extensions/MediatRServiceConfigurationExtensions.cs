using LanguageExt.Common;
using MediatR;
using template.net8.api.Behaviors;
using template.net8.api.Core.Attributes;
using template.net8.api.Domain.DTOs;
using template.net8.api.Features.Commands;

namespace template.net8.api.Settings.Extensions;

[CoreLibrary]
internal static class MediatRServiceConfigurationExtensions
{
    private static MediatRServiceConfiguration AddValidation<TRequest, TResponse>(
        this MediatRServiceConfiguration config) where TRequest : notnull
    {
        return config
            .AddBehavior<IPipelineBehavior<TRequest, Result<TResponse>>, ValidationBehavior<TRequest, TResponse>>();
    }

    internal static MediatRServiceConfiguration AddBehaviours(this MediatRServiceConfiguration config)
    {
        config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        config.AddValidation<CreateDummyCommand, DummyDto>();
        return config;
    }

    internal static MediatRServiceConfiguration AddPostProcesses(this MediatRServiceConfiguration config)
    {
        return config;
    }
}