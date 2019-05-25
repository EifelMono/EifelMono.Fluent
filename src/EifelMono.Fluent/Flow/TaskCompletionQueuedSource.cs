using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EifelMono.Fluent.Log;

namespace EifelMono.Fluent.Flow
{
    public class TaskCompletionQueuedSource<T> where T : object
    {
        public ConcurrentExtendedQueue<T> Items = new ConcurrentExtendedQueue<T>();
        protected object CompletionSourceLock = new object();
        protected TaskCompletionSource<bool> CompletionSource;

        public TaskCompletionQueuedSource()
        {
            Init();
        }

        public TaskCompletionQueuedSource(IEnumerable<T> collection) : this()
        {
            Items = new ConcurrentExtendedQueue<T>(collection);
            if (collection.LastOrDefault() is T value && value != null)
                LastData = value;
        }

        protected void Init()
        {
            lock (CompletionSourceLock)
                CompletionSource = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        }
        public void TrySetCanceled()
        {
            lock (CompletionSourceLock)
                CompletionSource.TrySetCanceled();
        }

        public Action<T> OnNewData { get; set; }

        public T LastData { get; set; } = default;

        public void NewData(T newData, CancellationToken cancellationToken = default)
        {
            LastData = newData;
            Items.Enqueue(newData);
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

        protected async Task<(bool Ok, T Value, Exception Exception)> WaitValueAsync(T waitValue, bool wait, params CancellationToken[] cancellationTokens)
        {
            if (wait && waitValue is null)
                return (false, default, new AggregateException());
            CancellationToken cancellationToken;
            cancellationToken.Register(() => { CompletionSource.TrySetCanceled(); });
            var tokenRegisters = new List<CancellationTokenRegistration>();
            try
            {
                foreach (var ct in cancellationTokens)
                    tokenRegisters.Add(ct.Register(() => { CompletionSource.TrySetCanceled(); }));

                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationTokens.ToList().Append(cancellationToken).ToArray());
                {
                    T value = default;
                    bool finished = false;
                    while (!finished)
                    {
                        if (Items.TryDequeue(out value))
                            finished = wait ? value != null && value.Equals(waitValue) : true;
                        else
                        {
                            try
                            {
                                await CompletionSource.Task.ConfigureAwait(false);
                                Init();
                            }
                            catch (Exception ex)
                            {
                                return (false, default, ex);
                            }
                        }
                        if (linkedCts.IsCancellationRequested)
                            return (false, default, new OperationCanceledException());
                    }
                    return (true, value, null);
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            finally
            {
                try
                {
                    foreach (var tr in tokenRegisters)
                        tr.Dispose();
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
            }
            return (false, default, new Exception());
        }

        public async Task<(bool Ok, T Value, Exception Exception)> WaitValueAsync(params CancellationToken[] cancellationTokens)
            => await WaitValueAsync(default, false, cancellationTokens).ConfigureAwait(false);

        public async Task<(bool Ok, T Value, Exception Exception)> WaitValueAsync(T waitData, params CancellationToken[] cancellationTokens)
            => await WaitValueAsync(waitData, true, cancellationTokens).ConfigureAwait(false);
    }
}
