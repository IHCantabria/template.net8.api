using System.Numerics;
using FluentValidation;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using template.net8.api.Business.Exceptions;
using template.net8.api.Contracts.Interfaces;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Factory;
using template.net8.api.Localize.Resources;

namespace template.net8.api.Controllers.Extensions;

[CoreLibrary]
internal static class ControllerExtensions
{
    /// <summary>
    ///     This extension method is used to convert a result to an IActionResult.
    /// </summary>
    internal static IActionResult ToActionResult<TResult, TContract>(this LanguageExt.Common.Result<TResult> result,
        MyControllerBase controller,
        ActionResultPayload<TResult, TContract> action, IStringLocalizer<ResourceMain> localizer)
    {
        return result.Match(obj => HandleSuccessResult(controller, action, obj),
            ex => ManageExceptionActionResult(ex, localizer, controller.HttpContext.Features));
    }

    private static IActionResult HandleSuccessResult<TResult, TContract>(
        ControllerBase controller,
        ActionResultPayload<TResult, TContract> action,
        TResult obj)
    {
        AddLinkHeaderIfNeeded(controller, action);

        if (action.Mapper is null)
            return HandleUnmappedAction(controller, action);

        var response = action.Mapper(obj);

        if (response is IFileContract fileResponse)
            return HandleFileContentResult(fileResponse);

        return HandleMappedAction(controller, action, response);
    }

    private static void AddLinkHeaderIfNeeded<TResult, TContract>(ControllerBase controller,
        ActionResultPayload<TResult, TContract> action)
    {
        if (action.AddLinkHeader)
            controller.Response.Headers.TryAdd("Link", action.LinkHeader);
    }

    private static IActionResult HandleUnmappedAction<TResult, TContract>(ControllerBase controller,
        ActionResultPayload<TResult, TContract> action)
    {
        if (action.IsAcceptedAction)
            return HandleAcceptedContentResult(controller, action);

        return action.IsCreatedAction
            ? HandleCreatedAtActionResult(action)
            : throw new CoreException(
                "Error Creating the Http Action Result. The mapping action for endpoint is not defined");
    }

    private static IActionResult HandleMappedAction<TResult, TContract>(ControllerBase controller,
        ActionResultPayload<TResult, TContract> action, TContract response)
    {
        if (action.IsAcceptedAction)
            return HandleAcceptedContentResult(controller, action, response);

        if (action.IsCreatedAction)
            return HandleCreatedAtActionResult(response, action);

        return HandleOkResult(controller, action, response);
    }

    private static IActionResult ManageExceptionActionResult(Exception ex,
        IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        if (ex is CoreException or ValidationException)
            return ExceptionMapper.MapExceptionToResult(ex, localizer, features);

        //Exception not Controlled
        //Important: Only Write details for Business Exceptions
        var clientProblemDetails = ProblemDetailsFactoryCore.CreateProblemDetailsInternalServerError(ex, localizer);
        features.Set(clientProblemDetails);

        return new BadRequestResult();
    }

    private static OkObjectResult HandleOkResult<TResult, TContract>(ControllerBase controller,
        ActionResultPayload<TResult, TContract> action, TContract response)
    {
        if (!action.AddLocationHeader)
            return new OkObjectResult(response);

        var dictionary = new RouteValueDictionary();
        var type = typeof(TContract);

        var value = type.GetProperty(action.ActionParam!.Value.Item1)!.GetValue(response);

        dictionary.Add(action.ActionParam.Value.Item2, value?.ToString());

        var locationUrl = controller.Url.Action(
            action.ActionPath?.Item2,
            action.ActionPath?.Item1,
            dictionary,
            controller.Request.Scheme);

        controller.Response.Headers.TryAdd("Location", locationUrl);

        return new OkObjectResult(response);
    }

    private static CreatedAtActionResult HandleCreatedAtActionResult<TResult, TContract>(TContract response,
        ActionResultPayload<TResult, TContract> action)
    {
        if (!action.AddLocationHeader)
            return new CreatedAtActionResult(null, null, null, response);

        var dictionary = new RouteValueDictionary();
        var type = typeof(TContract);
        var value = type.GetProperty(action.ActionParam!.Value.Item1)!.GetValue(response);

        dictionary.Add(action.ActionParam.Value.Item2, value!.ToString());
        return action.IsEmptyResponse
            ? new CreatedAtActionResult(action.ActionPath?.Item2, action.ActionPath?.Item1, dictionary, null)
            : new CreatedAtActionResult(action.ActionPath?.Item2, action.ActionPath?.Item1, dictionary, response);
    }

