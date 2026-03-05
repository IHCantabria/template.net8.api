using System.Diagnostics.CodeAnalysis;

namespace template.net8.api.Core.Exceptions;

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
public sealed class ResultFaultedInvalidOperationException : CoreException
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public ResultFaultedInvalidOperationException()
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public ResultFaultedInvalidOperationException(string message) : base(message)
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public ResultFaultedInvalidOperationException(string message, Exception innerException) : base(message,
        innerException)
    {
    }
}