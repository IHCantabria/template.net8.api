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