using System.Diagnostics.CodeAnalysis;
using Serilog.Core;
using Serilog.Events;

namespace template.net8.api.Core.Logger.Enrichers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class ClientIpEnricher : ILogEventEnricher
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private const string IpAddressPropertyName = "client.ip";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private const string IpAddressItemKey = "Serilog_ClientIp";

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly IHttpContextAccessor _contextAccessor;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public ClientIpEnricher() : this(new HttpContextAccessor())
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private ClientIpEnricher(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="logEvent" /> is <see langword="null" />.
    ///     <paramref name="propertyFactory" /> is <see langword="null" />.
    /// </exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        ArgumentNullException.ThrowIfNull(logEvent);
        ArgumentNullException.ThrowIfNull(propertyFactory);

        var property = ResolveIpProperty();
        if (property is null)
            return;

        logEvent.AddPropertyIfAbsent(property);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private LogEventProperty? ResolveIpProperty()
    {
        var httpContext = _contextAccessor.HttpContext;
        if (httpContext == null)
            return null;

        var ipAddress =
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        if (httpContext.Items.TryGetValue(IpAddressItemKey, out var existing)
            && existing is LogEventProperty property)
        {
            if (!string.Equals(
                    ((ScalarValue)property.Value).Value?.ToString(),
                    ipAddress,
                    StringComparison.Ordinal))
                property = new LogEventProperty(
                    IpAddressPropertyName,
                    new ScalarValue(ipAddress));

            return property;
        }

        var newProperty =
            new LogEventProperty(IpAddressPropertyName, new ScalarValue(ipAddress));

        httpContext.Items[IpAddressItemKey] = newProperty;

        return newProperty;
    }
}