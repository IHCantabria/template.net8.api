using template.net8.api.Core.Attributes;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Extensions;

namespace template.net8.api.Core.Parallel;

[CoreLibrary]
internal static class ParallelUtils
{
    internal static Task ExecuteDependentInParallelAsync(IEnumerable<Task> tasks,
        CancellationTokenSource cts)
    {
        return HandleTaskDependentCompletionAsync(tasks, cts);
    }

    internal static Task<IEnumerable<T>> ExecuteDependentInParallelAsync<T>(IEnumerable<Task<T>> tasks,
        CancellationTokenSource cts)
    {
        return HandleTaskDependentCompletionAsync(tasks, cts);
    }

    /// <exception cref="ResultSuccessInvalidOperationException">
    ///     Result is not a success! Use ExtractException method instead
    ///     and Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractException method
    /// </exception>
    /// <exception cref="ResultFaultedInvalidOperationException">
    ///     Result is not a failure! Use ExtractData method instead and
    ///     Check the state of Result with IsSuccess or IsFaulted before use this method or ExtractData method
    /// </exception>
    internal static async Task<LanguageExt.Common.Result<IEnumerable<T>>> ExecuteDependentInParallelAsync<T>(
        IEnumerable<Task<LanguageExt.Common.Result<T>>> tasks,
        CancellationTokenSource cts)
    {
        var taskResults = await HandleTaskDependentCompletionAsync(tasks, cts).ConfigureAwait(false);
        return taskResults.IsSuccess
            ? new LanguageExt.Common.Result<IEnumerable<T>>(taskResults.ExtractData())
            : new LanguageExt.Common.Result<IEnumerable<T>>(taskResults.ExtractException());
    }

    private static async Task HandleTaskDependentCompletionAsync(IEnumerable<Task> tasks,
        CancellationTokenSource cts)
    {
        var tasksList = tasks.ToList();
        while (tasksList.Count > 0)
        {
            // Wait for any task to complete
            var completedTask = await Task.WhenAny(tasksList).ConfigureAwait(false);

            // Remove the completed task from the list
            tasksList.Remove(completedTask);

            // Check if the completed task is Faulted
            if (completedTask.IsFaulted)
                // cancel all other tasks
                await cts.CancelAsync().ConfigureAwait(false);
        }
    }

    private static async Task<IEnumerable<T>> HandleTaskDependentCompletionAsync<T>(IEnumerable<Task<T>> tasks,
        CancellationTokenSource cts)
    {
        var completedResults = new List<T>();
        var tasksList = tasks.ToList();
        while (tasksList.Count > 0)
        {
            // Wait for any task to complete
            var completedTask = await Task.WhenAny(tasksList).ConfigureAwait(false);

            // Remove the completed task from the list
            tasksList.Remove(completedTask);

            var taskResult = await completedTask.ConfigureAwait(false);

            // Check if the completed task is Faulted
            if (completedTask.IsFaulted)
            {
                // cancel all other tasks
                await cts.CancelAsync().ConfigureAwait(false);
                return completedResults;
            }

            completedResults.Add(taskResult);
        }

        return completedResults;
    }

    private static async Task<LanguageExt.Common.Result<IEnumerable<T>>> HandleTaskDependentCompletionAsync<T>(
        IEnumerable<Task<LanguageExt.Common.Result<T>>> tasks,
        CancellationTokenSource cts)
    {
        var completedResults = new List<T>();
        var tasksList = tasks.ToList();
        while (tasksList.Count > 0)
        {
            // Wait for any task to complete
            var completedTask = await Task.WhenAny(tasksList).ConfigureAwait(false);

            // Remove the completed task from the list
            tasksList.Remove(completedTask);

            var taskResult = await completedTask.ConfigureAwait(false);

            // Check if the completed task is Faulted or the result of the completed task is Faulted
            if (completedTask.IsFaulted || taskResult.IsFaulted)
            {
                // cancel all other tasks
                await cts.CancelAsync().ConfigureAwait(false);
                return new LanguageExt.Common.Result<IEnumerable<T>>(completedTask.IsFaulted
                    ? completedTask.Exception
                    : taskResult.ExtractException());
            }

            completedResults.Add(taskResult.ExtractData());
        }

        return completedResults;
    }

    internal static Task ExecuteIndependentInParallelAsync(IEnumerable<Task> tasks,
        CancellationTokenSource cts)
    {
        return HandleTaskIndependentCompletionAsync(tasks);
    }

    internal static Task<IEnumerable<LanguageExt.Common.Result<T>>> ExecuteIndependentInParallelAsync<T>(
        IEnumerable<Task<LanguageExt.Common.Result<T>>> tasks,
        CancellationTokenSource cts)
    {
        return HandleTaskIndependentCompletionAsync(tasks);
    }

    private static async Task HandleTaskIndependentCompletionAsync(IEnumerable<Task> tasks)
    {
        var tasksList = tasks.ToList();
        while (tasksList.Count > 0)
        {
            // Wait for any task to complete
            var completedTask = await Task.WhenAny(tasksList).ConfigureAwait(false);

            // Remove the completed task from the list
            tasksList.Remove(completedTask);
        }
    }

    private static async Task<IEnumerable<LanguageExt.Common.Result<T>>> HandleTaskIndependentCompletionAsync<T>(
        IEnumerable<Task<LanguageExt.Common.Result<T>>> tasks)
    {
        var completedResults = new List<LanguageExt.Common.Result<T>>();
        var tasksList = tasks.ToList();
        while (tasksList.Count > 0)
        {
            // Wait for any task to complete
            var completedTask = await Task.WhenAny(tasksList).ConfigureAwait(false);

            // Remove the completed task from the list
            tasksList.Remove(completedTask);

            var taskResult = await completedTask.ConfigureAwait(false);


            completedResults.Add(taskResult);
        }

        return completedResults;
    }
}