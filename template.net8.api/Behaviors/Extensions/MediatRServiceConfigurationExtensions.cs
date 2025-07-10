using template.net8.api.Core.Attributes;

namespace template.net8.api.Behaviors.Extensions;

[CoreLibrary]
internal static class MediatRServiceConfigurationExtensions
{
    internal static MediatRServiceConfiguration ConfigurePostProcesses(this MediatRServiceConfiguration config)
    {
        config.AddRequestPostProcessor<NewDummyProcessors>();
        return config;
    }
}