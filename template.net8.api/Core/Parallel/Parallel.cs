using LanguageExt.Common;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Exceptions;
using template.net8.api.Core.Extensions;

namespace template.net8.api.Core.Parallel;

[CoreLibrary]
internal static class ParallelUtils
{
    /// <exception cref="ResultSuccessInvalidOperationException">
    ///     Result is not a success! Use ExtractException method instead and Check the
    ///     state of Result with IsSuccess or IsFaulted before use this method or ExtractException method
    /// </exception>
    /// <exception cref="ResultFaultedInvalidOperationException">
    ///     Result is not a failure! Use ExtractData method instead and Check the state
    ///     of Result with IsSuccess or IsFaulted before use this method or ExtractData method
    /// </exception>
    internal static async Task<Result<IEnumerable<T>>> ExecuteInParallelAsync<T>(IEnumerable<Task<Result<T>>> tasks,
        CancellationTokenSource cts)
    {
        var taskResults = await HandleTaskCompletionAsync(tasks, cts).ConfigureAwait(false);
        return taskResults.IsSuccess
            ? new Result<IEnumerable<T>>(taskResults.ExtractData())
            : new Result<IEnumerable<T>>(taskResults.ExtractException());
    }

    private static async Task<Result<IEnumerable<T>>> HandleTaskCompletionAsync<T>(IEnumerable<Task<Result<T>>> tasks,
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

            // Check if the completed task is Faulted
            if (completedTask.IsFaulted)
            {
                // cancel all other tasks
                await cts.CancelAsync().ConfigureAwait(false);
                return new Result<IEnumerable<T>>(completedTask.Exception);
            }

            // Check if the result of the completed task is Faulted
            var taskResult = await completedTask.ConfigureAwait(false);

            if (taskResult.IsSuccess)
            {
                completedResults.Add(taskResult.ExtractData());
                continue; //If the task is not faulted, continue with the next task
            }

            // cancel all other tasks
            await cts.CancelAsync().ConfigureAwait(false);
            return new Result<IEnumerable<T>>(taskResult.ExtractException());
        }

        return new Result<IEnumerable<T>>(completedResults);
    }
}