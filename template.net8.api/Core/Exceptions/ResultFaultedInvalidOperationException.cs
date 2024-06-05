using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Exceptions;

/// <summary>
///     Result Faulted Invalid Operation Exception
/// </summary>
[CoreLibrary]
public sealed class ResultFaultedInvalidOperationException : CoreException
{
    /// <summary>
    ///     Result Faulted Invalid Operation Exception Constructor
    /// </summary>
    public ResultFaultedInvalidOperationException()
    {
    }

    /// <summary>
    ///     Result Faulted Invalid Operation Exception Constructor with message
    /// </summary>
    /// <param name="message"></param>
    public ResultFaultedInvalidOperationException(string message) : base(message)
    {
    }

    /// <summary>
    ///     Result Faulted Invalid Operation Exception Constructor with message and inner exception
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public ResultFaultedInvalidOperationException(string message, Exception innerException) : base(message,
        innerException)
    {
    }
}