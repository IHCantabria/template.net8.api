using template.net8.api.Core.Attributes;
using template.net8.api.Core.Contracts;
using template.net8.api.Hubs.Dummy;
using template.net8.api.Hubs.Dummy.Contracts;
using template.net8.api.Hubs.Dummy.Interfaces;

namespace template.net8.api.Hubs;

[CoreLibrary]
internal enum HubEventType
{
    ClientCall,
    ServerCall
}

[CoreLibrary]
internal static class HubsDocumentation
{
    internal static class Dummy
    {
        internal static class CheckConnectionStatus
        {
            internal const string Name = nameof(DummyHub.CheckConnectionStatus);
            internal const string Type = nameof(HubEventType.ClientCall);
        }

        internal static class ConnectionStatus
        {
            internal const string Name = nameof(IDummyHub.ConnectionStatus);
            internal const string Type = nameof(HubEventType.ServerCall);

            internal static readonly string[] Fields =
                [nameof(HubInfoMessageResource.ConnectionId), nameof(HubInfoMessageResource.Message)];
        }

        internal static class ConnectionOnline
        {
            internal const string Name = nameof(IDummyHub.ConnectionOnline);
            internal const string Type = nameof(HubEventType.ServerCall);

            internal static readonly string[] Fields =
                [nameof(HubInfoMessageResource.ConnectionId), nameof(HubInfoMessageResource.Message)];
        }

        internal static class NewDummy
        {
            internal const string Name = nameof(IDummyHub.NewDummy);
            internal const string Type = nameof(HubEventType.ServerCall);

            internal static readonly string[] Fields =
            [
                nameof(DummyHubNewDummyMessageResource.DummyKey),
                nameof(DummyHubNewDummyMessageResource.Message)
            ];
        }
    }
}