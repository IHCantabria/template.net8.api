using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using template.net8.api.Core.Exceptions;

namespace template.net8.api.Business.Exceptions;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[UsedImplicitly]
public abstract class BusinessException : CoreException
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    protected BusinessException()
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    protected BusinessException(string message) : base(message)
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    protected BusinessException(string message, Exception innerException) : base(message, innerException)
    {
    }
}