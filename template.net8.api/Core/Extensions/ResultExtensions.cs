using template.net8.api.Core.Attributes;
using template.net8.api.Core.Exceptions;

namespace template.net8.api.Core.Extensions;

[CoreLibrary]
internal static class ResultExtensions
{
    /// <exception cref="ResultFaultedInvalidOperationException">
    ///     Result is not a failure! Use ExtractData method instead and
    ///     Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractData method
    /// </exception>
    internal static Exception ExtractException<T>(this LanguageExt.Common.Result<T> result)
    {
        if (result.IsSuccess)
            throw new ResultFaultedInvalidOperationException(
                "Result is not a failure! Use ExtractData method instead and Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractData method");

        Exception exception = default!;
        result.IfFail(ex => exception = ex);
        return exception;
    }

    /// <exception cref="ResultSuccessInvalidOperationException">
    ///     Result is not a success! Use ExtractException method instead
    ///     and Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractException method
    /// </exception>
    internal static T ExtractData<T>(this LanguageExt.Common.Result<T> result)
    {
        if (result.IsFaulted)
            throw new ResultSuccessInvalidOperationException(
                "Result is not a success! Use ExtractException method instead and Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractException method");

        T data = default!;
        result.IfSucc(d => data = d);
        return data;
    }
}