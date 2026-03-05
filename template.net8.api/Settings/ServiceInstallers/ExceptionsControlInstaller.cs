using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using template.net8.api.Settings.Extensions;
using template.net8.api.Settings.Handlers;
using template.net8.api.Settings.Interfaces;

namespace template.net8.api.Settings.ServiceInstallers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[UsedImplicitly]
internal sealed class ExceptionsControlInstaller : IServiceInstaller
{
    /// <inheritdoc cref="IServiceInstaller.LoadOrder" />
    public short LoadOrder => 2;

    /// <inheritdoc cref="IServiceInstaller.InstallServiceAsync" />
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public Task InstallServiceAsync(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddExceptionHandler<GlobalExceptionHandlerControl>();
        builder.Services.AddProblemDetails(setup =>
            setup.CustomizeProblemDetails = ctx => CustomizeProblemDetails(builder.Environment, ctx));
        return Task.CompletedTask;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void CustomizeProblemDetails(IHostEnvironment env, ProblemDetailsContext ctx)
    {
        var httpContextProblemDetails =
            ctx.HttpContext.Features.Get<ProblemDetails>();
        ctx.AddInstanceField();
        ctx.AddMethodField();
        ctx.AddRequestIdField();
        ctx.AddTraceIdField();
        ctx.AddCodeField();
        if (httpContextProblemDetails is not null) ctx.UseHttpContextProblemDetails(httpContextProblemDetails);

        //Details Error Removed for server errors in Production
        ctx.HiddenDetails(env);
    }
}