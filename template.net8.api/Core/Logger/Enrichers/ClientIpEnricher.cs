using System.Net.Sockets;
using Serilog.Core;
using Serilog.Events;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Logger.Enrichers;

[CoreLibrary]
internal sealed class ClientIpEnricher : ILogEventEnricher
{
    private const string IpAddressPropertyName = "client.ip";
    private const string IpAddressItemKey = "Serilog_ClientIp";

    private readonly IHttpContextAccessor _contextAccessor;

    public ClientIpEnricher() : this(new HttpContextAccessor())
    {
    }

    internal ClientIpEnricher(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    /// <summary>
    ///     Enriches the log event with Activity information.
    /// </summary>
    /// <param name="logEvent"></param>
    /// <param name="propertyFactory"></param>
    /// <exception cref="ArgumentNullException">
    ///     When
    ///     <paramref>
    ///         <name>property</name>
    ///     </paramref>
    ///     is <code>null</code>
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     When
    ///     <paramref>
    ///         <name>name</name>
    ///     </paramref>
    ///     is empty or only contains whitespace
    /// </exception>
    /// <exception cref="SocketException">
    ///     The address family is <see cref="System.Net.Sockets.AddressFamily.InterNetworkV6" />
    ///     and the address is bad.
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///     The
    ///     <see>
    ///         <cref>IDictionary`2</cref>
    ///     </see>
    ///     is read-only.
    /// </exception>
    /// <exception cref="System.Collections.Generic.KeyNotFoundException">
    ///     The property is retrieved and
    ///     <paramref>
    ///         <name>key</name>
    ///     </paramref>
    ///     is not found.
    /// </exception>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = _contextAccessor.HttpContext;
        if (httpContext == null) return;

        var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        if (httpContext.Items.TryGetValue(IpAddressItemKey, out var existing)
            && existing is LogEventProperty existingProperty)
        {
            if (!string.Equals(((ScalarValue)existingProperty.Value).Value?.ToString(), ipAddress,
                    StringComparison.Ordinal))
                existingProperty = new LogEventProperty(IpAddressPropertyName, new ScalarValue(ipAddress));

            logEvent.AddPropertyIfAbsent(existingProperty);
            return;
        }

        var newProperty = new LogEventProperty(IpAddressPropertyName, new ScalarValue(ipAddress));
        httpContext.Items[IpAddressItemKey] = newProperty;
        logEvent.AddPropertyIfAbsent(newProperty);
    }
}