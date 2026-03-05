namespace template.net8.api.Core.Attributes;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
internal sealed class ServiceLifetimeAttribute(ServiceLifetime serviceLifetime) : Attribute
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public ServiceLifetime ServiceLifetime { get; } = serviceLifetime;
}