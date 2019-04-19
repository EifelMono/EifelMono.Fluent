using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace EifelMono.Fluent.Flow
{
    public class ConcurrentExtendedQueue<T> : ConcurrentQueue<T>
    {
        public ConcurrentExtendedQueue() : base() { }
        public ConcurrentExtendedQueue(IEnumerable<T> collection) : base(collection) { }

        public void Clear()
        {
            while (!IsEmpty)
                TryDequeue(out var _);
            return;
        }
        public int ClearUntil(int count)
        {
            int notCleared = 0;
            while (Count > count)
                if (!TryDequeue(out var _))
                    notCleared++;
            return notCleared;
        }
    }
}
