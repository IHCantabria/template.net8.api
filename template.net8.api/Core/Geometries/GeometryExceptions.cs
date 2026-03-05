using System.Diagnostics.CodeAnalysis;
using template.net8.api.Core.Exceptions;

namespace template.net8.api.Core.Geometries;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "MemberCanBeInternal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "ClassNeverInstantiated.Global",
    Justification =
        "Exception type is part of the public API; instantiation depends on usage by consumers or higher-level layers.")]
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification = "Standard exception constructor required by CA1032 and RCS1194.")]
public sealed class GeometryExtentNotValidException : CoreException
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public GeometryExtentNotValidException()
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public GeometryExtentNotValidException(string message) : base(message)
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public GeometryExtentNotValidException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "MemberCanBeInternal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "ClassNeverInstantiated.Global",
    Justification =
        "Exception type is part of the public API; instantiation depends on usage by consumers or higher-level layers.")]
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification = "Standard exception constructor required by CA1032 and RCS1194.")]
public sealed class GeometryPointNotValidException : CoreException
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public GeometryPointNotValidException()
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public GeometryPointNotValidException(string message) : base(message)
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public GeometryPointNotValidException(string message, Exception innerException) : base(message, innerException)
    {
    }
}