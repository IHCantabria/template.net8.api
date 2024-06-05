using template.net8.api.Core.Attributes;

namespace template.net8.api.Business.Messages;

[CoreLibrary]
//TODO IMPORTANT: REFACTOR ALL the hardcoded msg to use properties inside this class 
internal static class MessageDefinitions
{
    internal const string DejaVu =
        "A Dejavu is usually a glitch in the matrix, it happens when they change something.";

    internal const string GenericServerError = "There is something wrong with our bloody servers today.";

    internal const string GenericClientError = "There is something wrong with your request.";
}