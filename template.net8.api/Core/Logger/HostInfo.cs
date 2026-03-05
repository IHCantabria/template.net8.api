using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;

namespace template.net8.api.Core.Logger;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class HostInfo
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumented",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static string GetHostIp()
    {
        var addresses = Dns.GetHostAddresses(Dns.GetHostName());

        var ipv4 = addresses
            .FirstOrDefault(static ip => ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip));

        if (ipv4 != null)
            return ipv4.ToString();

        var ipv6 = addresses
            .FirstOrDefault(static ip => ip.AddressFamily == AddressFamily.InterNetworkV6 && !IPAddress.IsLoopback(ip));

        return ipv6 != null ? ipv6.ToString() : "unknown";
    }
}