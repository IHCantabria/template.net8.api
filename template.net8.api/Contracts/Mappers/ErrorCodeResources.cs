using Microsoft.Extensions.Localization;

namespace template.net8.api.Contracts;

/// <summary>
///     Create Extent Resource
/// </summary>
public sealed partial record ErrorCodeResource
{
    /// <summary>
    ///     Convert Object to Resource
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
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
    ///     This method converts a LocalizedString to a ErrorCodeResource
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static ErrorCodeResource ToErrorCodeResource(
        LocalizedString obj)
    {
        return obj;
    }

    internal static IEnumerable<ErrorCodeResource> ToCollection(
        IReadOnlyList<LocalizedString> objs)
    {
        var resources = new ErrorCodeResource[objs.Count];
        for (var i = 0; i < objs.Count; i++) resources[i] = objs[i];
        return resources;
    }
}