using AutoMapper;
using JetBrains.Annotations;
using template.net8.api.Domain.DTOs;

namespace template.net8.api.Domain.Persistence.Models.Automapper;

[UsedImplicitly]
internal sealed class VersionProfile : Profile
{
    public VersionProfile()
    {
        CreateMap<Dummy, DummyDto>();
    }
}