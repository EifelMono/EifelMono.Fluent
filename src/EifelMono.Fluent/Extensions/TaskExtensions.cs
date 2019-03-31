using System;
using System.Threading;
using System.Threading.Tasks;

namespace EifelMono.Fluent.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), taskCompletionSource))
                if (task != await Task.WhenAny(task, taskCompletionSource.Task).ConfigureAwait(false))
                    throw new OperationCanceledException(cancellationToken);
            return await task.ConfigureAwait(false);
        }
    }
}
