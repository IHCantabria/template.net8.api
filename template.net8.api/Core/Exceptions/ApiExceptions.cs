using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Exceptions;

/// <summary>
///     Core Exception
/// </summary>
[CoreLibrary]
//TODO: Improve this class and childs Investigate How Log When This Exception is Created or Thrown.
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

[CoreLibrary]
internal sealed class BadRequestException : CoreException
{
    internal BadRequestException()
    {
    }

    internal BadRequestException(string message) : base(message)
    {
    }

    internal BadRequestException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

[CoreLibrary]
internal sealed class UnauthorizedException : CoreException
{
    internal UnauthorizedException()
    {
    }

    internal UnauthorizedException(string message) : base(message)
    {
    }

    internal UnauthorizedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

[CoreLibrary]
internal sealed class ForbiddenException : CoreException
{
    internal ForbiddenException()
    {
    }

    internal ForbiddenException(string message) : base(message)
    {
    }

    internal ForbiddenException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

[CoreLibrary]
internal sealed class NotFoundException : CoreException
{
    internal NotFoundException()
    {
    }

    internal NotFoundException(string message) : base(message)
    {
    }

    internal NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

[CoreLibrary]
internal sealed class RequestTimeoutException : CoreException
{
    internal RequestTimeoutException()
    {
    }

    internal RequestTimeoutException(string message) : base(message)
    {
    }

    internal RequestTimeoutException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

[CoreLibrary]
internal sealed class ConflictException : CoreException
{
    internal ConflictException()
    {
    }

    internal ConflictException(string message) : base(message)
    {
    }

    internal ConflictException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

[CoreLibrary]
internal sealed class GoneException : CoreException
{
    internal GoneException()
    {
    }

    internal GoneException(string message) : base(message)
    {
    }

    internal GoneException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

[CoreLibrary]
internal sealed class UnprocessableEntityException : CoreException
{
    internal UnprocessableEntityException()
    {
    }

    internal UnprocessableEntityException(string message) : base(message)
    {
    }

    internal UnprocessableEntityException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

[CoreLibrary]
internal sealed class InternalServerErrorException : CoreException
{
    internal InternalServerErrorException()
    {
    }

    internal InternalServerErrorException(string message) : base(message)
    {
    }

    internal InternalServerErrorException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

[CoreLibrary]
internal sealed class NotImplementedException : CoreException
{
    internal NotImplementedException()
    {
    }

    internal NotImplementedException(string message) : base(message)
    {
    }

    internal NotImplementedException(string message, Exception innerException) : base(
        message, innerException)
    {
    }
}