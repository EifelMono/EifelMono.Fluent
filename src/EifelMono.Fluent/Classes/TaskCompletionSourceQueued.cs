using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using EifelMono.Fluent.Log;

namespace EifelMono.Fluent.Classes
{
    public class TaskCompletionSourceQueued<T> where T : object
    {
        protected ConcurrentQueue<T> QueuedData = new ConcurrentQueue<T>();
        protected object CompletionSourceLock = new object();
        protected TaskCompletionSource<bool> CompletionSource;

        public TaskCompletionSourceQueued()
        {
            Init();
        }
        protected void Init()
        {
            lock (CompletionSourceLock)
                CompletionSource = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        public Action<T> OnNewData { get; set; }

        public T LastData { get; set; } = default;

        public void NewData(T newData, CancellationToken cancellationToken = default)
        {
            LastData = newData;
            QueuedData.Enqueue(newData);
            var setResult = false;
            lock (CompletionSourceLock)
                try
                {
                    setResult = CompletionSource.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
            try
            {
                OnNewData?.Invoke(newData);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            #region if not possible to notify, try again  
            Task.Run(async () =>
            {
                try
                {
                    if (!setResult)
                    {
                        var tries = 0;
                        while (tries++ < 10 && !cancellationToken.IsCancellationRequested)
                        {
                            try
                            {
                                if (CompletionSource.TrySetResult(true))
                                    break;
                            }
                            catch (OperationCanceledException)
                            {
                                return;
                            }
                            catch (Exception ex)
                            {
                                ex.LogException();
                            }
                            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
            });
            #endregion
        }

        protected async ValueTask<(bool Ok, T Data, Exception Exception)> WaitDataInternalAsync(T waitData, bool wait, CancellationToken cancellationToken = default)
        {
            if (wait && waitData is null)
                return (false, default, new ArgumentNullException());
            cancellationToken.Register(() => { CompletionSource.TrySetCanceled(); });
            T data = default;
            bool finished = false;
            while (!finished)
            {
                if (QueuedData.TryDequeue(out data))
                    finished = wait ? data != null && data.Equals(waitData) : true;
                else
                {
                    try
                    {
                        await (CompletionSource.Task).ConfigureAwait(false);
                        Init();
                    }
                    catch (Exception ex)
                    {
                        return (false, default, ex);
                    }
                }
                if (cancellationToken.IsCancellationRequested)
                    return (false, default, new Exception());
            }
            return (true, data, null);
        }

        public async ValueTask<(bool Ok, T Data, Exception Exception)> WaitDataAsync(CancellationToken cancellationToken = default)
            => await WaitDataInternalAsync(default, false, cancellationToken).ConfigureAwait(false);

        public async ValueTask<(bool Ok, T Data, Exception Exception)> WaitDataAsync(T waitData, CancellationToken cancellationToken = default)
            => await WaitDataInternalAsync(waitData, true, cancellationToken).ConfigureAwait(false);

    }
}
