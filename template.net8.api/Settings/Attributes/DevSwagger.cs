using Microsoft.AspNetCore.Mvc.ApplicationModels;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Attributes;

/// <summary>
///     Dev Swagger Attribute
/// </summary>
[CoreLibrary]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class DevSwaggerAttribute : Attribute;

internal sealed class ActionHidingConvention(string envName) : IActionModelConvention
{
    private readonly bool _isDevelopment = envName is Envs.Development or Envs.Local or Envs.Test;

    public void Apply(ActionModel action)
    {
        action.ApiExplorer.IsVisible = ShouldDisplaySwagger(action);
    }

    private bool ShouldDisplaySwagger(ICommonModel action)
    {
        // Swagger is visible by default. Only hide if DevSwagger attribute is present and _isDevelopment is false.
        return _isDevelopment || !HasDevSwaggerAttribute(action);
    }

    private static bool HasDevSwaggerAttribute(ICommonModel action)
    {
        return ControllerHasDevSwaggerAttribute(action) || ActionHasDevSwaggerAttribute(action);
    }

    private static bool ControllerHasDevSwaggerAttribute(ICommonModel action)
    {
        var actionModel = (ActionModel)action;
        return actionModel.Controller.Attributes.Any(IsDevSwaggerAttribute);
    }

    private static bool ActionHasDevSwaggerAttribute(ICommonModel action)
    {
        return action.Attributes.Any(IsDevSwaggerAttribute);
    }

    private static bool IsDevSwaggerAttribute(object attribute)
    {
        return attribute is DevSwaggerAttribute;
    }
}