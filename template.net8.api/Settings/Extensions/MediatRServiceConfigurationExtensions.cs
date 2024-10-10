using LanguageExt.Common;
using MediatR;
using template.net8.api.Behaviors;
using template.net8.api.Core.Attributes;
using template.net8.api.Domain.DTOs;
using template.net8.api.Domain.Persistence.Models;
using template.net8.api.Features.Commands;
using template.net8.api.Features.Querys;

namespace template.net8.api.Settings.Extensions;

[CoreLibrary]
internal static class MediatRServiceConfigurationExtensions
{
    internal static MediatRServiceConfiguration AddBehaviours(this MediatRServiceConfiguration config)
    {
        config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        config.AddValidations();

        return config;
    }

    private static MediatRServiceConfiguration AddValidation<TRequest, TResponse>(
        this MediatRServiceConfiguration config) where TRequest : notnull
    {
        return config
            .AddBehavior<IPipelineBehavior<TRequest, Result<TResponse>>, ValidationBehavior<TRequest, TResponse>>();
    }


    private static MediatRServiceConfiguration AddValidations(this MediatRServiceConfiguration config)
    {
        config.AddValidation<CommandCreateDummy, Dummy>();
        config.AddValidation<QueryGetDummy, DummyDto>();
        config.AddValidation<QueryGetDummies, IEnumerable<DummyDto>>();
        return config;
    }

    internal static MediatRServiceConfiguration AddPostProcesses(this MediatRServiceConfiguration config)
    {
        return config;
    }
}