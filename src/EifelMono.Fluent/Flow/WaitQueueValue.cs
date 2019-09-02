using System;
using System.Collections.Generic;
using System.Text;

namespace EifelMono.Fluent.Flow
{
    public class WaitQueueValue<T> : TaskCompletionQueuedSource<T>
    {
        public WaitQueueValue() : base()
        {
        }

        public WaitQueueValue(IEnumerable<T> collection) : base(collection)
        {
        }
    }
}
