﻿using System.Numerics;
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
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="CoreException">
    ///     Error Creating the Http Action Result. Error mapping action endpoint response to
    ///     resource
    /// </exception>
    internal static IActionResult ToActionResult<TResult, TContract>(this LanguageExt.Common.Result<TResult> result,
        ActionResultPayload<TResult, TContract> action, IStringLocalizer<ResourceMain> localizer,
        IFeatureCollection features)
    {
        return result.Match(obj =>
            {
                switch (action.Mapper)
                {
                    case null when action.IsEmptyResult:
                        return HandleAcceptedContentResult();
                    case null:
                        throw new CoreException(
                            "Error Creating the Http Action Result. The mapping action for endpoint is not defined");
                }

                var response = action.Mapper(obj);
                if (response is null)
                    throw new CoreException(
                        "Error Creating the Http Action Result. Error mapping action endpoint response to resource");

                if (response is IFileContract fileResponse) return HandleFileContentResult(fileResponse);

                if (action.AddLocationHeader)
                    return HandleCreatedAtActionResult(response, action);

                return new OkObjectResult(response);
            },
            ex => ManageExceptionActionResult(ex, localizer, features));
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

    private static CreatedAtActionResult HandleCreatedAtActionResult<TResult, TContract>(TContract response,
        ActionResultPayload<TResult, TContract> action)
    {
        var dictionary = new RouteValueDictionary();
        var type = typeof(TContract);
        var propertyDelegate = (Func<TContract, object>)Delegate.CreateDelegate(
            typeof(Func<TContract, object>), null, type.GetProperty(action.ActionParams.Item1)!.GetMethod!);
        var value = propertyDelegate(response);

        dictionary.Add(action.ActionParams.Item2, value.ToString());
        return action.IsEmptyResult
            ? new CreatedAtActionResult(action.ActionPath?.Item2, action.ActionPath?.Item1, dictionary, null)
            : new CreatedAtActionResult(action.ActionPath?.Item2, action.ActionPath?.Item1, dictionary, response);
    }

    private static AcceptedResult HandleAcceptedContentResult()
    {
        return new AcceptedResult();
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
    public ActionResultPayload(Func<TResult, TContract> mapper)
    {
        Mapper = mapper;
    }

    public ActionResultPayload()
    {
        IsEmptyResult = true;
    }

    public ActionResultPayload(Func<TResult, TContract> mapper, (string, string) actionPath,
        (string, string) actionKeyParam, bool isEmptyResult = false)
    {
        Mapper = mapper;
        ActionPath = actionPath;
        ActionParams = actionKeyParam;
        AddLocationHeader = true;
        IsEmptyResult = isEmptyResult;
    }

    internal bool AddLocationHeader { get; }
    internal bool IsEmptyResult { get; }
    internal (string, string) ActionParams { get; }
    internal (string, string)? ActionPath { get; }
    internal Func<TResult, TContract>? Mapper { get; }
}