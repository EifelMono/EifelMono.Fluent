using System;
using System.Threading;
using System.Threading.Tasks;
using EifelMono.Fluent.Log;

namespace EifelMono.Fluent.Extensions
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Attention this does not cancel the original Task!!!!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), taskCompletionSource))
                if (task != await Task.WhenAny(task, taskCompletionSource.Task).ConfigureAwait(false))
                    throw new OperationCanceledException(cancellationToken);
            if (task.IsFaulted)
                throw task.Exception;
            return await task.ConfigureAwait(false);
        }

        /// <summary>
        /// Attention this does not cancel the original Task!!!!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static Task<T> WithCancellation<T>(this Task<T> task, TimeSpan timeSpan)
            => WithCancellation(task, timeSpan.AsToken());

        /// <summary>
        /// Attention this does not cancel the original Task!!!!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisValue"></param>
        /// <param name="milliSeconds"></param>
        /// <returns></returns>
        public static Task<T> WithCancellation<T>(this Task<T> thisValue, long milliSeconds)
            => WithCancellation(thisValue, TimeSpan.FromMilliseconds(milliSeconds));

        /// <summary>
        /// Attention this does not cancel the original Task!!!!
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task WithCancellation(this Task thisValue, CancellationToken cancellationToken)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), taskCompletionSource))
                if (thisValue != await Task.WhenAny(thisValue, taskCompletionSource.Task).ConfigureAwait(false))
                    throw new OperationCanceledException(cancellationToken);
            if (thisValue.IsFaulted)
                throw thisValue.Exception;
            return;
        }

        /// <summary>
        /// Attention this does not cancel the original Task!!!!
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static Task WithCancellation(this Task thisValue, TimeSpan timeSpan)
            => WithCancellation(thisValue, timeSpan.AsToken());

        /// <summary>
        /// Attention this does not cancel the original Task!!!!
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="milliSeconds"></param>
        /// <returns></returns>
        public static Task WithCancellation(this Task thisValue, long milliSeconds)
            => WithCancellation(thisValue, TimeSpan.FromMilliseconds(milliSeconds));


        public static async Task<T> AwaitWithTimeoutAsync<T>(this T thisValue, TimeSpan timeout) where T : Task
        {
            try
            {
                if (await Task.WhenAny(thisValue,
                        Task.Delay(timeout)).ConfigureAwait(false) == thisValue)
                    return thisValue;
            }
            catch (Exception ex)
            {
                if (!(ex is OperationCanceledException))
                    ex.LogException();
            }
            return default;
        }

        public static async Task<(bool Ok, T Value)> AwaitWithTimeoutSafeAsync<T>(this T thisValue, TimeSpan timeout) where T : Task
        {
            try
            {
                return (true, await thisValue.AwaitWithTimeoutAsync(timeout).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                if (!(ex is OperationCanceledException))
                    ex.LogException();
            }
            return (false, thisValue);
        }

    }
}
