using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Exceptions;

/// <summary>
///     Result Success Invalid Operation Exception
/// </summary>
[CoreLibrary]
public sealed class ResultSuccessInvalidOperationException : CoreException
{
    /// <summary>
    ///     Result Success Invalid Operation Exception Constructor
    /// </summary>
    public ResultSuccessInvalidOperationException()
    {
    }

    /// <summary>
    ///     Result Success Invalid Operation Exception Constructor with message
    /// </summary>
    /// <param name="message"></param>
    public ResultSuccessInvalidOperationException(string message) : base(message)
    {
    }

    /// <summary>
    ///     Result Success Invalid Operation Exception Constructor with message and inner exception
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public ResultSuccessInvalidOperationException(string message, Exception innerException) : base(message,
        innerException)
    {
    }
}