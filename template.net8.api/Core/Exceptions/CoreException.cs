using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Exceptions;

/// <summary>
///     Core Exception
/// </summary>
[CoreLibrary]
//TODO: Improve this class and child Investigate How Log When This Exception is Created or Thrown. Think about the diference between this an Business Exception class
public class CoreException : Exception
{
    /// <summary>
    ///     Core Exception Constructor
    /// </summary>
    public CoreException()
    {
    }

    /// <summary>
    ///     Core Exception Constructor with message
    /// </summary>
    /// <param name="message"></param>
    public CoreException(string message) : base(message)
    {
    }

    /// <summary>
    ///     Core Exception Constructor with message and inner exception
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public CoreException(string message, Exception innerException) : base(message, innerException)
    {
    }
}