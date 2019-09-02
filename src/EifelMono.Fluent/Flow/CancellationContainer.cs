using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;

namespace EifelMono.Fluent.Flow
{
    public class CancellationContainerItem
    {
        public virtual CancellationToken Token { get; set; } = default;
        public CancellationTokenSource Source { get; set; } = null;

        public string Name { get; set; } = "";

        public override string ToString()
            => $"Name='{Name}' Type={GetType().Name.Replace("CancellationContainer", "")} IsCancellationRequested={Token.IsCancellationRequested}";
    }

    public class CancellationContainerSourceItem : CancellationContainerItem
    {
        public override CancellationToken Token { get => Source?.Token ?? default; }
    }

    public class CancellationContainerDisposeSourceItem : CancellationContainerSourceItem
    {
    }

    public class CancellationContainerDisposeTimeSpanItem : CancellationContainerDisposeSourceItem
    {
        public TimeSpan TimeSpan { get; set; }
    }

    public class CancellationContainer : IDisposable
    {
        public List<CancellationContainerItem> Items { get; private set; } = new List<CancellationContainerItem>();

        public CancellationContainer Add(CancellationToken cancellationToken, string name = "")
        {
            Items.Add(new CancellationContainerItem
            {
                Token = cancellationToken,
                Name = name
            });
            return this;
        }
        public CancellationContainer Add(CancellationTokenSource cancellationTokenSource, string name = "")
        {
            Items.Add(new CancellationContainerSourceItem
            {
                Source = cancellationTokenSource,
                Name = name
            });
            return this;
        }

        public CancellationContainer AddSource(string name = "")
        {
            Items.Add(new CancellationContainerDisposeSourceItem
            {
                Source = new CancellationTokenSource(),
                Name = name
            });
            return this;
        }

        public CancellationContainer AddTimeOut(TimeSpan timeSpan, string name = "")
        {
            Items.Add(new CancellationContainerDisposeTimeSpanItem
            {
                Source = new CancellationTokenSource(timeSpan),
                TimeSpan = timeSpan,
                Name = name
            });
            return this;
        }
        public CancellationContainer AddDaysTimeOut(double days, string name = "")
            => AddTimeOut(TimeSpan.FromDays(days), name);
        public CancellationContainer AddHoursTimeOut(double hours, string name = "")
            => AddTimeOut(TimeSpan.FromHours(hours), name);
        public CancellationContainer AddMinutesTimeOut(double minutes, string name = "")
            => AddTimeOut(TimeSpan.FromMinutes(minutes), name);
        public CancellationContainer AddSecondsTimeOut(double seconds, string name = "")
            => AddTimeOut(TimeSpan.FromSeconds(seconds), name);
        public CancellationContainer AddMillisecondsTimeOut(double milliseconds, string name = "")
            => AddTimeOut(TimeSpan.FromMilliseconds(milliseconds), name);
        public CancellationContainer AddTicksTimeOut(long value, string name = "")
            => AddTimeOut(TimeSpan.FromTicks(value), name);

        public (bool Ok, CancellationContainerItem item) Remove(string name)
        {
            if (Items.FirstOrDefault(i => i.Name == name) is var item && item is object)
            {
                Items.Remove(item);
                return (true, item);
            }
            return (false, null);
        }

        public (bool Ok, CancellationContainerItem item) Remove(CancellationToken cancellationToken)
        {
            if (Items.FirstOrDefault(i => i.Token == cancellationToken) is var item && item is object)
            {
                Items.Remove(item);
                return (true, item);
            }
            return (false, null);
        }

        public (bool Ok, CancellationContainerItem item) Remove(CancellationTokenSource cancellationTokenSource)
        {
            if (Items.FirstOrDefault(i => i.Source == cancellationTokenSource) is var item && item is object)
            {
                Items.Remove(item);
                return (true, item);
            }
            return (false, null);
        }

        public CancellationContainer SetDiposeAfterAction(bool isDisposeAfterAction)
        {
            IsDisposeAfterAction = isDisposeAfterAction;
            return this;
        }

        public bool IsDisposeAfterAction { get; set; } = true;
        public void DisposeAfterAction()
        {
            if (IsDisposeAfterAction)
                Dispose();
        }

        public void Dispose()
        {
            DisposeToken();
            DisposeDisposeableItems();
            Items.Clear();
        }

        public CancellationTokenSource TokenSource { get; protected set; } = null;

