using template.net8.api.Core.Attributes;

namespace template.net8.api.Domain;

/// <summary>
/// </summary>
[CoreLibrary]
//TODO: REFACTOR THIS CLASS, IS NOT USED
public sealed class VerificationModel
{
    /// <summary>
    /// </summary>
    public required object Verification { get; init; }

    /// <summary>
    /// </summary>
    public required bool IsUnique { get; init; }

    /// <summary>
    /// </summary>
    public required bool ExpecteResult { get; init; }

    /// <summary>
    /// </summary>
    public required string Msg { get; init; }
}