using template.net8.api.Core.Attributes;
using template.net8.api.Core.Exceptions;

namespace template.net8.api.Business.Exceptions;

[CoreLibrary]
internal abstract class BusinessException : CoreException
{
    protected BusinessException()
    {
    }

    protected BusinessException(string message) : base(message)
    {
    }

    protected BusinessException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

[CoreLibrary]
internal sealed class BadRequestException : BusinessException
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
internal sealed class UnauthorizedException : BusinessException
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
internal sealed class ForbiddenException : BusinessException
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
internal sealed class NotFoundException : BusinessException
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
internal sealed class RequestTimeoutException : BusinessException
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
internal sealed class ConflictException : BusinessException
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
internal sealed class GoneException : BusinessException
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
internal sealed class UnprocessableEntityException : BusinessException
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
internal sealed class InternalServerErrorException : BusinessException
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
internal sealed class NotImplementedException : BusinessException
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