using Microsoft.AspNetCore.Mvc;
using template.net8.api.Core.Attributes;
using template.net8.api.Settings.Extensions;
using template.net8.api.Settings.Handlers;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     Exceptions Control Installer
/// </summary>
[CoreLibrary]
public sealed class ExceptionsControlInstaller : IServiceInstaller
{
    /// <summary>
    ///     Load order of the service installer
    /// </summary>
    public short LoadOrder => 2;

    /// <summary>
    ///     Install Exception Handler Service
    /// </summary>
    /// <param name="builder"></param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddExceptionHandler<GlobalExceptionHandlerControl>();
        builder.Services.AddProblemDetails(setup =>
        {
            setup.CustomizeProblemDetails = ctx => { CustomizeProblemDetails(builder.Environment, ctx); };
        });
        return Task.CompletedTask;
    }

    private static void CustomizeProblemDetails(IHostEnvironment env, ProblemDetailsContext ctx)
    {
        var httpContextProblemDetails =
            ctx.HttpContext.Features.Get<ProblemDetails>();
        ctx.AddInstanceField();
        ctx.AddMethodField();
        ctx.AddTraceIdField();
        ctx.AddCodeField();
        if (httpContextProblemDetails is not null) ctx.UseHttpContextProblemDetails(httpContextProblemDetails);

        //Details Error Removed for server errors in Production
        ctx.HiddenDetails(env);
    }
}