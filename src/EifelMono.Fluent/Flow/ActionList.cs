using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EifelMono.Fluent.Flow
{
    public abstract class ActionListCore
    {
        protected List<object> Items { get; set; } = new List<object>();

        protected object Add(object item)
        {
            lock (Items)
            {
                Items.Add(item);
            }
            return item;
        }

        public object Remove(object item)
        {
            lock (Items)
            {
                if (Items.Contains(item))
                    Items.Remove(item);
            }
            return item;
        }

        public void Clear()
        {
            lock (Items)
                Items.Clear();
        }

        public int Count
        {
            get
            {
                lock (Items)
                    return Items.Count;
            }
        }

        protected List<object> Clone()
        {
            lock (Items)
                return Items.Select(item => item).ToList();
        }

        protected (int Calls, int Errors) Invoke(Action<object> action)
        {
            int calls = 0;
            int errors = 0;
            Clone().ForEach(item =>
            {
                try
                {
                    calls++;
                    action?.Invoke(item);
                }
                catch
                {
                    errors++;
                }
            });
            return (calls, errors);
        }

        protected (int Calls, int Errors) InvokeParallel(Action<object> action)
        {
            object sync = new object();
            int calls = 0;
            int errors = 0;
#if NETSTANDARD1_6
            Clone().ForEach(item =>
            {
                Task.Run(() =>
                {
                    try
                    {
                        lock (sync)
                            calls++;
                        action?.Invoke(item);
                    }
                    catch
                    {
                        lock (sync)
                            errors++;
                    }
                });
            });
#else
            Parallel.ForEach(Clone(), item =>
            {
                try
                {
                    lock (sync)
                        calls++;
                    action?.Invoke(item);
                }
                catch
                {
                    lock (sync)
                        errors++;
                }
            });
#endif
            return (calls, errors);
        }
    }

    public class ActionList : ActionListCore
    {
        public Action Add(Action action)
        {
            base.Add(action);
            return action;
        }

        public Action Remove(Action action)
        {
            base.Remove(action);
            return action;
        }

        public (int Calls, int Errors) Invoke()
            => Invoke(item => (item as Action)?.Invoke());
        public (int Calls, int Errors) InvokeParallel()
            => InvokeParallel(item => (item as Action)?.Invoke());
    }

    public class ActionList<T1> : ActionListCore
    {
        public Action<T1> Add(Action<T1> action)
        {
            base.Add(action);
            return action;
        }

        public Action<T1> Remove(Action<T1> action)
        {
            base.Remove(action);
            return action;
        }

        public (int Calls, int Errors) Invoke(T1 arg1)
            => Invoke(item => (item as Action<T1>)?.Invoke(arg1));
        public (int Calls, int Errors) InvokeParallel(T1 arg1)
            => InvokeParallel(item => (item as Action<T1>)?.Invoke(arg1));
    }

    public class ActionList<T1, T2> : ActionListCore
    {
        public Action<T1, T2> Add(Action<T1, T2> action)
        {
            base.Add(action);
            return action;
        }

        public Action<T1, T2> Remove(Action<T1, T2> action)
        {
            base.Remove(action);
            return action;
        }

        public (int Calls, int Errors) Invoke(T1 arg1, T2 arg2)
            => Invoke(item => (item as Action<T1, T2>)?.Invoke(arg1, arg2));
        public (int Calls, int Errors) InvokeParallel(T1 arg1, T2 arg2)
            => InvokeParallel(item => (item as Action<T1, T2>)?.Invoke(arg1, arg2));
    }

    public class ActionList<T1, T2, T3> : ActionListCore
    {
        public Action<T1, T2, T3> Add(Action<T1, T2, T3> action)
        {
            base.Add(action);
            return action;
        }

        public Action<T1, T2, T3> Remove(Action<T1, T2, T3> action)
        {
            base.Remove(action);
            return action;
        }

        public (int Calls, int Errors) Invoke(T1 arg1, T2 arg2, T3 arg3)
            => Invoke(item => (item as Action<T1, T2, T3>)?.Invoke(arg1, arg2, arg3));
        public (int Calls, int Errors) InvokeParallel(T1 arg1, T2 arg2, T3 arg3)
            => InvokeParallel(item => (item as Action<T1, T2, T3>)?.Invoke(arg1, arg2, arg3));
    }

    public class ActionList<T1, T2, T3, T4> : ActionListCore
    {
        public Action<T1, T2, T3, T4> Add(Action<T1, T2, T3, T4> action)
        {
            base.Add(action);
            return action;
        }

        public Action<T1, T2, T3, T4> Remove(Action<T1, T2, T3, T4> action)
        {
            base.Remove(action);
            return action;
        }

        public (int Calls, int Errors) Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => Invoke(item => (item as Action<T1, T2, T3, T4>)?.Invoke(arg1, arg2, arg3, arg4));
        public (int Calls, int Errors) InvokeParallel(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => InvokeParallel(item => (item as Action<T1, T2, T3, T4>)?.Invoke(arg1, arg2, arg3, arg4));
    }

    public class ActionList<T1, T2, T3, T4, T5> : ActionListCore
    {
        public Action<T1, T2, T3, T4, T5> Add(Action<T1, T2, T3, T4, T5> action)
        {
            base.Add(action);
            return action;
        }

        public Action<T1, T2, T3, T4, T5> Remove(Action<T1, T2, T3, T4, T5> action)
        {
            base.Remove(action);
            return action;
        }

        public (int Calls, int Errors) Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => Invoke(item => (item as Action<T1, T2, T3, T4, T5>)?.Invoke(arg1, arg2, arg3, arg4, arg5));
        public (int Calls, int Errors) InvokeParallel(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => InvokeParallel(item => (item as Action<T1, T2, T3, T4, T5>)?.Invoke(arg1, arg2, arg3, arg4, arg5));
    }
}
