using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace template.net8.api.Settings.Attributes;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
internal sealed class DevSwaggerAttribute : Attribute;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class ActionHidingConvention(string envName) : IActionModelConvention
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly bool _isDevelopment = envName is Envs.Development or Envs.Local or Envs.Test;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public void Apply(ActionModel action)
    {
        action.ApiExplorer.IsVisible = ShouldDisplaySwagger(action);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private bool ShouldDisplaySwagger(ICommonModel action)
    {
        // Swagger is visible by default. Only hide if DevSwagger attribute is present and _isDevelopment is false.
        return _isDevelopment || !HasDevSwaggerAttribute(action);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static bool HasDevSwaggerAttribute(ICommonModel action)
    {
        return ControllerHasDevSwaggerAttribute(action) || ActionHasDevSwaggerAttribute(action);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static bool ControllerHasDevSwaggerAttribute(ICommonModel action)
    {
        var actionModel = (ActionModel)action;
        return actionModel.Controller.Attributes.Any(IsDevSwaggerAttribute);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static bool ActionHasDevSwaggerAttribute(ICommonModel action)
    {
        return action.Attributes.Any(IsDevSwaggerAttribute);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static bool IsDevSwaggerAttribute(object attribute)
    {
        return attribute is DevSwaggerAttribute;
    }
}