        public CancellationToken Token
        {
            get
            {
                if (TokenSource is null)
                    TokenSource = CancellationTokenSource
                        .CreateLinkedTokenSource(Items.Select(item => item.Token).ToArray());
                return TokenSource.Token;
            }
        }

        public void DisposeToken()
        {
            if (TokenSource is object)
            {
                TokenSource.Dispose();
                TokenSource = null;
            }
        }

        protected void HandleDisposeableItems(bool reset)
        {
            foreach (var item in Items)
                switch (item)
                {
                    case CancellationContainerItem coreItem
                        when coreItem.GetType() == typeof(CancellationContainerItem):
                        break;
                    case CancellationContainerSourceItem sourceItem
                        when sourceItem.GetType() == typeof(CancellationContainerSourceItem)
                            && sourceItem?.Source is object:
                        break;
                    case CancellationContainerDisposeSourceItem disposeSourceItem
                        when disposeSourceItem.GetType() == typeof(CancellationContainerDisposeSourceItem)
                            && disposeSourceItem?.Source is object:
                        disposeSourceItem.Source.Dispose();
                        if (reset)
                            disposeSourceItem.Source = new CancellationTokenSource();
                        break;
                    case CancellationContainerDisposeTimeSpanItem disposeTimeSpanItem
                        when disposeTimeSpanItem.GetType() == typeof(CancellationContainerDisposeTimeSpanItem)
                            && disposeTimeSpanItem?.Source is object:
                        disposeTimeSpanItem.Source.Dispose();
                        if (reset)
                            disposeTimeSpanItem.Source = new CancellationTokenSource(disposeTimeSpanItem.TimeSpan);
                        break;
                }
        }

        public void ResetDisposeableItems()
            => HandleDisposeableItems(reset: true);

        public void DisposeDisposeableItems()
            => HandleDisposeableItems(reset: false);

        public void Reset()
        {
            ResetDisposeableItems();
            DisposeToken();
        }

        public bool Cancel(string name)
        {
            if (Items.FirstOrDefault(i => i.Name == name) is CancellationContainerSourceItem item && item is object)
            {
                item.Source.Cancel();
                return true;
            }
            return false;
        }

        public bool IsCancellationRequested
            => Items.Where(i => i.Token.IsCancellationRequested).Any();

        public CancellationContainerItem this[string name]
            => Items.FirstOrDefault(i => i.Name == name);

        public List<CancellationContainerItem> CanceledItems
            => Items.Where(i => i.Token.IsCancellationRequested).ToList();
        public List<CancellationToken> CanceledItemsTokens
            => Items.Where(i => i.Token.IsCancellationRequested).Select(i => i.Token).ToList();
        public List<CancellationTokenSource> CanceledItemsSources
            => Items.Where(i => i.Token.IsCancellationRequested && i.Source is object)
                .Select(i => i.Source).ToList();

        public static async Task Delay(TimeSpan delay, CancellationContainer cancellationContainer)
        {
            try
            {
                await Task.Delay(delay, cancellationContainer.Token).ConfigureAwait(false);
            }
            finally
            {
                cancellationContainer.DisposeAfterAction();
            }
        }
        public static Task Delay(int millisecondsDelay, CancellationContainer cancellationContainer)
            => Delay(TimeSpan.FromMilliseconds(millisecondsDelay), cancellationContainer);

        public static Task WaitUntil(TimeSpan delay, CancellationContainer cancellationContainer)
            => Delay(delay, cancellationContainer);
        public static Task WaitUntil(int millisecondsDelay, CancellationContainer cancellationContainer)
            => Delay(millisecondsDelay, cancellationContainer);
        public static Task WaitUntil(CancellationContainer cancellationContainer)
            => Delay(-1, cancellationContainer);
    }
    public static class CancellationContainerExtensions
    {
        public const string ContainerRootName = "ContainerRoot";
        public static CancellationContainer NewContainer(this CancellationTokenSource thisValue, string name = ContainerRootName)
            => new CancellationContainer().Add(thisValue, name);
        public static CancellationContainer NewContainer(this CancellationToken thisValue, string name = ContainerRootName)
            => new CancellationContainer().Add(thisValue, name);
        public static CancellationContainer NewContainer(this TimeSpan thisValue, string name = ContainerRootName)
            => new CancellationContainer().AddTimeOut(thisValue, name);
    }
}
