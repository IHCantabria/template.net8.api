using Serilog;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Logger;

namespace template.net8.api.Settings.Handlers;

[CoreLibrary]
internal static class GlobalErrorAndExitHandlers
{
    public static void Register()
    {
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        AppDomain.CurrentDomain.DomainUnload += OnDomainUnload;
    }

    private static void OnUnhandledException(object? sender, UnhandledExceptionEventArgs args)
    {
        var ex = args.ExceptionObject as Exception
                 ?? new CoreException($"Unhandled exception object: {args.ExceptionObject}");

        MainLoggerMethods.LogCriticalUnhandledException(ex);

        if (!args.IsTerminating) return;

        MainLoggerMethods.LogShutdown();
        Log.CloseAndFlush();
    }

    private static void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs args)
    {
        MainLoggerMethods.LogUnobservedTaskException(args.Exception);
        args.SetObserved();
    }

    private static void OnProcessExit(object? sender, EventArgs e)
    {
        MainLoggerMethods.LogProcessExit();
    }

    private static void OnDomainUnload(object? sender, EventArgs e)
    {
        MainLoggerMethods.LogDomainUnload();
    }
}