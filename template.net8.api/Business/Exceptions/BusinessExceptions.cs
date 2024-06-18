using template.net8.api.Core.Attributes;
using template.net8.api.Core.Exceptions;

namespace template.net8.api.Business.Exceptions;

//TODO: Improve this class. Important! Investigate How Log When This Exception is Created or Thrown
[CoreLibrary]
internal class BusinessException : CoreException
{
    internal BusinessException()
    {
    }

    internal BusinessException(string message) : base(message)
    {
    }

    internal BusinessException(string message, Exception innerException) : base(message, innerException)
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