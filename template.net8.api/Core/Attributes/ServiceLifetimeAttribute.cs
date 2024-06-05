namespace template.net8.api.Core.Attributes;

/// <summary>
///     This attribute is used to define the lifetime of the service.
/// </summary>
/// <param name="serviceLifetime"></param>
[AttributeUsage(AttributeTargets.Class)]
[CoreLibrary]
public sealed class ServiceLifetimeAttribute(ServiceLifetime serviceLifetime) : Attribute
{
    /// <summary>
    ///     The lifetime of the service.
    /// </summary>
    public ServiceLifetime ServiceLifetime { get; private set; } = serviceLifetime;
}