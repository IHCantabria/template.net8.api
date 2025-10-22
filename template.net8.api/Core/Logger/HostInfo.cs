using System.Net;
using System.Net.Sockets;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Logger;

[CoreLibrary]
internal static class HostInfo
{
    /// <summary>
    ///     Gets the first non-loopback IP address of the host machine.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>hostNameOrAddress</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="SocketException">An error is encountered when resolving the local host name.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     The length of
    ///     <paramref>
    ///         <name>hostNameOrAddress</name>
    ///     </paramref>
    ///     is greater than 255 characters.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <paramref>
    ///         <name>hostNameOrAddress</name>
    ///     </paramref>
    ///     is an invalid IP address.
    /// </exception>
    internal static string GetHostIp()
    {
        var addresses = Dns.GetHostAddresses(Dns.GetHostName());

        var ipv4 = addresses
            .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip));

        if (ipv4 != null)
            return ipv4.ToString();

        var ipv6 = addresses
            .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetworkV6 && !IPAddress.IsLoopback(ip));

        return ipv6 != null ? ipv6.ToString() : "unknown";
    }
}