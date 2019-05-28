using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EifelMono.Fluent.Log;

namespace EifelMono.Fluent.Flow
{
    public class TaskCompletionQueuedSource<T>
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

        public void AddData(T addData, CancellationToken cancellationToken = default)
            => NewData(addData, cancellationToken);

        protected async Task<(bool Ok, T Value, Exception Exception)> WaitValueAsync(T[] waitValues, bool wait, params CancellationToken[] cancellationTokens)
        {
            if (wait && (waitValues is null || waitValues.Length == 0))
                return (false, default, new AggregateException());
            CancellationToken cancellationToken;
            cancellationToken.Register(() => { CompletionSource.TrySetCanceled(); });
            var tokenRegisters = new List<CancellationTokenRegistration>();
            try
            {
                foreach (var ct in cancellationTokens)
                    tokenRegisters.Add(ct.Register(() => { CompletionSource.TrySetCanceled(); }));

                var waitValuesList = wait ? waitValues.ToList() : new List<T>();
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationTokens.ToList().Append(cancellationToken).ToArray());
                {
                    T value = default;
                    bool finished = false;
                    while (!finished)
                    {
                        if (Items.TryDequeue(out value))
                        {
                            if (wait)
                            {
                                if (waitValuesList.Contains(value))
                                    waitValuesList.Remove(value);
                                finished = waitValuesList.Count == 0;
                            }
                            else
                                finished = true;
                        }
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


        public Task<(bool Ok, T Value, Exception Exception)> WaitValueAsync(params CancellationToken[] cancellationTokens)
            => WaitValueAsync(new T[] { }, false, cancellationTokens);
        public Task<(bool Ok, T Value, Exception Exception)> WaitValueAsync(TimeSpan timeSpan)
            => WaitValueAsync(new T[] { }, false, new CancellationTokenSource(timeSpan).Token);


        public Task<(bool Ok, T Value, Exception Exception)> WaitValueAsync(T waitData1, params CancellationToken[] cancellationTokens)
            => WaitValueAsync(new T[] { waitData1 }, true, cancellationTokens);
        public Task<(bool Ok, T Value, Exception Exception)> WaitValueAsync(T waitData1, TimeSpan timeSpan)
            => WaitValueAsync(new T[] { waitData1 }, true, new CancellationTokenSource(timeSpan).Token);
        public async Task<(bool Ok, Exception Exception)> WaitValuesAsync(T[] waitValues, params CancellationToken[] cancellationTokens)
        {
            var result = await WaitValueAsync(waitValues, true, cancellationTokens).ConfigureAwait(false);
            return (result.Ok, result.Exception);
        }
        public Task<(bool Ok, Exception Exception)> WaitValuesAsync(T[] waitValues, TimeSpan timeSpan)
            => WaitValuesAsync(waitValues, new CancellationTokenSource(timeSpan).Token);
        public Task<(bool Ok, Exception Exception)> WaitValuesAsync(T waitData1, T waitData2, params CancellationToken[] cancellationTokens)
            => WaitValuesAsync(new T[] { waitData1, waitData2 }, cancellationTokens);
        public Task<(bool Ok, Exception Exception)> WaitValuesAsync(T waitData1, T waitData2, TimeSpan timeSpan)
            => WaitValuesAsync(new T[] { waitData1, waitData2 }, new CancellationTokenSource(timeSpan).Token);
        public Task<(bool Ok, Exception Exception)> WaitValuesAsync(T waitData1, T waitData2, T waitData3, params CancellationToken[] cancellationTokens)
            => WaitValuesAsync(new T[] { waitData1, waitData2, waitData3 }, cancellationTokens);
        public Task<(bool Ok, Exception Exception)> WaitValuesAsync(T waitData1, T waitData2, T waitData3, TimeSpan timeSpan)
            => WaitValuesAsync(new T[] { waitData1, waitData2, waitData3 }, new CancellationTokenSource(timeSpan).Token);
        public Task<(bool Ok, Exception Exception)> WaitValuesAsync(T waitData1, T waitData2, T waitData3, T waitData4, params CancellationToken[] cancellationTokens)
            => WaitValuesAsync(new T[] { waitData1, waitData2, waitData3, waitData4 }, cancellationTokens);
        public Task<(bool Ok, Exception Exception)> WaitValuesAsync(T waitData1, T waitData2, T waitData3, T waitData4, TimeSpan timeSpan)
            => WaitValuesAsync(new T[] { waitData1, waitData2, waitData3, waitData4 }, new CancellationTokenSource(timeSpan).Token);
        public Task<(bool Ok, Exception Exception)> WaitValuesAsync(T waitData1, T waitData2, T waitData3, T waitData4, T waitData5, params CancellationToken[] cancellationTokens)
            => WaitValuesAsync(new T[] { waitData1, waitData2, waitData3, waitData4, waitData5 }, cancellationTokens);
        public Task<(bool Ok, Exception Exception)> WaitValuesAsync(T waitData1, T waitData2, T waitData3, T waitData4, T waitData5, TimeSpan timeSpan)
            => WaitValuesAsync(new T[] { waitData1, waitData2, waitData3, waitData4, waitData5 }, new CancellationTokenSource(timeSpan).Token);
    }
}
