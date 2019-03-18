using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EifelMono.Fluent.Classes;

namespace EifelMono.Fluent.Test
{
    public class Xlock : IDisposable
    {
        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }

    public class Xlock<T> : Xlock
    {
        public Xlock(List<T> lockList) { _items = lockList;
        }
        private List<T> StartItems { get; set; }

        public async Task<Xlock<T>> WaitAsync(params T[] startItems)
        {
            StartItems = startItems.ToList();
            var signal = new TaskCompletionSourceQueued<bool>();
            void s_items_OnRemove()
            {
                lock (_items)
                {
                    foreach (var item in StartItems)
                        if (_items.Contains(item))
                        {
                            signal.NewData(false);
                            return;
                        }
                    foreach (var item in StartItems)
                        _items.Add(item);
                }
                signal.NewData(true);
            }

            try
            {
                lock (_items)
                    s_OnRemove += s_items_OnRemove;
                s_items_OnRemove();
                while (!(await signal.WaitDataAsync() is var result && result.Ok && result.Data))
                { }
            }
            finally
            {
                lock (_items)
                    s_OnRemove -= s_items_OnRemove;
            }
            return this;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (_items)
                {
                    foreach (var item in StartItems)
                        _items.Remove(item);
                    s_OnRemove?.Invoke();
                }
            }
            base.Dispose(disposing);
        }

        private readonly List<T> _items = new List<T>();
#pragma warning disable IDE1006 // Naming Styles
        public static event Action s_OnRemove;
#pragma warning restore IDE1006 // Naming Styles
    }

    public class XlockInt : Xlock<int>
    {
        public XlockInt() : base(s_items) { }
        static readonly List<int> s_items = new List<int>();
    }

    public class XlockPort : Xlock<int>
    {
        public XlockPort() : base(s_items) { }
        static readonly List<int> s_items = new List<int>();
    }

    public class XlockDirectory : Xlock<string>
    {
        public XlockDirectory() : base(s_items) { }
        static readonly List<string> s_items = new List<string>();
    }
}
