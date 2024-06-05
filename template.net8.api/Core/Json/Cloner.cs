using System.Text.Json;
using LanguageExt;
using LanguageExt.Common;
using template.net8.Api.Core.Attributes;
using template.net8.Api.Core.Exceptions;
using template.net8.Api.Core.Extensions;

namespace template.net8.Api.Core.Json;

[CoreLibrary]
internal static class Cloner
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions().AddCoreOptions();

    /// <exception cref="ResultFaultedInvalidOperationException">
    ///     Result is not a failure! Use ExtractData method instead and Check the state
    ///     of Result with IsSuccess or IsFaulted before use this method or ExtractData method
    /// </exception>
    /// <exception cref="ResultSuccessInvalidOperationException">
    ///     Result is not a success! Use ExtractException method instead and Check the
    ///     state of Result with IsSuccess or IsFaulted before use this method or ExtractException method
    /// </exception>
    internal static Try<T> DeepClone<T>(T obj)
    {
        return () =>
        {
            var serializeResult = Serialize(obj).Try();
            return serializeResult.IsSuccess
                ? Deserialize<T>(serializeResult.ExtractData()).Try()
                : new Result<T>(serializeResult.ExtractException());
        };
    }

    private static Try<string> Serialize<T>(T obj)
    {
        return () => JsonSerializer.Serialize(obj, Options);
    }

    private static Try<T> Deserialize<T>(string stringObj)
    {
        return () =>
        {
            var obj = JsonSerializer.Deserialize<T>(stringObj, Options);
            return obj is not null
                ? obj
                : new Result<T>(new CoreException("The response received from the Deserializer is empty"));
        };
    }
}