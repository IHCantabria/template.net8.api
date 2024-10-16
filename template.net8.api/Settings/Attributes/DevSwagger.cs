using Microsoft.AspNetCore.Mvc.ApplicationModels;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Settings.Attributes;

/// <summary>
/// </summary>
[CoreLibrary]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class DevSwaggerAttribute : Attribute;

internal sealed class ActionHidingConvention(string envName) : IActionModelConvention
{
    private readonly bool _isDevelopment = envName is Envs.Development or Envs.Local;

    public void Apply(ActionModel action)
    {
        action.ApiExplorer.IsVisible = ShouldDisplaySwagger(action);
    }

    private bool ShouldDisplaySwagger(ActionModel action)
    {
        // Swagger is visible by default. Only hide if DevSwagger attribute is present and _isDevelopment is false.
        return _isDevelopment || !HasDevSwaggerAttribute(action);
    }

    private static bool HasDevSwaggerAttribute(ActionModel action)
    {
        return ControllerHasDevSwaggerAttribute(action) || ActionHasDevSwaggerAttribute(action);
    }

    private static bool ControllerHasDevSwaggerAttribute(ActionModel action)
    {
        return action.Controller.Attributes.Any(IsDevSwaggerAttribute);
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