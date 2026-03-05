using Microsoft.Extensions.Localization;

namespace template.net8.api.Core.Contracts;

public sealed partial record ErrorCodeResource
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static implicit operator ErrorCodeResource(LocalizedString obj)
    {
        ArgumentNullException.ThrowIfNull(obj);
        return new ErrorCodeResource
        {
            Key = obj.Name,
            Description = obj.Value
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static ErrorCodeResource ToErrorCodeResource(
        LocalizedString obj)
    {
        return obj;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static IEnumerable<ErrorCodeResource> ToCollection(
        IReadOnlyList<LocalizedString> objs)
    {
        var resources = new ErrorCodeResource[objs.Count];
        for (var i = 0; i < objs.Count; i++) resources[i] = objs[i];
        return resources;
    }
}