    private static CreatedAtActionResult HandleCreatedAtActionResult<TResult, TContract>(
        ActionResultPayload<TResult, TContract> action)
    {
        if (!action.AddLocationHeader)
            return new CreatedAtActionResult(null, null, null, null);

        var dictionary = new RouteValueDictionary
            { { action.ActionParam!.Value.Item2, action.ActionParam.Value.Item1 } };
        return new CreatedAtActionResult(action.ActionPath?.Item2, action.ActionPath?.Item1, dictionary, null);
    }

    private static AcceptedResult HandleAcceptedContentResult<TResult, TContract>(ControllerBase controller,
        ActionResultPayload<TResult, TContract> action)
    {
        if (!action.AddLocationHeader)
            return new AcceptedResult();

        var dictionary = new RouteValueDictionary
            { { action.ActionParam!.Value.Item2, action.ActionParam.Value.Item1 } };

        var locationUrl = controller.Url.Action(
            action.ActionPath?.Item2,
            action.ActionPath?.Item1,
            dictionary,
            controller.Request.Scheme);

        return new AcceptedResult(locationUrl, null);
    }

    private static AcceptedResult HandleAcceptedContentResult<TResult, TContract>(ControllerBase controller,
        ActionResultPayload<TResult, TContract> action, TContract response)
    {
        if (!action.AddLocationHeader)
            return new AcceptedResult((string?)null, response);

        var dictionary = new RouteValueDictionary();
        var type = typeof(TContract);

        var value = type.GetProperty(action.ActionParam!.Value.Item1)!.GetValue(response);

        dictionary.Add(action.ActionParam.Value.Item2, value?.ToString());

        var locationUrl = controller.Url.Action(
            action.ActionPath?.Item2,
            action.ActionPath?.Item1,
            dictionary,
            controller.Request.Scheme);

        return action.IsEmptyResponse
            ? new AcceptedResult(locationUrl, null)
            : new AcceptedResult(locationUrl, response);
    }

    private static FileContentResult HandleFileContentResult(IFileContract response)
    {
        return new FileContentResult(response.Data.ToArray(), response.ContentType)
        {
            FileDownloadName = response.FileName
        };
    }
}

