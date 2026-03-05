using Serilog;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Logger;

namespace template.net8.api.Settings.Handlers;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class GlobalErrorAndExitHandlers
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public static void Register()
    {
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        AppDomain.CurrentDomain.DomainUnload += OnDomainUnload;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void OnUnhandledException(object? sender, UnhandledExceptionEventArgs args)
    {
        var ex = args.ExceptionObject as Exception
                 ?? new CoreException($"Unhandled exception object: {args.ExceptionObject}");

        MainLoggerMethods.LogCriticalUnhandledException(ex);

        if (!args.IsTerminating) return;

        MainLoggerMethods.LogShutdown();
        Log.CloseAndFlush();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs args)
    {
        MainLoggerMethods.LogUnobservedTaskException(args.Exception);
        args.SetObserved();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void OnProcessExit(object? sender, EventArgs e)
    {
        MainLoggerMethods.LogProcessExit();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void OnDomainUnload(object? sender, EventArgs e)
    {
        MainLoggerMethods.LogDomainUnload();
    }
}