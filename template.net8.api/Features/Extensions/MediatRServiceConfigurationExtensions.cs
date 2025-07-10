using template.net8.api.Core.Attributes;
using template.net8.api.Domain.DTOs;
using template.net8.api.Domain.Persistence.Models;
using template.net8.api.Features.Commands;
using template.net8.api.Features.Querys;
using template.net8.api.Settings.Extensions;

namespace template.net8.api.Features.Extensions;

[CoreLibrary]
internal static class MediatRServiceConfigurationExtensions
{
    internal static MediatRServiceConfiguration ConfigureValidations(this MediatRServiceConfiguration config)
    {
        config.AddValidation<CommandCreateDummy, Dummy>();
        config.AddValidation<QueryGetDummy, DummyDto>();
        config.AddValidation<QueryGetDummies, IEnumerable<DummyDto>>();
        return config;
    }
}