[CoreLibrary]
internal sealed record ActionResultPayload<TResult, TContract> : IEqualityOperators<
    ActionResultPayload<TResult, TContract>, ActionResultPayload<TResult, TContract>, bool>
{
    internal bool AddLocationHeader { get; init; }
    internal bool AddLinkHeader { get; init; }
    internal bool IsEmptyResponse { get; init; }
    internal bool IsAcceptedAction { get; init; }
    internal bool IsCreatedAction { get; init; }
    internal string? LinkHeader { get; init; }
    internal (string, string)? ActionParam { get; init; }
    internal (string, string)? ActionPath { get; init; }
    internal Func<TResult, TContract>? Mapper { get; init; }

    internal static ActionResultPayload<TResult, TContract> Ok(
        Func<TResult, TContract> mapper,
        string? linkHeader = null)
    {
        return new ActionResultPayload<TResult, TContract>
        {
            Mapper = mapper,
            AddLinkHeader = linkHeader is not null,
            LinkHeader = linkHeader
        };
    }

    internal static ActionResultPayload<TResult, TContract> OkWithLocation(
        (string ControllerName, string ActionName) actionPath,
        (string PropertyName, string RouteParamName) actionParam,
        Func<TResult, TContract> mapper,
        string? linkHeader = null)
    {
        return new ActionResultPayload<TResult, TContract>
        {
            Mapper = mapper,
            AddLocationHeader = true,
            ActionParam = actionParam,
            ActionPath = actionPath,
            AddLinkHeader = linkHeader is not null,
            LinkHeader = linkHeader
        };
    }

    internal static ActionResultPayload<TRes, TFile> File<TRes, TFile>(
        Func<TRes, TFile> mapper,
        string? linkHeader = null) where TFile : IFileContract
    {
        return new ActionResultPayload<TRes, TFile>
        {
            Mapper = mapper,
            AddLinkHeader = linkHeader is not null,
            LinkHeader = linkHeader
        };
    }

    internal static ActionResultPayload<TResult, TContract> Created(
        Func<TResult, TContract> mapper,
        string? linkHeader = null)
    {
        return new ActionResultPayload<TResult, TContract>
        {
            Mapper = mapper,
            IsCreatedAction = true,
            AddLinkHeader = linkHeader is not null,
            LinkHeader = linkHeader
        };
    }

    internal static ActionResultPayload<TResult, TContract> CreatedWithLocation(
        (string ControllerName, string ActionName) actionPath,
        (string PropertyName, string RouteParamName) actionParam,
        Func<TResult, TContract> mapper,
        string? linkHeader = null)
    {
        return new ActionResultPayload<TResult, TContract>
        {
            Mapper = mapper,
            IsCreatedAction = true,
            AddLocationHeader = true,
            ActionParam = actionParam,
            ActionPath = actionPath,
            AddLinkHeader = linkHeader is not null,
            LinkHeader = linkHeader
        };
    }

    internal static ActionResultPayload<TResult, TContract> CreatedEmpty(
        string? linkHeader = null)
    {
        return new ActionResultPayload<TResult, TContract>
        {
            IsCreatedAction = true,
            IsEmptyResponse = true,
            AddLinkHeader = linkHeader is not null,
            LinkHeader = linkHeader
        };
    }

    internal static ActionResultPayload<TResult, TContract> CreatedEmptyWithLocation(
        (string ControllerName, string ActionName) actionPath,
        (string PropertyName, string RouteParamName) actionParam,
        string? linkHeader = null)
    {
        return new ActionResultPayload<TResult, TContract>
        {
            IsCreatedAction = true,
            IsEmptyResponse = true,
            AddLocationHeader = true,
            ActionParam = actionParam,
            ActionPath = actionPath,
            AddLinkHeader = linkHeader is not null,
            LinkHeader = linkHeader
        };
    }

    internal static ActionResultPayload<TResult, TContract> CreatedEmptyWithLocation(
        (string ControllerName, string ActionName) actionPath,
        (string PropertyName, string RouteParamName) actionParam,
        Func<TResult, TContract> mapper,
        string? linkHeader = null)
    {
        return new ActionResultPayload<TResult, TContract>
        {
            Mapper = mapper,
            IsCreatedAction = true,
            IsEmptyResponse = true,
            AddLocationHeader = true,
            ActionParam = actionParam,
            ActionPath = actionPath,
            AddLinkHeader = linkHeader is not null,
            LinkHeader = linkHeader
        };
    }

    internal static ActionResultPayload<TResult, TContract> Accepted(
        Func<TResult, TContract> mapper,
        string? linkHeader = null)
    {
        return new ActionResultPayload<TResult, TContract>
        {
            Mapper = mapper,
            IsAcceptedAction = true,
            AddLinkHeader = linkHeader is not null,
            LinkHeader = linkHeader
        };
    }

    internal static ActionResultPayload<TResult, TContract> AcceptedWithLocation(
        (string ControllerName, string ActionName) actionPath,
        (string PropertyName, string RouteParamName) actionParam,
        Func<TResult, TContract> mapper,
        string? linkHeader = null)
    {
        return new ActionResultPayload<TResult, TContract>
        {
            Mapper = mapper,
            IsAcceptedAction = true,
            AddLocationHeader = true,
            ActionParam = actionParam,
            ActionPath = actionPath,
            AddLinkHeader = linkHeader is not null,
            LinkHeader = linkHeader
        };
    }

    internal static ActionResultPayload<TResult, TContract> AcceptedEmpty(
        string? linkHeader = null)
    {
        return new ActionResultPayload<TResult, TContract>
        {
            IsAcceptedAction = true,
            IsEmptyResponse = true,
            AddLinkHeader = linkHeader is not null,
            LinkHeader = linkHeader
        };
    }

    internal static ActionResultPayload<TResult, TContract> AcceptedEmptyWithLocation(
        (string ControllerName, string ActionName) actionPath,
        (string PropertyName, string RouteParamName) actionParam,
        string? linkHeader = null)
    {
        return new ActionResultPayload<TResult, TContract>
        {
            IsAcceptedAction = true,
            IsEmptyResponse = true,
            AddLocationHeader = true,
            ActionParam = actionParam,
            ActionPath = actionPath,
            AddLinkHeader = linkHeader is not null,
            LinkHeader = linkHeader
        };
    }

    internal static ActionResultPayload<TResult, TContract> AcceptedEmptyWithLocation(
        (string ControllerName, string ActionName) actionPath,
        (string PropertyName, string RouteParamName) actionParam,
        Func<TResult, TContract> mapper,
        string? linkHeader = null)
    {
        return new ActionResultPayload<TResult, TContract>
        {
            Mapper = mapper,
            IsAcceptedAction = true,
            IsEmptyResponse = true,
            AddLocationHeader = true,
            ActionParam = actionParam,
            ActionPath = actionPath,
            AddLinkHeader = linkHeader is not null,
            LinkHeader = linkHeader
        };
    }
}